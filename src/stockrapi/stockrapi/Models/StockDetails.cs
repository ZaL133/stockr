using Polygon.Net;

namespace stockrapi.Models
{
    public class StockDetails
    {
        public string Ticker { get; set; }
        public DailyOpenCloseResponse DailyOpenClose { get; set; }
        public TickerDetailsResponse TickerDetails { get; set; }
        public AggregatesBarsResponse DailyBars { get; set; }
        public AggregatesBarsResponse MonthlyBars { get; set; }
        public StockDividendsResponse Dividends { get; set; }
    }
}
