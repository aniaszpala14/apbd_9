using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Task9.Context;
using Task9.DTOs;
using Task9.Models;

namespace Task9.Controllers;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("aniapi/[controller]")]
public class TripsController : Controller
{
    private readonly ApbdContext _dbContext;

    public TripsController(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(int page = 1, int pageSize = 10)
    {
        var totalTrips = await _dbContext.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips/(double)pageSize);

        var trips = await _dbContext.Trips
            .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
            .OrderByDescending(o => o.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                Countries = t.IdCountries.Select(c => new { c.Name }),
                Clients = t.ClientTrips.Select(ct => new { FirstName = ct.IdClientNavigation.FirstName, LastName = ct.IdClientNavigation.LastName }).ToList()
            })
            .ToListAsync();
        
            var response = new
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = totalPages,
                Trips = trips
            };

            return Ok(response);
            } 

    
    [HttpPost]
    public async Task<IActionResult> RegisterClientToTrip(ClientTripDTO clienttripDTO)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == clienttripDTO.Pesel);
        if(client != null)
        { return BadRequest("Klient o tym PESELU juz istnieje"); }
        
        var isClinClientTrip = await _dbContext.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == clienttripDTO.IdTrip);
        if(isClinClientTrip != null)
        { return BadRequest("Klient jest zapisany na wycieczke"); }
        
        var trip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.IdTrip == clienttripDTO.IdTrip);
        if(trip==null || trip.DateFrom <= DateTime.Now)
        { return BadRequest("Wycieczka nie istnieje lub zostala juz odbyta"); }
        
        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = clienttripDTO.IdTrip,
            PaymentDate = clienttripDTO.PaymentDate,
            RegisteredAt = DateTime.Now
        };
        
        _dbContext.ClientTrips.Add(clientTrip);
        await _dbContext.SaveChangesAsync();

        return Ok(clientTrip);
    }
    
    
    
    
    
    
    
    
    
    
    
    
}
