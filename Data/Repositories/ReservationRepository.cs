using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<Reservation>> AllAsyncIncludeAll()
        {
            return await RepositoryDbSet.Include(x => x.Companies)
                .Include(x => x.TravelRoute)
                .ToListAsync();

        }
    }
}
