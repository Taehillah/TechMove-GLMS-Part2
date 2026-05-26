using System.Text.Json;

namespace TechMoveGLMS.Web.Services;

public class CurrencyService(HttpClient httpClient, IConfiguration configuration, ILogger<CurrencyService> logger) : ICurrencyService
{
    public async Task<decimal> GetUsdToZarRateAsync()
    {
        var endpoint = configuration["CurrencyApi:UsdToZarEndpoint"];

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new InvalidOperationException("Currency API endpoint is not configured.");
        }

        try
        {
            using var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            if (document.RootElement.TryGetProperty("rates", out var rates)
                && rates.TryGetProperty("ZAR", out var zarRate)
                && zarRate.TryGetDecimal(out var rate))
            {
                return rate;
            }

            throw new InvalidOperationException("The currency API response did not contain a ZAR rate.");
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or TaskCanceledException)
        {
            logger.LogError(ex, "Failed to retrieve USD to ZAR exchange rate.");
            throw new InvalidOperationException("Could not retrieve the current exchange rate. Please try again later.", ex);
        }
    }

    public decimal ConvertUsdToZar(decimal usd, decimal rate)
    {
        return Math.Round(usd * rate, 2, MidpointRounding.AwayFromZero);
    }
}
