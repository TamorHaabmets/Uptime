using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPriceListRepository : IBaseRepository<PriceList>
    {
        public PriceList LastPriceList();
        public PriceList LastPriceListIncludeAll();
    }
}
