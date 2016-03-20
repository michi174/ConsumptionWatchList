using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ConsumptionWatchList.ViewModels;
using System.Threading.Tasks;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ConsumptionWatchList.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Items : Page
    {
        public DefaultItemsViewModel DefaultItems { set; get; }

        public Items()
        {
            this.InitializeComponent();
        }

        public async Task<string> DoBusyThings()
        {
            await Task.Delay(5000);

            return "...you must wait, but at least i don't block the UX...";
        }

        private async void btnTest_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Task<string> asyncString = this.DoBusyThings();
            this.testblockAsync.Text = "Loading...";

            await Task.Delay(500);
            this.testblock.Text = "I run instantly...";

            while (!asyncString.IsCompleted)
            {
                await Task.Delay(500);
                this.testblockAsync.Text += ".";
            }

            this.testblockAsync.Text = await asyncString;
            this.testblockAfter.Text = "...i have to wait for the line above me \nbecause I'm depend it!";
        }
    }
}
