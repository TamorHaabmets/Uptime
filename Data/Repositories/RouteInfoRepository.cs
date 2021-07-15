using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RouteInfoRepository : BaseRepository<RouteInfo>, IRouteInfoRepository
    {
        public RouteInfoRepository(ApplicationDbContext dataContext) : base(dataContext)
        {
        }
    }
}
