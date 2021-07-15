using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Domain;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.ReservationDtos;
using Uptime.Services;

namespace Uptime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDtoGet>>> GetReservations()
        {
            var reservations = await _reservationService.GetReservations();
            var reservationDtoGets = mapReservationToDto(reservations);
            return reservationDtoGets;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(ReservationDtoPost reservationDtoPost)
        {
            try
            {
                await _reservationService.AddReservation(mapDtoToReservation(reservationDtoPost));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }

        private Reservation mapDtoToReservation(ReservationDtoPost reservationDtoPost)
        {
            return new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = reservationDtoPost.FirstName,
                LastName = reservationDtoPost.LastName,
                TotalPrice = reservationDtoPost.TotalPrice,
                TotalTravelTime = reservationDtoPost.TotalTravelTime,
                TravelRoute = reservationDtoPost.TravelRoute.Select(x => new Destination
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = x
                }).ToList(),
                Companies = reservationDtoPost.Companies.Select(x => _reservationService.GetCompanyByName(x)).ToList()
            };
        }

        private static List<ReservationDtoGet> mapReservationToDto(List<Reservation> reservations)
        {
            return reservations.Select(x => new ReservationDtoGet
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Companies = x.Companies.Select(y => y.Name).ToList(),
                TotalPrice = x.TotalPrice,
                TotalTravelTime = new TimeSpan(x.TotalTravelTime),
                TravelRoute = x.TravelRoute.Select(y => y.Name).ToList()
            }).ToList();
        }
    }
}
