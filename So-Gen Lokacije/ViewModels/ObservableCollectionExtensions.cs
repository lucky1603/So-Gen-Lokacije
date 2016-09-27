using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace So_Gen_Lokacije.ViewModels
{
    public static class ObservableCollectionExtensions
    {

        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable<T>
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            for (var startIndex = 0; startIndex < collection.Count - 1; startIndex += 1)
            {
                var indexOfSmallestItem = startIndex;
                for (var i = startIndex + 1; i < collection.Count; i += 1)
                    if (collection[i].CompareTo(collection[indexOfSmallestItem]) < 0)
                        indexOfSmallestItem = i;
                if (indexOfSmallestItem != startIndex)
                    collection.Move(indexOfSmallestItem, startIndex);
            }
        }

    }
}
