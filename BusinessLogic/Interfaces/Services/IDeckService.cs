public interface IDeckService
{
    Task<Deck> CreateDeck(DeckInDto deck);
    Task<Deck> GetByName(string name);
    Task<IEnumerable<DeckArchetypeCountDto>> GetDeckArchetypeCount();
    Task<IEnumerable<string>> GetAllArchetypes();
}