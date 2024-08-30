using AutoMapper;
using Polygon.Net;

namespace stockrapi.Plumbing
{
    public class StockerPolygonDependencies : IPolygonDependencies
    {
        public PolygonSettings Settings { get; set; } = new PolygonSettings();
        public IHttpClientFactory HttpClientFactory { get; set; }
        public IMapper Mapper { get; set; } 
    }
}
