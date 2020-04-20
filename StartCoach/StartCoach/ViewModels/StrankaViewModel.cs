using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StartCoach.ViewModels
{
    public class StrankaViewModel : BaseViewModel
    {
        public StrankaViewModel()
        {
            testCommand = new Command(testMethod);
        }

        public ICommand testCommand { get; }
        private string test = "test";

        public string Test { get => test; set => SetProperty(ref test, value); }

        public void testMethod()
        {
            Test = DateTime.Now.ToString();
        }
        
    }
}
