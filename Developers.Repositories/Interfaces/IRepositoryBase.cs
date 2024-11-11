using System.Linq.Expressions;
namespace Developers.Repositories.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    Task<T> ObtenerAsync(int id); // Recuperar un registro por id
    Task<IEnumerable<T>> ObtenerTodosAsync( //Recuperar todos
        Expression<Func<T, bool>> filter = null, // filtros
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // ordenado
        string includeProperties = null, // incluir las relaciones
        bool isTracking = true // Si tiene activado o no el tracking
        );
    Task<T> ObtenerPrimeroAsync(
        Expression<Func<T, bool>> filter = null, // filtros
        string includeProperties = null, // incluir las relaciones
        bool isTracking = true
        ); // Obtener solo el primero
    Task AgregarAsync(T entity);
    void Remover(T entity);
    void RemoverRango(IEnumerable<T> entity);
}
