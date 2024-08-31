using AutoMapper;
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
        private IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private static Dictionary<string, Dictionary<string, DailyOpenCloseResponse>> _cache = new Dictionary<string, Dictionary<string, DailyOpenCloseResponse>>();

        public StockQuoteController(ILogger<StockQuoteController> logger,
                                    IConfiguration config,
                                    IHttpClientFactory httpClientFactory,
                                    IMapper mapper)
        {
            _logger             = logger;
            _config             = config;
            _httpClientFactory  = httpClientFactory;
            _mapper             = mapper;
        }

        [HttpGet(Name = "GetDailyStockQuote")]
        public async Task<DailyOpenCloseResponse> Get(string ticker)
        {
            // These requests are delayed by a day. No need to waste time here (or *valuble* api requests)
            var date = DateTime.Today.AddDays(-1)
                                     .ToString("yyyy-MM-dd");

            if (_cache.ContainsKey(ticker) && _cache[ticker].ContainsKey(date))
            {
                return _cache[ticker][date];
            }
            else
            {
                var deps = new StockerPolygonDependencies();

                deps.Settings.ApiKey    = _config["apiKey"];
                deps.HttpClientFactory  = _httpClientFactory;
                deps.Mapper             = _mapper;

                var client = new PolygonClient(deps);

                var dailyOpenClosesResponses = await client.GetDailyOpenCloseAsync(ticker,
                                                                                   date);

                CacheResponse(ticker, date, dailyOpenClosesResponses);

                return dailyOpenClosesResponses;
            }
        }

        private void CacheResponse(string tickr, string date, DailyOpenCloseResponse response)
        {
            if (!_cache.ContainsKey(tickr))
            {
                _cache.Add(tickr, new Dictionary<string, DailyOpenCloseResponse>());
            }

            if (!_cache[tickr].ContainsKey(date))
            {
                _cache[tickr].Add(date, response);
            }
            else
            {
                _cache[tickr][date] = response;
            }
        }
    }
}