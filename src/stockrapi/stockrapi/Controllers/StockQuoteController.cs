using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Polygon.Net;
using stockrapi.Models;
using stockrapi.Plumbing;
using System.Diagnostics.Eventing.Reader;

namespace stockrapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockQuoteController : ControllerBase
    {
        private readonly ILogger<StockQuoteController> _logger;
        private static Dictionary<string, StockDetails> _cache = new Dictionary<string, StockDetails>();
        private StockerPolygonDependencies _polygonDependencies;
        private PolygonClient _client;

        public StockQuoteController(ILogger<StockQuoteController> logger,
                                    IConfiguration config,
                                    IHttpClientFactory httpClientFactory,
                                    IMapper mapper)
        {
            _logger = logger;

            _polygonDependencies                    = new StockerPolygonDependencies();
            _polygonDependencies.Settings.ApiKey    = config["apiKey"];
            _polygonDependencies.HttpClientFactory  = httpClientFactory;
            _polygonDependencies.Mapper             = mapper;

            _client = new PolygonClient(_polygonDependencies);
        }

        [HttpGet("GetDailyStockQuote")]
        public async Task<DailyOpenCloseResponse> Get(string ticker)
        {
            // These requests are delayed by a day. No need to waste time here (or *valuble* api requests)
            var date = DateTime.Today.AddDays(-1)
                                     .ToString("yyyy-MM-dd");

            if (_cache.ContainsKey(ticker) && _cache[ticker].DailyOpenClose != null)
            {
                return _cache[ticker].DailyOpenClose;
            }
            else
            {

                var dailyOpenClosesResponses = await _client.GetDailyOpenCloseAsync(ticker,
                                                                                   date);

                // Cache
                if (!_cache.ContainsKey(ticker)) _cache.Add(ticker, new StockDetails());
                _cache[ticker].DailyOpenClose = dailyOpenClosesResponses;

                return dailyOpenClosesResponses;
            }
        }

        [HttpGet("GetTicker")]
        public async Task<TickerDetailsResponse> GetTicker(string ticker)
        {
            var tickerResponse = await _client.GetTickerDetailsAsync(ticker);

            return tickerResponse;

        }

        [HttpGet("GetAggregates")]
        public async Task<AggregatesBarsResponse> GetAggregate(string ticker, string end = null, string start = null)
        {
            // Defaults
            if (string.IsNullOrWhiteSpace(end)) { end = DateTime.Today.ToString("yyyy-MM-dd"); }
            if (string.IsNullOrWhiteSpace(start)) { start = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd"); }

            // Get
            var aggregates = await _client.GetAggregatesBarsAsync(ticker, 1, "day", start, end);

            // Return
            return aggregates;
        }

        [HttpGet("GetDividends")]
        public async Task<StockDividendsResponse> GetDividends(string ticker)
        {
            // Get
            var dividends = await _client.GetStockDividendsAsync(ticker);

            // Return
            return dividends;
        }

        [HttpGet("GetStockDetails")]
        public async Task<StockDetails> GetStockDetails(string ticker)
        {
            // Check the cache
            if (_cache.TryGetValue(ticker, out var result)) 
            { 
                return result; 
            }

            // Populate via the API
            var stockDetails = new StockDetails { Ticker = ticker };
            stockDetails.TickerDetails  = await _client.GetTickerDetailsAsync(ticker);
            stockDetails.AggregateBars  = await _client.GetAggregatesBarsAsync(ticker,
                                                                              multiplier: 1,
                                                                              timespan: "day",
                                                                              from: DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd"),
                                                                              to: DateTime.Today.ToString("yyyy-MM-dd"));
            stockDetails.Dividends      = await _client.GetStockDividendsAsync(ticker);

            // Add to cache
            _cache.Add(ticker, stockDetails);

            return _cache[ticker];
        }

    }
}