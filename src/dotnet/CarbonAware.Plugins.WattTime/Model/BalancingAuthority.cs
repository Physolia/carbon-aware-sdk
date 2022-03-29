namespace CarbonAware.Plugins.WattTime.Model;

[Serializable]
public record BalancingAuthority
{
    public string abbrev { get; set; }
    public int id { get; set; }
    public string name { get; set; }
}
