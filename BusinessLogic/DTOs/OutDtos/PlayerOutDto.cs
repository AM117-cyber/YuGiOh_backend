public class PlayerOutDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public double Money { get; set; }
    public int MunicipalityId { get; set; }
    public string MunicipalityName { get; set; }
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
    public IEnumerable<DeckOutDto> Decks { get; set; }
}