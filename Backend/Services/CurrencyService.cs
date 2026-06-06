using Backend.Data;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

public class CurrencyService
{
    private readonly CurrencyDbContext _db;
    private readonly INbpService _nbp;
    public CurrencyService(CurrencyDbContext db, INbpService nbp)
    {
        _db = db;
        _nbp = nbp;
    }

    public async Task<List<CurrencyRate>> GetCurrenciesAsync(DateOnly date)
    {
        var existing = await _db.CurrencyRates
            .AsNoTracking()
            .Where(c => c.EffectiveDate == date)
            .ToListAsync();

        if (existing.Count > 0)
            return existing;

        const int maxDaysBack = 30;

        for (var i = 0; i < maxDaysBack; i++)
        {
            var tryDate = date.AddDays(-i);

            var downloaded = await _nbp.DownloadRatesAsync(tryDate);

            if (downloaded.Count == 0)
                continue;

            var effectiveDate = downloaded.First().EffectiveDate;

            var alreadyExists = await _db.CurrencyRates
                .AnyAsync(c => c.EffectiveDate == effectiveDate);

            if (!alreadyExists)
            {
                _db.CurrencyRates.AddRange(downloaded);
                await _db.SaveChangesAsync();
            }

            return downloaded;
        }

        return new List<CurrencyRate>();
    }
    public async Task<int> RefreshCurrenciesAsync()
    {
        var today = DateOnlyExtension.Today();

        var exists = await _db.CurrencyRates
            .AnyAsync(c => c.EffectiveDate == today);

        if (exists)
            return 0;

        var downloaded = await _nbp.DownloadRatesAsync(today);

        if (downloaded.Count == 0)
            return 0;

        _db.CurrencyRates.AddRange(downloaded);
        await _db.SaveChangesAsync();

        return downloaded.Count;
    }
}
