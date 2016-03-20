using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ConsumptionWatchList.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ConsumptionWatchList.Common;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;



// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace ConsumptionWatchList
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DefaultItemsViewModel AllItems { set; get; }

        public MainPage()
        {
            this.InitializeComponent();
            this.AllItems = new DefaultItemsViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainPivot.SelectedItem = AddItems;
        }

        private async void btnUpdate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ItemViewModel uItem = new ItemViewModel() { ID = AllItems.SelectedItem.ID, Name = this.ItemName.Text, Symbol = this.ItemSymbol.Text, Amount = 1 };
            await this.AllItems.UpdateAsync(uItem);
        }

        private async void btnActivateUpdate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.changeUpdateBoxVisibilty(true);
        }

        private async void MainList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await this.changeUpdateBoxVisibilty(false);
        }

        private async Task changeUpdateBoxVisibilty(bool visible)
        {
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Showing += (s, args) =>
            {
                this.cmdBar.Visibility = Visibility.Collapsed;
            };
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Hiding += (s, args) =>
            {
                this.cmdBar.Visibility = Visibility.Visible;
            };

            if (visible)
            {
                await this.CreateGlassEffect();

                this.UpdateBox.Visibility = Visibility.Visible;
                this.btnActivateUpdate.Content = "Abbrechen";
            }
            else
            {
                this.UpdateBox.Visibility = Visibility.Collapsed;
                this.btnActivateUpdate.Content = "Bearbeiten";
            }
        }

        private async void btnAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.changeUpdateBoxVisibilty(true);
        }

        private async void btnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.changeUpdateBoxVisibilty(false);
        }

        private async Task CreateGlassEffect()
        {
            using (var stream = await Content.RenderToRandomAccessStreamAsync())
            {
                var device = new CanvasDevice();
                var bitmap = await CanvasBitmap.LoadAsync(device, stream);

                var renderer = new CanvasRenderTarget(device, bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height, bitmap.Dpi);

                using (var ds = renderer.CreateDrawingSession())
                {
                    var blur = new GaussianBlurEffect();
                    blur.BlurAmount = 10.0f;
                    blur.Source = bitmap;
                    ds.DrawImage(blur);

                }

                stream.Seek(0);
                await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                ImageBrush bgImage = new ImageBrush();
                bgImage.ImageSource = image;
                this.UpdateBox.Background = bgImage;
            }
        }

        private async void UpdateBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //await this.CreateGlassEffect();
        }

        private void abbSelect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.MainList.IsMultiSelectCheckBoxEnabled = true;
        }

        private void SearchBoxToggle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.FilterBox.Visibility == Visibility.Visible)
            {
                this.FilterBox.Visibility = Visibility.Collapsed;
                this.SearchBoxToggleIcon.Glyph = "\uE011";
                this.SearchBoxToggleText.Text = "Erwiterte Suche";
            }
            else
            {
                this.FilterBox.Visibility = Visibility.Visible;
                this.SearchBoxToggleIcon.Glyph = "\uE010";
                this.SearchBoxToggleText.Text = "Erweiterte Suche ausblenden";
            }
            
        }
    }
}
