namespace BackndTests;

using Backend.Models;
using Backend.Services;
using Moq;

public static class NbpMockFactory
{
    public static Mock<INbpService> Create(List<CurrencyRate> result)
    {
        var mock = new Mock<INbpService>();

        mock.Setup(x => x.DownloadRatesAsync(It.IsAny<DateOnly>()))
            .ReturnsAsync(result);

        return mock;
    }
}