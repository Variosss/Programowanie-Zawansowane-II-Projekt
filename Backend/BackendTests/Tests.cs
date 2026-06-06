using Backend.Models;
using Backend.Services;
using BackndTests;
using Xunit;
using Moq;

public class CurrencyServiceTests
{
    [Fact]
    public async Task GetCurrenciesAsync_ReturnsFromDb_WhenExists()
    {
        var db = DbContextFactory.Create();

        var date = new DateOnly(2026, 6, 1);

        db.CurrencyRates.Add(new CurrencyRate
            {
                Id = Guid.NewGuid(),
                CurrencyCode = "USD",
                CurrencyName = "US Dollar",
                Rate = 4.2m,
                EffectiveDate = date
            });

        await db.SaveChangesAsync();

        var nbpMock = new Mock<INbpService>();

        var service = new CurrencyService(db, nbpMock.Object);

        var result = await service.GetCurrenciesAsync(date);

        Assert.Single(result);
        nbpMock.Verify(x => x.DownloadRatesAsync(It.IsAny<DateOnly>()), Times.Never);
    }
    [Fact]
    public async Task GetCurrenciesAsync_CallsApi_WhenDbEmpty()
    {
        var db = DbContextFactory.Create();

        var date = new DateOnly(2026, 6, 1);

        var apiData = new List<CurrencyRate>
            {
                new CurrencyRate
                    {
                        Id = Guid.NewGuid(),
                        CurrencyCode = "EUR",
                        CurrencyName = "Euro",
                        Rate = 4.5m,
                        EffectiveDate = date
                    }
            };

        var nbpMock = new Mock<INbpService>();
        nbpMock.Setup(x => x.DownloadRatesAsync(date))
            .ReturnsAsync(apiData);

        var service = new CurrencyService(db, nbpMock.Object);

        var result = await service.GetCurrenciesAsync(date);

        Assert.Single(result);
        nbpMock.Verify(x => x.DownloadRatesAsync(date), Times.Once);
    }
    [Fact]
    public async Task GetCurrenciesAsync_SavesToDb_WhenApiReturnsData()
    {
        var db = DbContextFactory.Create();

        var date = new DateOnly(2026, 6, 1);

        var apiData = new List<CurrencyRate>
            {
                new CurrencyRate
                    {
                        Id = Guid.NewGuid(),
                        CurrencyCode = "USD",
                        CurrencyName = "US Dollar",
                        Rate = 4.1m,
                        EffectiveDate = date
                    }
            };

        var nbpMock = new Mock<INbpService>();
        nbpMock.Setup(x => x.DownloadRatesAsync(date))
            .ReturnsAsync(apiData);

        var service = new CurrencyService(db, nbpMock.Object);

        await service.GetCurrenciesAsync(date);

        var saved = db.CurrencyRates.ToList();

        Assert.Single(saved);
    }
    [Fact]
    public async Task GetCurrenciesAsync_ReturnsEmpty_WhenApiFails()
    {
        var db = DbContextFactory.Create();

        var date = new DateOnly(2026, 6, 1);

        var nbpMock = new Mock<INbpService>();
        nbpMock.Setup(x => x.DownloadRatesAsync(It.IsAny<DateOnly>()))
            .ReturnsAsync(new List<CurrencyRate>());

        var service = new CurrencyService(db, nbpMock.Object);

        var result = await service.GetCurrenciesAsync(date);

        Assert.Empty(result);
    }
}
