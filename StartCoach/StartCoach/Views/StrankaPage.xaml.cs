using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using StartCoach.Services;
using StartCoach.ViewModels;

namespace StartCoach.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrankaPage : ContentPage
    {
        public StrankaPage()
        {
            InitializeComponent();
            BindingContext = new StrankaViewModel(DependencyService.Get<IAudioPlayerService>());
        }
        
    }
}