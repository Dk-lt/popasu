using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly ApplicationDbContext _context;

    public ClassroomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Classroom?> GetByNumberAsync(string number)
    {
        return await _context.Classrooms
            .Include(c => c.Equipment)
            .Include(c => c.Furniture)
            .FirstOrDefaultAsync(c => c.Number == number);
    }

    public async Task<IEnumerable<Classroom>> GetAllAsync()
    {
        return await _context.Classrooms
            .Include(c => c.Equipment)
            .Include(c => c.Furniture)
            .ToListAsync();
    }

    public async Task<Classroom> AddAsync(Classroom entity)
    {
        await _context.Classrooms.AddAsync(entity);
        return entity;
    }

    public void Update(Classroom entity)
    {
        _context.Classrooms.Update(entity);
    }

    public void Delete(Classroom entity)
    {
        _context.Classrooms.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}

