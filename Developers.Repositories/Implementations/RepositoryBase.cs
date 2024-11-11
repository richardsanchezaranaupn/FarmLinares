using Developers.Persistence;
using Developers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Developers.Repositories.Implementations;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DevelopersDbContext _db;
    internal DbSet<T> dbSet;
    public RepositoryBase(DevelopersDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }
    public async Task AgregarAsync(T entity)
    {
        await _db.AddAsync(entity); // Insert Into Table
    }
    public async Task<T> ObtenerAsync(int id)
    {
        return await dbSet.FindAsync(id); // Select * from table (solo por Id)
    }
    public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        if (filter is not null)
            query = query.Where(filter); // select * from where 

        if (includeProperties is not null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
        }

        if (orderBy is not null)
            query = orderBy(query);

        if (!isTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<T> ObtenerPrimeroAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = dbSet;
        if (filter is not null)
            query = query.Where(filter); // select * from where 

        if (includeProperties is not null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
        }

        if (!isTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }

    public void Remover(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoverRango(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}
