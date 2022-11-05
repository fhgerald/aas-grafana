namespace GrafanaConnector.Services;

internal class RecodingManagementService : IHostedService
{
    private readonly TwinClientService _twinClientService;
    private readonly IRecodingStrategyService _recodingStrategyService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="twinClientService">Access to AAS server</param>
    /// <param name="recodingStrategyService">Access to repositories.</param>
    public RecodingManagementService(TwinClientService twinClientService, IRecodingStrategyService recodingStrategyService)
    {
        _twinClientService = twinClientService;
        _recodingStrategyService = recodingStrategyService;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var time = new TimeSpan(0, 0, 10);
        foreach (var key in _twinClientService.GetReferences())
        {
            _recodingStrategyService.AddIntervalBasedStrategy(key, time, r => Convert.ToDouble(_twinClientService.GetValue(r)));
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _recodingStrategyService.Disconnect();
        
        return Task.CompletedTask;
    }
}