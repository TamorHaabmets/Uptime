using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dataContext) : base(dataContext)
        {
        }

        public Company FindByName(string name)
        {
            return RepositoryDbSet.First(x => x.Name == name);
        }
    }
}
