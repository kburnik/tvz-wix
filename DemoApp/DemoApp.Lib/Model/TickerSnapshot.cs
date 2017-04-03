/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
namespace DemoApp.Lib.Model
{
    using System.Collections.Generic;

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
