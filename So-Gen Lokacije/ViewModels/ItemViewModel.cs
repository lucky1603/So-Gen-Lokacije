using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace So_Gen_Lokacije.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged, IComparable<ItemViewModel>
    {
        private int _id;
        private SortedObservableCollection<ServiceViewModel> _services;

        /// <summary>
        /// Id of the item. This is not displayed as the separate line, but will be used in the next step
        /// in order to get the desired option from the server.
        /// </summary>
        /// <r
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Services attached to this location.
        /// </summary>
        public SortedObservableCollection<ServiceViewModel> Services
        {
            get
            {
                if(this._services == null)
                {
                    this._services = new SortedObservableCollection<ServiceViewModel>();
                }

                return this._services;
            }
        }

        private double _longitude;

        /// <summary>
        /// Gets/sets longitude value.
        /// </summary>
        public double Longitude
        {
            get
            {
                return _longitude;
            }

            set
            {
                _longitude = value;
            }
        }

        private double _latitude;
        
        /// <summary>
        /// Gets/sets latitude value.
        /// </summary>
        public double Latitude
        {
            get
            {
                return _latitude;
            }

            set
            {
                _latitude = value;
            }
        }

        private string _lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        private double distanceX;

        public double DistanceX
        {
            get
            {
                return distanceX;
            }

            set
            {
                distanceX = value;
            }
        }

        private double distanceY;

        public double DistanceY
        {
            get
            {
                return distanceY;
            }

            set
            {
                distanceY = value;
            }
        }

        /// <summary>
        /// Return the distance to my position.
        /// </summary>
        public double Distance
        {
            get
            {
                return Math.Sqrt(Math.Pow(DistanceX, 2) + Math.Pow(DistanceY, 2));
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

        public int CompareTo(ItemViewModel other)
        {
            if (other.Distance > this.Distance)
                return -1;
            else
                return 1;
        }

        public async Task GetLocationData()
        {
            Services.Clear();

            Services.Add(new ServiceViewModel()
            {
                Id = 1,
                Description = "Uplata/isplata - privatna lica",
                Queued = 5
            });

            Services.Add(new ServiceViewModel()
            {
                Id = 2,
                Description = "Uplata/isplata - pravna lica",
                Queued = 7
            });

            Services.Add(new ServiceViewModel()
            {
                Id = 3,
                Description = "Uplata/isplata - pravna lica",
                Queued = 2
            });
        }
    }
}