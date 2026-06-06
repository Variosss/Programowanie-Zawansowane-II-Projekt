namespace Backend.Models;

public class FetchCurrenciesResponse {
    public bool Success { get; set; }

    public string? Message { get; set; }

    public int FetchedCount { get; set; }
}
