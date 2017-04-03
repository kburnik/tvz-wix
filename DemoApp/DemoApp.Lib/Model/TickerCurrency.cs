/*
 * MIT License
 * Copyright (c) 2017 Kristijan Burnik
 * Please refer to the LICENSE file in project root.
 */
namespace DemoApp.Lib.Model
{
    /// <summary>
    /// Represents the ticker data for a single currency.
    /// </summary>
    public class TickerCurrency
    {
        public int Id { get; set; }
        public string Last { get; set; }
        public string LowestAsk { get; set; }
        public string HighestBid { get; set; }
        public string PercentChange { get; set; }
        public string BaseVolume { get; set; }
        public string QuoteVolume { get; set; }
        public string IsFrozen { get; set; }
        public string High24hr { get; set; }
        public string Low24hr { get; set; }
    }
}
