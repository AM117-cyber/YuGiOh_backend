public interface IDeckRepository
{
    Task<Deck> Create(Deck deck);
    Task<Deck> findByName(string name);
    Task<Deck> findById(int Id);

}
