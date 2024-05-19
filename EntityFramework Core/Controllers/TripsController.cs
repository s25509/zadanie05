using EntityFramework_Core.Context;
using EntityFramework_Core.DTOs;
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
}