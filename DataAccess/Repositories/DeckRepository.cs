using Microsoft.EntityFrameworkCore;

public class DeckRepository: IDeckRepository
{
    private readonly IApplicationDbContext _context;

    public DeckRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Deck> Create(Deck deck)
    {
        _context.Decks.Add(deck);
        await _context.SaveChangesAsync();
        return deck;
    }

public async Task<Deck> findByName(string name)
{
    return await _context.Decks
        .Where(d => d.MyStatus == EntityStatus.visible)
        .FirstOrDefaultAsync(m => m.Name == name);
}
public async Task<Deck> findById(int Id)
{
    return await _context.Decks
        .Where(d => d.MyStatus == EntityStatus.visible)
        .FirstOrDefaultAsync(m => m.Id == Id);
}

public async Task<IEnumerable<DeckArchetypeCountDto>> GetDeckArchetypeCount()
{
    var deckCounts = await _context.Decks
        .Where(d => d.MyStatus == EntityStatus.visible)
        .GroupBy(d => d.Archetype)
        .Select(g => new DeckArchetypeCountDto
        {
            Archetype = g.Key,
            Count = g.Count()
        })
        .OrderByDescending(dto => dto.Count)
        .ToListAsync();

    return deckCounts;
}


public async Task<IEnumerable<string>> GetAllArchetypes()
{
    var archetypes = await _context.Decks
        .Select(d => d.Archetype)
        .Distinct()
        .ToListAsync();

    return archetypes;
}



}