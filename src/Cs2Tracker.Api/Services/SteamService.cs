using System.Globalization;
using System.Net.Http.Json;

namespace Cs2Tracker.Services;

public class SteamService
{
    private readonly HttpClient _http;

    public SteamService(HttpClient http)
    {
        _http = http;
    }

    public async Task<decimal?> GetPriceAsync(string marketHashName)
    {
        var url =
            $"https://steamcommunity.com/market/priceoverview/?appid=730&currency=3&market_hash_name={marketHashName}";

        var result = await _http.GetFromJsonAsync<SteamResponse>(url);

        if (result == null || !result.success)
            return null;

        return ParsePrice(result.lowest_price);
    }

    private decimal ParsePrice(string price)
    {
        var cleaned = price.Replace("€", "").Replace("$", "").Trim();

        // European format uses comma as decimal separator
        // Replace comma with period for decimal parsing
        cleaned = cleaned.Replace(",", ".");

        if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        return 0;
    }
}

public class SteamResponse
{
    public bool success { get; set; }
    public string lowest_price { get; set; } = "";
}