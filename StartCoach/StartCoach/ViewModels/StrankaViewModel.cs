using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;
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
            startCommand = new Command(StartAccelerometer);
            stopCommand = new Command(StopAccelerometer);
        }

        public ICommand testCommand { get; }
        public ICommand startCommand { get; }
        public ICommand stopCommand { get; }
        
        private string labelStop;
        private string labelX;
        private string labelY;
        private string labelZ;
        private string count = "";
        private int reactionTime;

        public string LabelStop { get => labelStop; set => SetProperty(ref labelStop, value); }
        public string LabelX { get => labelX; set => SetProperty(ref labelX, value); }
        public string LabelY { get => labelY; set => SetProperty(ref labelY, value); }
        public string LabelZ { get => labelZ; set => SetProperty(ref labelZ, value); }
        public string Count { get => count; set => SetProperty(ref count, value); }
        public int ReactionTime { get => reactionTime; set => SetProperty(ref reactionTime, value); }



        

        public void StartAccelerometer()
        {
            Accelerometer.Start(SensorSpeed.UI);
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                if (Count == "")
                {
                    Count = "3";
                    return true;
                }
                if (Count == "3")
                {
                    Count = "2";
                    return true;
                }
                if (Count == "2")
                {
                    Count = "1";
                    return true;
                }
                if (Count == "1")
                {
                    Count = "START";
                    LabelStop = "Timer elapsed.";

                    ReactionTime = MeasureReaction();
                    return false;
                }
                else return false;
            }); ;


        }

        public int MeasureReaction()
        {
            return 0;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            LabelX = data.Acceleration.X.ToString();
            LabelY = data.Acceleration.Y.ToString();
            LabelZ = data.Acceleration.Z.ToString();
        }

        public void StopAccelerometer()
        {
            Count = "";
            LabelStop = "";
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            Accelerometer.Stop();
        }
        
 
    }
}
