using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Lib.Model
{
    public class TickerItem
    {
        public string CurrencyPair { get; set; }
        public TickerCurrency Details { get; set; }
    }
}
