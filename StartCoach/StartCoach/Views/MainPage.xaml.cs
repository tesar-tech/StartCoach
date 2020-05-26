using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using StartCoach.Models;
using Xamarin.Essentials;

namespace StartCoach.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            //přidám tam první stránku.. Je to trochu nešikovné, protože defaultní stránka se 
            //nastavuje v xamlu 
            MenuPages.Add((int)MenuItemType.Stranka, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {//tahle metoda se zavolá (z menuPage), pokud něajkou stránku opouštím
            //v id je stránka na kterou odcházím
            if (!MenuPages.ContainsKey(id))// pokud není ve slovníku, tak se do něj  přidá
            {
                switch (id)
                {
                    case (int)MenuItemType.Stranka:
                        MenuPages.Add(id, new NavigationPage(new StrankaPage()));
                        break;
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];// v newPage je instance na stránku na kterou chci jít

            if (newPage != null && Detail != newPage) // pokud tam nejsem ...
            {
                Detail = newPage;//Detail je properta MasterDetailPage (ze které tato MainPage dědí)
                //když se zmení Detail, tak proběhne přeskočení stránky (když tohle zakomentuješ, tak to nikam neskočí )

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}