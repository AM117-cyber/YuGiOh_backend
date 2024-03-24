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
        .FirstOrDefaultAsync(m => m.Name == name);
}
public async Task<Deck> findById(int Id)
{
    return await _context.Decks
        .FirstOrDefaultAsync(m => m.Id == Id);
}

}