using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Lib.Model
{
    /// <summary>
    /// Represents an entire ticker (map of all currencies) in a given time.
    /// </summary>
    public class TickerSnapshot : Dictionary<string, TickerCurrency>
    {

        private List<TickerItem> _items;

        public List<TickerItem> Items
        {
            get
            {
                if (this._items == null)
                {
                    this._items = new List<TickerItem>();
                    foreach (var kvp in this)
                    {
                        this._items.Add(new TickerItem()
                        {
                            CurrencyPair = kvp.Key,
                            Details = kvp.Value
                        });
                    }
                }

                return this._items;
            }
        }
    }
}
