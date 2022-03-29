namespace CarbonAware.Plugins.WattTime.Model;

[Serializable]
public record LoginResult
{
    public string token { get; set; }
}