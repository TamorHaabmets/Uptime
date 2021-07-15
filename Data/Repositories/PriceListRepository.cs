using Data.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PriceListRepository : BaseRepository<PriceList>, IPriceListRepository
    {
        public PriceListRepository(ApplicationDbContext dataContext) : base(dataContext)
        {
        }
        public PriceList LastPriceList()
        {
            return RepositoryDbSet.Include(x => x.Legs)
                .ThenInclude(x => x.RouteInfo.To)
                .Include(x => x.Legs)
                .ThenInclude(x => x.RouteInfo.From)
                .OrderByDescending(x => x.ValidUntil)
                .FirstOrDefault();

        }

        public PriceList LastPriceListIncludeAll()
        {
            return RepositoryDbSet.Include(x => x.Legs)
                .ThenInclude(x => x.RouteInfo)
                .Include(x => x.Legs)
                .ThenInclude(x => x.RouteInfo.To)
                .Include(x => x.Legs)
                .ThenInclude(x => x.RouteInfo.From)
                .Include(x => x.Legs)
                .ThenInclude(x => x.Providers)
                .Include(x => x.Legs)
                .ThenInclude(x => x.Providers)
                .ThenInclude(x => x.Company)
                .OrderByDescending(x => x.ValidUntil)
                .FirstOrDefault();

        }
    }
}
