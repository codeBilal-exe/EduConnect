namespace EduConnect.Interfaces;

// OCP: New entity types can be added without modifying this interface
// DIP: Depend on abstraction, not concrete implementations
public interface IRepository<T>
{
    T? GetById(Guid id);
    List<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
}
