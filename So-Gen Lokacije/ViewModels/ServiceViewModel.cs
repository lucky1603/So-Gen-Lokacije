using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace So_Gen_Lokacije.ViewModels
{
    public class ServiceViewModel : INotifyPropertyChanged, IComparable<ServiceViewModel>
    {

        private int id;
        private string orderMark;
        private string description;
        private int queued;
        private double minutesEstimated;

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

        public double MinutesEstimated
        {
            get
            {
                return minutesEstimated;
            }

            set
            {
                minutesEstimated = value;
            }
        }

        public string OrderMark
        {
            get
            {
                return orderMark;
            }

            set
            {
                orderMark = value;
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

        public async Task MakeReservation(int poslovnica)
        {
            using (var client = new HttpClient())
            {
                String[] args = new String[] { "063297167", "1" };

                var values = new Dictionary<string, string>
                {
                   { "data[0]", "+381638620648" },
                   { "data[1]", poslovnica.ToString() },
                   { "data[2]", OrderMark }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("http://172.25.28.112/services/mobile_reservation.php", content);
                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
