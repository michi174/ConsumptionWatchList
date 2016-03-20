using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace ConsumptionWatchList
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        public static NavigationRootPage Current;

        public static SplitView MainSplitView
        {
            get { return Current.mainSplitView; }
        }

        public NavigationRootPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private void tests_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.rootFrame.Navigate(typeof(Views.Items));
        }

        private void currentList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.rootFrame.Navigate(typeof(MainPage));
        }

        private void NavListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.mainSplitView.IsPaneOpen = false;
        }
    }
}
