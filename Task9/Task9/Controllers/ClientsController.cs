using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task9.Context;
using Task9.Models;

namespace Task9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : Controller
{
    private readonly ApbdContext _dbContext;

    public ClientsController(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var client = await _dbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);
        
        if(client == null)
        {return NotFound();}
        if(client.ClientTrips.Count > 0) //if(client.ClientTrips.Any()) alternatywa
        { return BadRequest("Klient jest wpisany na wycieczke nie można go usunąć"); }
        
        
        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync(); //wazne koniec
        
        return Ok();
    }
    
    
    //druga opcja z wykladu8
    [HttpDelete("drugi" + "{idClient}")]
    public async Task<IActionResult> DeleteClient2(int idClient)
    {
        var clientToRemove = new Client
        {
            IdClient = idClient

        };
        _dbContext.Clients.Attach(clientToRemove);
     
        
        if(clientToRemove.ClientTrips.Count > 0) //if(client.ClientTrips.Any()) alternatywa
        { return BadRequest("Klient jest wpisany na wycieczke nie można go usunąć"); }
        
        _dbContext.Clients.Remove(clientToRemove);
        await _dbContext.SaveChangesAsync(); //wazne koniec
        
        return Ok();
    }
    
}
