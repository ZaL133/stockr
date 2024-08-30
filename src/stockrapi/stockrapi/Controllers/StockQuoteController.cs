using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Polygon.Net;
using stockrapi.Models;
using stockrapi.Plumbing;

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
            var deps = new StockerPolygonDependencies();

            deps.Settings.ApiKey        = _config["apiKey"];
            deps.HttpClientFactory      = _httpClientFactory;
            deps.Mapper                 = _mapper;

            var client = new PolygonClient(deps);

            var dailyOpenClosesResponses = await client.GetDailyOpenCloseAsync(ticker, 
                                                                               DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));

            return dailyOpenClosesResponses;
        }
    }
}