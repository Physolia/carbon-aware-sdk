using CarbonAware.Config;
using CarbonAware.Model;
using CarbonAware.Plugins.WattTime;
using Microsoft.AspNetCore.Mvc;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace CarbonAware.WebApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("emissions")]
public class CarbonAwareController : ControllerBase
{
    private readonly ILogger<CarbonAwareController> _logger;
    private ICarbonAwarePlugin _plugin;
    private ServiceManager _serviceManager;
    private IConfigManager _configManager;
    private CarbonAwareWattTimeClient _client;

    public CarbonAwareController(ILogger<CarbonAwareController> logger)
    {
        _logger = logger;
        _client = new CarbonAwareWattTimeClient();
        _configManager = new ConfigManager("carbon-aware.json");
        _serviceManager = new ServiceManager(_configManager);
        var pluginService = _serviceManager.ServiceProvider.GetService<ICarbonAwarePlugin>();

        if (pluginService is not null)
        {
            _plugin = pluginService;
        }
        else
        {
            throw new Exception("Services are not configured properly.  Could not find plugin service.");
        }
    }

    [HttpGet("bylocations/best")]
    public IEnumerable<EmissionsData> GetBestEmissionsDataForLocationsByTime([FromQuery(Name = "locations")] string[] locations, DateTime? time = null, DateTime? toTime = null, int durationMinutes = 0)
    {
        var response = _plugin.GetBestEmissionsDataForLocationsByTime(locations.ToList(), time ?? DateTime.Now, toTime, durationMinutes);

        return response;
    }

    [HttpGet("bylocations")]
    public IEnumerable<EmissionsData> GetEmissionsDataForLocationsByTime([FromQuery(Name = "locations")] string[] locations, DateTime? time = null, DateTime? toTime = null, int durationMinutes = 0)
    {
        var response = _plugin.GetEmissionsDataForLocationsByTime(locations.ToList(), time ?? DateTime.Now, toTime, durationMinutes);

        return response;
    }

    [HttpGet("bylocation")]
    public IEnumerable<EmissionsData> GetEmissionsDataForLocationByTime(string location, DateTime? time = null, DateTime? toTime = null, int durationMinutes = 0)
    {
        var response = _plugin.GetEmissionsDataForLocationByTime(location, time ?? DateTime.Now, toTime, durationMinutes);

        return response;
    }

    // TODO(bderusha): REMOVE THIS AFTER TESTING
    [HttpGet("watttime/data")]
    public async Task<IActionResult> WattTimeAsync([FromQuery(Name = "ba")] string ba, [FromQuery(Name = "start")] string? startTime = null, [FromQuery(Name = "end")] string? endTime = null)
    {
        
        var response = await _client.GetDataAsync(ba, startTime, endTime);

        return Ok(response);
    }

    // TODO(bderusha): REMOVE THIS AFTER TESTING
    [HttpGet("watttime/forecast")]
    public async Task<IActionResult> WattTimeForecastAsync([FromQuery(Name = "ba")] string ba, [FromQuery(Name = "start")] string? startTime = null, [FromQuery(Name = "end")] string? endTime = null)
    {
        
        var response = await _client.GetCurrentForecastAsync(ba);

        return Ok(response);
    }
}
