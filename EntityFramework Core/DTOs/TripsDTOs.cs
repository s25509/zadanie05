namespace EntityFramework_Core.DTOs;

public class TripDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public IEnumerable<CountryDTO> Countries { get; set; }
    public IEnumerable<ClientDTO> Clients { get; set; }
}

public class CountryDTO
{
    public string Name { get; set; }
}

public class ClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}