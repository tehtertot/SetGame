using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Set
{
    public class ScoreGrouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        
        public ScoreGrouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
            {
                this.Items.Add(item);
            }
        }
    }
}
