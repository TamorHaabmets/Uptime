using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uptime.Services
{
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICompanyRepository _companyRepository;

        public ReservationService(IReservationRepository reservationRepository, ICompanyRepository companyRepository)
        {
            _reservationRepository = reservationRepository;
            _companyRepository = companyRepository;
        }

        public async Task<List<Reservation>> GetReservations()
        {
            return await _reservationRepository.AllAsyncIncludeAll();
        }

        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
            return reservation;
        }

        public Company GetCompanyByName(string name)
        {
            return _companyRepository.FindByName(name);
        }
    }
}
