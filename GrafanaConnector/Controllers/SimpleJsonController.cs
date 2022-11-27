using GrafanaConnector.DataHandling;
using GrafanaConnector.Models;
using GrafanaConnector.Services;
using GrafanaConnector.ViewModels;
using Meshmakers.Common.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GrafanaConnector.Controllers;

/// <summary>
/// Implements Grafana Simple JSON interface, see https://grafana.com/grafana/plugins/grafana-simple-json-datasource/
/// </summary>
[ApiController]
[Route("")]
public class SimpleJsonController : ControllerBase
{
    private readonly ILogger<SimpleJsonController> _logger;
    private readonly IRecodingStrategyService _recodingStrategyService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="recodingStrategyService">Service that provides access to historical data</param>
    public SimpleJsonController(ILogger<SimpleJsonController> logger, IRecodingStrategyService recodingStrategyService)
    {
        _logger = logger;
        _recodingStrategyService = recodingStrategyService;
    }

    /// <summary>
    /// Return 200 ok. Used for "Test connection" on the datasource config page.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Get() => Ok();

    /// <summary>
    /// Used by the find metric options on the query tab in panels.
    /// </summary>
    /// <returns></returns>
    [HttpPost("search")]
    public IActionResult Search()
    {
        return Ok(_recodingStrategyService.GetReferences().Select(x=> 
            new ReferenceViewModel{ Text = $"{x.AasId}: {x.SubModelId}->{x.SubModelElementPath}", Value = x.Key}));
    }

    /// <summary>
    /// Returns metrics based on input.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("query")]
    public IActionResult Query([FromBody] QueryViewModel query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var smoothingWindow = TimeSpan.FromTicks(5 * TimeSpan.TicksPerMinute);
        var halfSmoothingWindow = 0.5 * smoothingWindow;

        var dataFrom = query.Range.From - halfSmoothingWindow;
        var dataTo = query.Range.To + halfSmoothingWindow;

        var samplingInterval = new TimeSpan(query.IntervalMs * TimeSpan.TicksPerMillisecond);

        // Ensure that there is a valid target
        if (query.Targets.Any(t => string.IsNullOrWhiteSpace(t.Target)))
        {
            return Ok(new TimeSeriesViewModel[]{});
        }

        return Ok
        (
            query.Targets.Select
            (
                target =>
                {
                    var reference = new Reference(target.Target);
                    var repository = _recodingStrategyService.GetRepository(reference);
                    return (Reference: target.Target, Repository: repository);
                }
            ).Select
            (
                dsg => new TimeSeriesViewModel
                (
                    dsg.Reference,
                    dsg.Repository.GetDataPoints(dataFrom, dataTo)
                        .FilterDataPoints(query.Range.From, query.Range.To, samplingInterval, smoothingWindow).Select(x=> new object[]{x.Value, x.DateTime.ToUnixEpochInMilliSecondsTime()}).ToArray()
                )).ToArray()
        );
    }

    /// <summary>
    /// Return annotations. In your sample unused.
    /// </summary>
    /// <returns></returns>
    [HttpPost("annotations")]
    public IActionResult GetAnnotations()
    {
        return Ok();
    }
}