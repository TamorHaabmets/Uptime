using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uptime.Services
{
    public class PriceListService
    {
        private readonly IPriceListRepository _priceListRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly HttpClientService _httpClientService;

        public PriceListService(IPriceListRepository priceListRepository, 
            HttpClientService httpClientService, ICompanyRepository companyRepository)
        {
            _priceListRepository = priceListRepository;
            _httpClientService = httpClientService;
            _companyRepository = companyRepository;
        }

        public async Task<PriceList> AddPriceList(PriceList priceList)
        {
            await _priceListRepository.AddAsync(priceList);
            await _priceListRepository.SaveChangesAsync();
            return priceList;
        }

        public async Task<PriceList> GetPriceListById(string id)
        {
            return await _priceListRepository.FindAsync(id);
        }

        public async Task<PriceList> UpdatePriceLists()
        {
            var fetchedPriceList = await _httpClientService.GetTravelPrice();
            var isNewPriceList = !_priceListRepository.Exists(fetchedPriceList.Id);

            if (isNewPriceList)
            {
                try
                {
                    fetchedPriceList = overwriteCompanies(fetchedPriceList);
                    await AddPriceList(fetchedPriceList);
                }
                catch (Exception e)
                {
                    // here should be _logger.LogInformation($"Error adding new PriceList: {e}");
                    throw;
                }
            }
            else
            {
                return await GetPriceListById(fetchedPriceList.Id);
            }

            return fetchedPriceList;
        }

        public bool CheckIsPriceListExpired(PriceList priceList)
        {
            var dbIsEmpty = priceList == null;
            return  dbIsEmpty || priceList.ValidUntil < DateTime.UtcNow;
        }

        public PriceList GetLastPriceList()
        {
            return _priceListRepository.LastPriceList();
        }
        public PriceList GetLastPriceListIncludeAll()
        {
            return _priceListRepository.LastPriceListIncludeAll();
        }

        private PriceList overwriteCompanies(PriceList priceList)
        {
            var tempCompanies = new List<Company>();

            priceList.Legs.ForEach(x =>
            {
                x.Providers.ForEach(y =>
                {
                    var isAlreadyAdded = tempCompanies.Exists(z => z.Id == y.Company.Id);
                    var existsInDb = _companyRepository.Exists(y.Company.Id);

                    if (!existsInDb && !isAlreadyAdded)
                    {
                        tempCompanies.Add(y.Company);
                    }
                    else if (isAlreadyAdded)
                    {
                        y.Company = tempCompanies.FirstOrDefault(w => w.Id == y.Company.Id);
                    }
                    else
                    {
                        var dbCompany = _companyRepository.FindAsync(y.Company.Id).Result;
                        y.Company = dbCompany;
                    }
                });
            });
            return priceList;
        }
    }
}
