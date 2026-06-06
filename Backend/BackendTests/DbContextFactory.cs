namespace BackndTests;

using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

public static class DbContextFactory
{
    public static CurrencyDbContext Create()
    {
        var options = new DbContextOptionsBuilder<CurrencyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CurrencyDbContext(options);
    }
}