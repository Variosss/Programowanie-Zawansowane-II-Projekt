namespace Backend.Nbp;

public class NbpTableResponse
{
    public string Table { get; set; } = default!;

    public string No { get; set; } = default!;

    public DateOnly EffectiveDate { get; set; }

    public List<NbpRate> Rates { get; set; } = [];
}

public class NbpRate
{
    public string Currency { get; set; } = default!;

    public string Code { get; set; } = default!;

    public decimal Mid { get; set; }
}
