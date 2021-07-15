using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Uptime.Services;
using Domain.DataTransferObjects.RouteDtos;
using Uptime.Services.Helpers;

namespace Uptime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteInfosController : ControllerBase
    {
        private readonly RouteInfoService _routeInfoService;
        private readonly PriceListService _priceListService;


        public RouteInfosController(RouteInfoService routeInfoService,
            PriceListService priceListService)
        {
            _routeInfoService = routeInfoService;
            _priceListService = priceListService;
        }

        [HttpPost("destinations")]
        public List<RoutesDto> GetRouteInfos([FromBody]string[] startAndEnd)
        {
            var priceList = _priceListService.GetLastPriceListIncludeAll();
            var destinations = _routeInfoService.GetPossibleDestinations(priceList);
            var allPaths = _routeInfoService.FindAllRoutes(startAndEnd, destinations, priceList);

            return _routeInfoService.PopulateProviders(startAndEnd, allPaths, priceList.ValidUntil);
        }

        [HttpPost("filter")]
        public List<RoutesDto> FilterRoutes(FilterRoutesDto dto)
        {
            var routes = GetRouteInfos(dto.StartAndEnd);
            _routeInfoService.FilterRoutes(dto.Companies, routes);

            return routes;
        }
    }
}
