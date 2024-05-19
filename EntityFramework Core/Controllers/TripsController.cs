using EntityFramework_Core.Context;
using EntityFramework_Core.DTOs;
using EntityFramework_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController(_2019sbdContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTripsAsync()
    {
        var trips = await context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                Countries = t.IdCountries.Select(c => new CountryDTO
                {
                    Name = c.Name
                }),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                })
            })
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync();
        return Ok(trips);
    }

    [HttpPost("{idTrip}/clients")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignClientToTripAsync(int idTrip, [FromBody] AssignClientToTripDTO dto)
    {
        var trip = await context.Trips.FindAsync(idTrip);
        if (trip is null) return NotFound("Trip not found");

        var client = await context.Clients.SingleOrDefaultAsync(c => c.Pesel == dto.Pesel);
        if (client is null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Pesel = dto.Pesel
            };

            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
        }
        else if (dto.FirstName != client.FirstName || 
                 dto.LastName != client.LastName || 
                 dto.Email != client.Email || 
                 dto.Telephone != client.Telephone)
        {
            return Conflict("Provided Client data doesn't match data already stored in system");
        }

        var clientTrip = await context.ClientTrips
            .FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
        if (clientTrip is not null) return Conflict("Client is already assigned to this trip");

        var newClientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            PaymentDate = dto.PaymentDate,
            RegisteredAt = DateTime.Now
        };

        await context.ClientTrips.AddAsync(newClientTrip);
        await context.SaveChangesAsync();

        return Ok("Client successfully added to Trip");
    }
}