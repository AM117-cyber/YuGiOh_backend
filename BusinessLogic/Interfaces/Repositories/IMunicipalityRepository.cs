public interface IMunicipalityRepository
{
    Task<Municipality> Create(Municipality municipality);
    Task<Municipality> findByName(string name);

}
