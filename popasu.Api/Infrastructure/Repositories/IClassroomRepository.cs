using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IClassroomRepository
{
    Task<Classroom?> GetByNumberAsync(string number);
    Task<IEnumerable<Classroom>> GetAllAsync();
    Task<Classroom> AddAsync(Classroom entity);
    void Update(Classroom entity);
    void Delete(Classroom entity);
    Task<int> SaveChangesAsync();
}

