namespace Backend.Services;

using Models;

public interface INbpService
{
    Task<List<CurrencyRate>> DownloadRatesAsync(DateOnly date);
}
