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
            exitCommand = new Command(Exit);
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
        public ICommand exitCommand { get; }
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
        private CancellationTokenSource tokenSource;

        public string LabelReactionTime { get => labelReactionTime; set => SetProperty(ref labelReactionTime, value); }
        public string Count { get => count; set => SetProperty(ref count, value); }
        public bool IsRetryButtonVisible { get => isRetryButtonVisible; set => SetProperty(ref isRetryButtonVisible, value); }
        public bool IsStartButtonVisible { get => isStartButtonVisible; set => SetProperty(ref isStartButtonVisible, value); }
        public bool IsExitButtonVisible { get => isExitButtonVisible; set => SetProperty(ref isExitButtonVisible, value); }
        public long ReactionTime { get => reactionTime; set => SetProperty(ref reactionTime, value); }


        //zvuk jako Enumerace
        // přehrání zvuku do Interfacu


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
            tokenSource = new CancellationTokenSource();
            ShowHideStartButton();
            ShowHideExitButton();

            MakeSound(Sound.Pripravit);
            bool returnNow = false;
            await Task.Delay(5000, tokenSource.Token).ContinueWith(tsk =>
            {
                if (tokenSource.IsCancellationRequested)
                {
                    Retry();
                    returnNow = true;
                }
            });
            if (returnNow)
                return;

            Accelerometer.Start(SensorSpeed.UI);
            MakeSound(Sound.Pozor);
            await Task.Delay(rnd.Next(1500, 2501));

            swReaction.Start();
            MakeSound(Sound.Vystrel);
        }

        Dictionary<Sound, string> soundCommands = new Dictionary<Sound, string>()
        {
            { Sound.Pozor,"pozor" },{ Sound.Pripravit,"připravit" },{ Sound.Vystrel,"výstřel" },
        };

        public void MakeSound(Sound sound)
        {
            Count = soundCommands[sound];
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

        private void Exit()
        {
            tokenSource.Cancel();
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
