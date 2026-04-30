using Cs2Tracker.Models;

namespace Cs2Tracker.Services;

public class PriceUpdateService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PriceUpdateService> _logger;

    public PriceUpdateService(IServiceProvider serviceProvider, ILogger<PriceUpdateService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price Update Service started");

        // Wait 10 seconds on startup before first update
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateAllPrices();
                _logger.LogInformation("Price update completed at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating prices");
            }

            // Wait 30 minutes before next update
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task UpdateAllPrices()
    {
        using var scope = _serviceProvider.CreateScope();
        var steamService = scope.ServiceProvider.GetRequiredService<SteamService>();
        var caseService = scope.ServiceProvider.GetRequiredService<CaseService>();

        // Complete list of ALL CS:GO/CS2 weapon cases
        var casesToTrack = new[]
        {
            // Original CS:GO Cases
            "CS:GO Weapon Case",
            "CS:GO Weapon Case 2",
            "CS:GO Weapon Case 3",

            // eSports Cases
            "eSports 2013 Case",
            "eSports 2013 Winter Case",
            "eSports 2014 Summer Case",

            // Operation Cases
            "Operation Bravo Case",
            "Operation Phoenix Weapon Case",
            "Operation Breakout Weapon Case",
            "Operation Vanguard Weapon Case",
            "Operation Wildfire Case",
            "Operation Hydra Case",
            "Operation Broken Fang Case",
            "Operation Riptide Case",
            "Shattered Web Case",

            // Chroma Series
            "Chroma Case",
            "Chroma 2 Case",
            "Chroma 3 Case",

            // Gamma Series
            "Gamma Case",
            "Gamma 2 Case",

            // Spectrum Series
            "Spectrum Case",
            "Spectrum 2 Case",

            
            "Glove Case",
            "Huntsman Weapon Case",
            "Falchion Case",
            "Shadow Case",
            "Revolver Case",
            "Clutch Case",
            "Horizon Case",
            "Danger Zone Case",
            "Prisma Case",
            "Prisma 2 Case",
            "Fracture Case",
            "Snakebite Case",
            "Dreams & Nightmares Case",
            "Recoil Case",
            "Revolution Case",
            "Kilowatt Case",
            "CS20 Case",

            // Armory cases
            "Gallery Case",
            "Fever Case"
        };

        foreach (var caseName in casesToTrack)
        {
            try
            {
                var price = await steamService.GetPriceAsync(caseName);

                if (price.HasValue)
                {
                    await caseService.AddOrUpdateAsync(new Case
                    {
                        Name = caseName,
                        Price = price.Value,
                        UpdatedAt = DateTime.UtcNow
                    });

                    _logger.LogInformation("Updated {CaseName}: €{Price}", caseName, price.Value);
                }
                else
                {
                    _logger.LogWarning("Could not fetch price for {CaseName}", caseName);
                }

                // Delay between Steam API calls to avoid rate limiting
                await Task.Delay(TimeSpan.FromSeconds(10), CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching price for {CaseName}", caseName);
            }
        }
    }
}
