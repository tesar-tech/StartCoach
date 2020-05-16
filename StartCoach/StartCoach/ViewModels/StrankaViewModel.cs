using Android.OS;
using StartCoach.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StartCoach.ViewModels
{
    public class StrankaViewModel : BaseViewModel
    {
        public StrankaViewModel(IAudioPlayerService audioPlayer)
        {
            startCommand = new Command(StartAsync);
            retryCommand = new Command(Retry);
            HideStopButtonCommand = new Command(ShowHideRetryButton);
            HideStartButtonCommand = new Command(ShowHideStartButton);

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

            _audioPlayer = audioPlayer;
            _audioPlayer.OnFinishedPlaying = () => {
                _isStopped = true;
            };
            _isStopped = true;
        }

        public ICommand startCommand { get; }
        public ICommand retryCommand { get; }
        public ICommand HideStopButtonCommand { get; }
        public ICommand HideStartButtonCommand { get; }

        private string labelReactionTime;
        private string count = "";
        private long reactionTime;
        private float avrg1;
        private float avrg2;
        private Random rnd = new Random();
        Stopwatch swReaction = new Stopwatch();
        private bool isRetryButtonVisible = false;//defaultní hodnota
        private bool isStartButtonVisible = true;
        private bool isExitButtonVisible = false;

        private IAudioPlayerService _audioPlayer;
        private bool _isStopped;

        public string LabelReactionTime { get => labelReactionTime; set => SetProperty(ref labelReactionTime, value); }
        public string Count { get => count; set => SetProperty(ref count, value); }
        public bool IsRetryButtonVisible { get => isRetryButtonVisible; set => SetProperty(ref isRetryButtonVisible, value); }
        public bool IsStartButtonVisible { get => isStartButtonVisible; set => SetProperty(ref isStartButtonVisible, value); }
        public bool IsExitButtonVisible { get => isExitButtonVisible; set => SetProperty(ref isExitButtonVisible, value); }
        public long ReactionTime { get => reactionTime; set => SetProperty(ref reactionTime, value); }




        //nemusí být public
        private void ShowHideRetryButton()
        {
            IsRetryButtonVisible = !IsRetryButtonVisible;//přehození viditelnosti 
        }

        private void ShowHideStartButton()
        {
            IsStartButtonVisible = !IsStartButtonVisible;
        }

        private void ShowHideExitButton()
        {
            IsExitButtonVisible = !IsExitButtonVisible;
        }

        public async void StartAsync()
        {
            ShowHideStartButton();
            ShowHideExitButton();

            MakeSound(Sound.Pripravit);
            await Task.Delay(5000);

            Accelerometer.Start(SensorSpeed.UI);
            MakeSound(Sound.Pozor);
            await Task.Delay(rnd.Next(1500, 2501));

            swReaction.Start();
            MakeSound(Sound.Vystrel);
        }

        public void MakeSound(Sound sound)
        {
            if (sound == Sound.Pripravit)
            {
                PlayAudio(sound);
                Count = "připravit";
            }
            if (sound == Sound.Pozor)
            {
                PlayAudio(sound);
                Count = "pozor";
            }
            if (sound == Sound.Vystrel)
            {
                PlayAudio(sound);
                Count = "výstřel";
            }
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            avrg1 = (data.Acceleration.X + data.Acceleration.Y + data.Acceleration.Z) / 3;

            if (swReaction.IsRunning)
            {
                if (avrg1 > (1.05 * avrg2))
                {
                    swReaction.Stop();
                    reactionTime = swReaction.ElapsedMilliseconds;
                    LabelReactionTime = reactionTime.ToString() + " milliseconds";
                    ShowHideRetryButton();
                }
            }
            avrg2 = avrg1;
        }

        public void PlayAudio(Sound sound)
        {
            if (_isStopped)
            {
                _isStopped = false;
                _audioPlayer.Play(sound);
            }
            else
            {
                _audioPlayer.Pause();
                _audioPlayer.Play(sound);
            }
        }

        public void Retry()
        {
            Count = "";
            LabelReactionTime = "";
            Accelerometer.Stop();
            IsRetryButtonVisible = false;
            IsStartButtonVisible = true;
            IsExitButtonVisible = false;
            swReaction.Reset();
        }

        public enum Sound
        {
            Pripravit,
            Pozor,
            Vystrel
        }
    }
}
