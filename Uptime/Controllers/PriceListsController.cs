using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain;
using Uptime.Services;
using Microsoft.Extensions.Logging;

namespace Uptime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceListsController : ControllerBase
    {
        private readonly PriceListService _priceListService;
        private readonly RouteInfoService _routeInfoService;

        public PriceListsController(PriceListService priceListService, RouteInfoService routeInfoService)
        {
            _priceListService = priceListService;
            _routeInfoService = routeInfoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetDestinations()
        {
            var priceList = _priceListService.GetLastPriceList();
            if (_priceListService.CheckIsPriceListExpired(priceList))
            {
                priceList = await _priceListService.UpdatePriceLists();
            }
            return _routeInfoService.GetPossibleDestinations(priceList);
        }
    }
}
