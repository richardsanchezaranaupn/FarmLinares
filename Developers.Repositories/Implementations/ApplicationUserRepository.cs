using Developers.Models;
using Developers.Persistence;
using Developers.Repositories.Interfaces;

namespace Developers.Repositories.Implementations;

public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
{
    private readonly DevelopersDbContext _db;
    public ApplicationUserRepository(DevelopersDbContext db) : base(db)
    {
        _db = db;
    }
}
