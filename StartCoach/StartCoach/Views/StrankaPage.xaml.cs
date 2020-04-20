using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace StartCoach.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrankaPage : ContentPage
    {
        public StrankaPage()
        {
            InitializeComponent();

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            LabelX.Text = data.Acceleration.X.ToString();
            LabelY.Text = data.Acceleration.Y.ToString();
            LabelZ.Text = data.Acceleration.Z.ToString();
        }

        public void ToggleAccelerometer(object sender, System.EventArgs e)
        {
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                Accelerometer.Stop();

            }
            else
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.UI);
            }
        }
    }
}