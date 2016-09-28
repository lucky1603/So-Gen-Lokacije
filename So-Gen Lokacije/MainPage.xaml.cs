﻿using System;
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

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
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
                this.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                this.LayoutRoot.RowDefinitions[2].Height = new GridLength(0);
            }
            else
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