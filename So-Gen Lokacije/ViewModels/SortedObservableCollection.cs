using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace So_Gen_Lokacije.ViewModels
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
        where T : IComparable<T>
    {
        protected override void InsertItem(int index, T item)
        {
            for (var i = 0; i < Count; i++)
            {
                switch (Math.Sign(this[i].CompareTo(item)))
                {
                    case 0:
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;

                }
            }
            base.InsertItem(Count, item);
        }
    }
}
