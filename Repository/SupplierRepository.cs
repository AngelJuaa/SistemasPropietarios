using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class SupplierRepository : IRepository<Supplier>
{
    private readonly StoreContext _context;

    public SupplierRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplier>> Get() =>
        await _context.Suppliers.AsNoTracking().ToListAsync();

    public async Task<Supplier?> GetById(int id) =>
        await _context.Suppliers.FindAsync(id);

    public async Task Add(Supplier entity) =>
        await _context.Suppliers.AddAsync(entity);

    public void Update(Supplier entity) =>
        _context.Suppliers.Update(entity);

    public void Delete(Supplier entity) =>
        _context.Suppliers.Remove(entity);

    public async Task Save() =>
        await _context.SaveChangesAsync();
}
