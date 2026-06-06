namespace Backend.Services;

using Models;
using Nbp;


public class NbpService : INbpService 
{
    private readonly HttpClient _httpClient;

    public NbpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CurrencyRate>> DownloadRatesAsync(DateOnly date)
    {
        try
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<NbpTableResponse>>(
                $"https://api.nbp.pl/api/exchangerates/tables/B/{date:yyyy-MM-dd}/?format=json");

            if (response is null || response.Count == 0)
                return new List<CurrencyRate>();

            var table = response.First();

            return table.Rates
                .Select(rate => new CurrencyRate
                    {
                        Id = Guid.NewGuid(),
                        CurrencyCode = rate.Code,
                        CurrencyName = rate.Currency,
                        Rate = rate.Mid,
                        EffectiveDate = table.EffectiveDate
                    })
                .ToList();
        }
        catch (HttpRequestException)
        {
            return [];
        }
    }
}