using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using So_Gen_Lokacije.Resources;
using System.Net;
using System.IO;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Flurl.Http;
using Windows.Devices.Geolocation;
using System.Linq;
using System.Device.Location;

namespace So_Gen_Lokacije.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _latitude = 0.0;
        private double _longitude = 0.0;
        private ItemViewModel selectedItem;
        private GeoCoordinate _geoCoordinate;

        public MainViewModel()
        {
            this.Items = new SortedObservableCollection<ItemViewModel>();
            this.Items.OrderBy(g => g.Distance);
            this.GeoCoordinate = new GeoCoordinate(44, 22);
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public SortedObservableCollection<ItemViewModel> Items { get; private set; }

        public ItemViewModel SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
        }

        public double Longitude
        {
            get
            {
                return this._longitude;
            }

            set
            {
                this._longitude = value;
                this.NotifyPropertyChanged("Longitude");               
            }
        }

        public double Latitude
        {
            get
            {
                return this._latitude;
            }

            set
            {
                this._latitude = value;
                this.NotifyPropertyChanged("Latitude");
            }
        }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public GeoCoordinate GeoCoordinate
        {
            get
            {
                return _geoCoordinate;
            }

            set
            {
                _geoCoordinate = value;
                this.NotifyPropertyChanged("GeoCoordinate");
            }
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            await GetCurrentLocation();
            var responseString = await "http://172.25.28.112/services/mobile_get_office.php"
                .PostUrlEncodedAsync(new { data = "{\"0\":\"+381638620648\"}", thing2 = "world" })
                .ReceiveString();

            String[] entries = responseString.Split(new char[] { ';', '~'});            
            
            for(int i = 0; i < entries.Length; i ++)
            {
                if (i == 0)
                {
                    // do something
                }
                else
                {
                    String[] tokens = entries[i].Split('|');
                    
                    try
                    {
                        this.Items.Add(new ItemViewModel()
                        {
                            Id = System.Convert.ToInt32(tokens[0]),
                            Latitude = System.Convert.ToDouble(tokens[2]),
                            Longitude = System.Convert.ToDouble(tokens[3]),
                            DistanceX = System.Math.Abs(this.Latitude - System.Convert.ToDouble(tokens[2])),
                            DistanceY = System.Math.Abs(this.Longitude - System.Convert.ToDouble(tokens[3])),
                            LineOne = tokens[1],
                            LineTwo = tokens[4] + ", " + String.Format("{0:0.0}", this.DistanceTo(System.Convert.ToDouble(tokens[2]), System.Convert.ToDouble(tokens[3]))) + " km",
                            LineThree = tokens[2] + ", " + tokens[3]
                            
                        });
                        
                    }
                    catch
                    {
                        continue;
                    }
                }
                                                
            }
                  
            this.IsDataLoaded = true;
            this.NotifyPropertyChanged("GeoCoordinate");
        }

        private double toRad(double grad)
        {
            return (grad / 360) * 2 * Math.PI;
        }

        private double DistanceTo(double latitude, double longitude)
        {
            var R = 6371; // km

            var dLat = toRad((latitude - this.Latitude));
            var dLon = toRad((longitude - this.Longitude));
            var lat1 = toRad(this.Latitude);
            var lat2 = toRad(latitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }

        /// <summary>
        /// Gets my location.
        /// </summary>
        private async Task GetCurrentLocation()
        {
            Geolocator locationFinder = new Geolocator
            {
                DesiredAccuracyInMeters = 50,
                DesiredAccuracy = PositionAccuracy.Default
            };

            try
            {
                Geoposition currentLocation = await locationFinder.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromSeconds(128),
                    timeout: TimeSpan.FromSeconds(10));
                this.Latitude = currentLocation.Coordinate.Point.Position.Latitude;
                this.Longitude = currentLocation.Coordinate.Point.Position.Longitude;

                // This is until we set the applicaiton on the phone,
                // because the emulator is giving the wrong values.
                if(this._latitude == 0)
                    this._latitude = 44.8044219;
                if(this._longitude == 0)
                    this._longitude = 20.3995146;

                GeoCoordinate c = CoordinateConverter.ConvertGeocoordinate(currentLocation.Coordinate);
                if(c.Longitude < 0)
                {
                    c.Latitude = 44.8044219;
                    c.Longitude = 20.3995146;
                }
                
                this.GeoCoordinate = c;

            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("An Exception Occured");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}