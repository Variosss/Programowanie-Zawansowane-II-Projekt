namespace Backend.Helpers;

public class DateOnlyExtension {
   public static DateOnly Today() => DateOnly.FromDateTime(DateTime.Today);
}
