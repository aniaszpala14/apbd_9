using System.Transactions;

using Task9.Repositories;

namespace Task9.Controllers;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("aniapi/[controller]")]
public class TripsController : Controller
{
    private readonly ITripsRepository _tripsRepository;

    public TripsController(ITripsRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }

    
    
    
    
    
    
    
    
    
    
    
    
}
