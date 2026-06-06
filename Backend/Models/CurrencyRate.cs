namespace Backend.Models;

public class CurrencyRate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CurrencyCode { get; set; } = default!;

    public string CurrencyName { get; set; } = default!;

    public decimal Rate { get; set; }

    public DateOnly EffectiveDate { get; set; }}