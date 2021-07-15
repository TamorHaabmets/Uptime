using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.RouteDtos;
using Uptime.Services.Helpers;

namespace Uptime.Services
{
    public class RouteInfoService
    {
        public RouteInfoService(IRouteInfoRepository routeInfoRepository)
        {
        }

        public List<string> GetPossibleDestinations(PriceList priceList)
        {
            var routeInfos = priceList.Legs.Select(x => x.RouteInfo).ToList();
            var routes = routeInfos.Select(x => x.From.Name).Concat(routeInfos.Select(x => x.To.Name)).Distinct().ToList();
            return routes;
        }

        public void FilterRoutes(string[] notAcceptedCompanies, List<RoutesDto> routes)
        {
            foreach (var route in routes)
            {
                
                foreach (var company in notAcceptedCompanies)
                {
                    route.Providers.RemoveAll(x => x.Companies.Contains(company));
                }
            }
        }
        public List<List<RouteInfo>> FindAllRoutes(string[] startAndEnd, List<string> destinations, PriceList priceList)
        {
            var graph = new Graph(destinations);
            var routeInfos = priceList.Legs.Select(x => x.RouteInfo);
            addEdges(routeInfos, graph);
            var allPaths = graph.GetAllPaths(startAndEnd[0], startAndEnd[1]);
            return allPaths;
        }

        public List<RoutesDto> PopulateProviders(string[] startAndEnd, List<List<RouteInfo>> allPaths,
            DateTime priceListValidUntil)
        {
            var start = startAndEnd[0];
            var end = startAndEnd[1];
            var result = new List<RoutesDto>();

            foreach (var routeInfoList in allPaths)
            {
                var firstRoute = routeInfoList.First(x => x.From.Name == start);
                var routes = new List<string> { firstRoute.From.Name, firstRoute.To.Name };
                var uniqueCompanies = new List<string>();
                var providersDtos = populateProvidersForFirstRoute(firstRoute, out uniqueCompanies);


                foreach (var routeInfo in routeInfoList)
                {
                    var isFirstRoute = routeInfo.From.Name == start;
                    if (isFirstRoute) continue;

                    var tempProvidersDto = new List<ProvidersDto>();
                    foreach (var providersDto in providersDtos)
                    {
                        foreach (var provider in routeInfo.Leg.Providers.Where(provider => providersDto.LastFlightEnd < provider.FlightStart))
                        {
                            var newProvidersDto = createProvidersDto(providersDto, provider, routeInfo);
                            tempProvidersDto.Add(newProvidersDto);
                            providersDto.Children.Add(newProvidersDto);
                            var isAlreadyAdded = uniqueCompanies.Contains(provider.Company.Name);
                            if (!isAlreadyAdded)
                            {
                                uniqueCompanies.Add(provider.Company.Name);
                            }
                        }

                        if (providersDto.Children.Count == 0 && routeInfo.To.Name != end)
                        {
                            removeImpossableRoutes(tempProvidersDto, providersDto);
                        }
                    }

                    providersDtos = tempProvidersDto;
                    routes.Add(routeInfo.To.Name);
                }

                if (providersDtos.Count == 0) continue;
                result.Add(new RoutesDto
                {
                    ValidUntil = priceListValidUntil,
                    Companies = uniqueCompanies,
                    Providers = mapRouteInfoDtos(providersDtos),
                    TravelRoutes = routes
                });
            }
            
            return result;
        }

        private static ProvidersDto createProvidersDto(ProvidersDto providersDto, Provider provider, RouteInfo routeInfo)
        {
            return new()
            {
                Parent = providersDto,
                TotalPrice = providersDto.TotalPrice + provider.Price,
                TotalDistance = providersDto.TotalDistance + routeInfo.Distance,
                LastFlightEnd = provider.FlightEnd,
                TotalTime = providersDto.TotalTime + (provider.FlightEnd - providersDto.LastFlightEnd),
                Provider = provider,
                Children = new List<ProvidersDto>(),
                Companies = providersDto.Companies.Append(provider.Company.Name).ToList()
            };
        }

        private static List<RouteInfoDto> mapRouteInfoDtos(List<ProvidersDto> providersDtos)
        {
            return providersDtos.Select(providersDto => new RouteInfoDto
            {
                Id = Guid.NewGuid().ToString(),
                TotalTime = providersDto.TotalTime,
                TotalDistance = providersDto.TotalDistance,
                TotalPrice = providersDto.TotalPrice,
                TravelStart = providersDto.LastFlightEnd - providersDto.TotalTime,
                TravelEnd = providersDto.LastFlightEnd,
                Companies = providersDto.Companies
            }).ToList();
        }

        private static List<ProvidersDto> populateProvidersForFirstRoute(RouteInfo firstRoute, out List<string> uniqueCompanies)
        {
            uniqueCompanies = new List<string>();
            var providersDtos = new List<ProvidersDto>();
            foreach (var provider in firstRoute.Leg.Providers)
            {
                if (provider.FlightStart > DateTime.UtcNow)
                    providersDtos.Add(new ProvidersDto
                    {
                        Parent = null,
                        Provider = provider,
                        TotalPrice = provider.Price,
                        TotalDistance = firstRoute.Distance,
                        LastFlightEnd = provider.FlightEnd,
                        TotalTime = provider.FlightEnd - provider.FlightStart,
                        Children = new List<ProvidersDto>(),
                        Companies = new List<string> {provider.Company.Name}
                    });
                var isAlreadyAdded = uniqueCompanies.Contains(provider.Company.Name);
                if (!isAlreadyAdded)
                {
                    uniqueCompanies.Add(provider.Company.Name);
                }
            }

            return providersDtos;
        }

        private static void removeImpossableRoutes(List<ProvidersDto> tempProvidersDto, ProvidersDto providersDto)
        {
            if (providersDto.Parent != null)
            {
                removeImpossableRoutes(tempProvidersDto, providersDto.Parent);
            }
            tempProvidersDto.Remove(providersDto);
        }

        private static void addEdges(IEnumerable<RouteInfo> routeInfos, Graph graph)
        {
            foreach (var routeInfo in routeInfos)
            {
                graph.AddEdge(routeInfo);
            }
        }
    }
}
