using Android.OS;
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
        public StrankaViewModel()
        {
            startCommand = new Command(StartAsync);
            retryCommand = new Command(Retry);
            HideStopButtonCommand = new Command(ShowHideRetryButton);
            HideStartButtonCommand = new Command(ShowHideStartButton);

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
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

        public string LabelReactionTime { get => labelReactionTime; set => SetProperty(ref labelReactionTime, value); }
        public string Count { get => count; set => SetProperty(ref count, value); }
        public bool IsRetryButtonVisible { get => isRetryButtonVisible; set => SetProperty(ref isRetryButtonVisible, value); }
        public bool IsStartButtonVisible { get => isStartButtonVisible; set => SetProperty(ref isStartButtonVisible, value); }
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

        public async void StartAsync()
        {
            ShowHideStartButton();

            MakeSound("pripravit");
            await Task.Delay(5000);

            Accelerometer.Start(SensorSpeed.UI);
            MakeSound("pozor");
            await Task.Delay(rnd.Next(1500, 2501));

            swReaction.Start();
            MakeSound("vystrel");
        }

        public void MakeSound(String sound)
        {
            if (sound == "pripravit") Count = "připravit";
            if (sound == "pozor") Count = "pozor";
            if (sound == "vystrel") Count = "výstřel";
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
            ShowHideRetryButton();
            ShowHideStartButton();
            swReaction.Reset();
        }
    }
}
