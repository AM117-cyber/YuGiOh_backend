public interface IMunicipalityService
{
    Task<Municipality> CreateMunicipality(Municipality municipality);
    Task<Municipality> GetByName(string name);
}