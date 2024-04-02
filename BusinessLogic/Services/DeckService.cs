using System.Globalization;

public class DeckService: IDeckService
{
    private readonly IDeckRepository _deckRepository;
    private readonly IUserRepository _userRepository;

    public DeckService(IDeckRepository deckRepository, IUserRepository userRepository)
    {
        _deckRepository = deckRepository;
        _userRepository = userRepository;
    }

    public async Task<Deck> CreateDeck(DeckInDto deck)
    {
        Player player = await _userRepository.findByIdWithDeck(deck.PlayerId);
        var existingDeck = player.Decks
        .Any(d => d.Name == deck.Name && d.MyStatus == EntityStatus.visible);
    if (existingDeck)
    {
        throw new ArgumentException("A deck with this name already exists for this player");
    }
        Deck deckForRepository = new Deck
        {
            Name = deck.Name,
            Archetype = deck.Archetype,
            MainCards = deck.MainCards,
            ExtraCards = deck.ExtraCards,
            SideCards = deck.SideCards,
            PlayerId = deck.PlayerId,
            MyStatus = EntityStatus.visible
        };

        return await _deckRepository.Create(deckForRepository);
    }

    public async Task<bool> DeleteDeck(int deckId)
    {
        var deck = await _deckRepository.findById(deckId);
        if (deck == null)
        {
            throw new ArgumentException("Deck doesn't exist.");
        }
        deck.MyStatus = EntityStatus.hidden;
        return true; 
    }

    public Task<IEnumerable<string>> GetAllArchetypes()
    {
        return _deckRepository.GetAllArchetypes();
    }

    public async Task<Deck> GetByName(string name)
    {
        return await _deckRepository.findByName(name);
    }

    public Task<IEnumerable<DeckArchetypeCountDto>> GetDeckArchetypeCount()
    {
        return _deckRepository.GetDeckArchetypeCount();
    }
}
