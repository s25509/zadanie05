using EntityFramework_Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController(_2019sbdContext context) : ControllerBase
{
    [HttpDelete("{idClient}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        var client = await context.Clients.FindAsync(idClient);
        if (client is null) return NotFound("Client not found");

        var trips = await context.Trips
            .Where(t => t.ClientTrips.Any(ct => ct.IdClient == idClient))
            .ToListAsync();
        if (trips.Count != 0) return Conflict("Cannot delete a Client that is assigned to a trip");

        await context.Clients
            .Where(c => c.IdClient == idClient)
            .ExecuteDeleteAsync();
        return Ok("Client deleted successfully");
    }
}