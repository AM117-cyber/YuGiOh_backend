public class MunicipalityService: IMunicipalityService
{
    private readonly IMunicipalityRepository _municipalityRepository;

    public MunicipalityService(IMunicipalityRepository municipalityRepository)
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

