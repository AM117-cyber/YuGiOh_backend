public class MunicipalityService
{
    private readonly MunicipalityRepository _municipalityRepository;

    public MunicipalityService(MunicipalityRepository municipalityRepository)
    {
        _municipalityRepository = municipalityRepository;
    }

    public async Task<Municipality> CreateMunicipality(Municipality municipality)
    {
        return await _municipalityRepository.Create(municipality);
    }

    public async Task<Municipality> GetByName(string name)
    {
        return await _municipalityRepository.findByName(name);
    }
}
