using System.Data;
using System.Data.Common;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Task9.Repositories;

namespace Exercise5NEW.Repositories;

public class TripsRepository : ITripsRepository
{
    public readonly IConfiguration _configuration;

    public TripsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

}