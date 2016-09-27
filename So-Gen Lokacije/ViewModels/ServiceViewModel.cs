using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace So_Gen_Lokacije.ViewModels
{
    public class ServiceViewModel : INotifyPropertyChanged, IComparable<ServiceViewModel>
    {

        private int id;
        private string description;
        private int queued;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public int Queued
        {
            get
            {
                return queued;
            }

            set
            {
                queued = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if(null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int CompareTo(ServiceViewModel other)
        {
            if (other.Id > Id)
                return -1;
            else
                return 1;
        }
    }
}
