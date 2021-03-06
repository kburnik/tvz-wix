﻿/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
namespace DemoApp.Lib.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a model wrapping the currency pair (key) and the details.
    /// Suitable for keeping in a list-like structure.
    /// </summary>
    public class TickerItem
    {
        public string CurrencyPair { get; set; }
        public TickerCurrency Details { get; set; }
    }
}
