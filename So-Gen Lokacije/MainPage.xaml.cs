using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using So_Gen_Lokacije.Resources;
using So_Gen_Lokacije.ViewModels;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;

namespace So_Gen_Lokacije
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            App.ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "GeoCoordinate")
            {
                DrawMapLocations();
            }
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
                DrawMapLocations();
            }
        }

        /// <summary>
        /// Detect selection in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender == this.ServiceList)
            {
                ServiceViewModel svm = (ServiceViewModel)this.ServiceList.SelectedItem;
                ItemViewModel ivm = (ItemViewModel)this.LocationList.SelectedItem;
                string message = "Zahtevali ste rezervaciju servisa - " + svm.Description + ". Jeste li sigurni da hocete da nastavite?";
                if(MessageBox.Show(message, "Provera", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    svm.MakeReservation(ivm.Id);
                    message = "Poslali ste zahtev za rezervacijom servisa - " + svm.Description + ". Uskoro ćete kao potvrdu primiti SMS poruku sa vašim brojem na listi čekanja. Sačuvajte tu poruku.";
                    MessageBox.Show(message, "Potvrda", MessageBoxButton.OK);
                }
                                
                this.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                this.LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
            }
            else if(sender == this.LocationList)
            {
                if (((LongListSelector)sender).SelectedItem.GetType().Equals(typeof(ItemViewModel)))
                {
                    ItemViewModel ivm = (ItemViewModel)((LongListSelector)sender).SelectedItem;
                    await ivm.GetLocationData();
                    this.SecondPanel.DataContext = ivm;
                    this.LayoutRoot.RowDefinitions[1].Height = new GridLength(0);
                    this.LayoutRoot.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                }
            }
            
        }
       
        private async void ServiceList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender == this.ServiceList)
            {
                ServiceViewModel svm = (ServiceViewModel)this.ServiceList.SelectedItem;
                ItemViewModel ivm = (ItemViewModel)this.LocationList.SelectedItem;
                if(svm.Id != -1)
                {
                    string message = "Zahtevali ste rezervaciju servisa - " + svm.Description + ". Jeste li sigurni da hocete da nastavite?";
                    if (MessageBox.Show(message, "Provera", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        svm.MakeReservation(ivm.Id);
                        message = "Poslali ste zahtev za rezervacijom servisa - " + svm.Description + ". Uskoro ćete kao potvrdu primiti SMS poruku sa vašim brojem na listi čekanja. Sačuvajte tu poruku.";
                        MessageBox.Show(message, "Potvrda", MessageBoxButton.OK);
                    }
                }
                
                this.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                this.LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
            }
            else if (sender == this.LocationList)
            {
                if (((LongListSelector)sender).SelectedItem.GetType().Equals(typeof(ItemViewModel)))
                {
                    ItemViewModel ivm = (ItemViewModel)((LongListSelector)sender).SelectedItem;
                    await ivm.GetLocationData();
                    this.SecondPanel.DataContext = ivm;
                    this.LayoutRoot.RowDefinitions[1].Height = new GridLength(0);
                    this.LayoutRoot.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                }
            }
        }

        private void DrawMapLocations()
        {
            if(this.MyMap.Layers.Count > 0)
            {
                this.MyMap.Layers.Clear();
            }

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            Rectangle myRect = new Rectangle();
            myRect.Fill = new SolidColorBrush(Colors.Red);
            myRect.Height = 15;
            myRect.Width = 15;
            myRect.Opacity = 50;

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = App.ViewModel.GeoCoordinate;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);
            
            // Add the MapLayer to the Map.
            this.MyMap.Layers.Add(myLocationLayer);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}