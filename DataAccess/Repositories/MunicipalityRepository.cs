using Microsoft.EntityFrameworkCore;

public class MunicipalityRepository: IMunicipalityRepository
{
    private readonly IApplicationDbContext _context;

    public MunicipalityRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Municipality> Create(Municipality municipality)
    {
        _context.Municipalities.Add(municipality);
        await _context.SaveChangesAsync();
        return municipality;
    }

public async Task<Municipality> findByName(string name)
{
    return await _context.Municipalities
        .Include(m => m.Province) // Eager loading
        .FirstOrDefaultAsync(m => m.Name == name);
}

}

