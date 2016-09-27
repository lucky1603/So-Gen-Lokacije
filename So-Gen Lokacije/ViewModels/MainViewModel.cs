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

namespace So_Gen_Lokacije.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _latitude = 0.0;
        private double _longitude = 0.0;

        public MainViewModel()
        {
            this.Items = new SortedObservableCollection<ItemViewModel>();
            this.Items.OrderBy(g => g.Distance);                       
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public SortedObservableCollection<ItemViewModel> Items { get; private set; }

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

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            await GetCurrentLocation();
            var responseString = await "http://172.25.28.112/services/mobile_get_office.php"
                .PostUrlEncodedAsync(new { data = "{\"0\":\"063297167\"}", thing2 = "world" })
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
                            LineTwo = tokens[4],
                            LineThree = tokens[2] + ", " + tokens[3]
                        });
                        
                    }
                    catch
                    {
                        continue;
                    }
                }
                                                
            }
            
            
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            this.IsDataLoaded = true;
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
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("An Exception Occured");
            }
        }

        //private async Task test()
        //{
        //    string strUrl = "http://172.25.28.112/services/mobile_get_office.php";
        //    WebRequest request = HttpWebRequest.Create(strUrl);
        //    request.ContentType = "POST";

        //    HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        //    Stream s = (Stream)response.GetResponseStream();
        //    StreamReader readStream = new StreamReader(s);
        //    string dataString = readStream.ReadToEnd();
        //    response.Close();
        //    s.Close();
        //    readStream.Close();
        //}

        //void PostJsonRequestWebClient()
        //{
        //    WebClient webclient = new WebClient();
        //    Uri uristring = new Uri("http://172.25.28.112/services/mobile_get_office.php"); 
        //    webclient.Headers["ContentType"] = "application/json";
        //    string WebUrlRegistration = "";
        //    //string JsonStringParams = "{\"data\":\"{\"0\":\"{\"063297167\"}\"}";
        //    string JsonStringParams = "{\"data\":\"{\"0\":\"063 297 167\"}\"}";
        //    webclient.UploadStringCompleted += wc_UploadStringCompleted;
        //    //Post data like this                                                                           
        //    webclient.UploadStringAsync(uristring, "POST", JsonStringParams); 
        //}
        //private void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result != null)
        //        {
        //            string responce = e.Result.ToString();

        //            //To Do Your functionality 
        //            // Sample data; replace with real data
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
        //            this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

        //            this.IsDataLoaded = true;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}



        //private void setMapLocation(String longitude, String latitude)
        //{
        //    var latlon = latitude + "," + longitude;
        //    var latlon1 = "44.8044219,20.3995146";
        //    var img_url = "http://maps.googleapis.com/maps/api/staticmap?center=" + latlon1 + " &zoom=16&size=1000x800&sensor=true";
        //    var content = "<html><body><p><img src='" + img_url + "' width=\"100%\"/></p></body></html>";
        //    try
        //    {
        //        //this.MyBrowser.Navigate(new Uri(img_url));
        //        this.MyBrowser.NavigateToString(content);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }

        //}

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