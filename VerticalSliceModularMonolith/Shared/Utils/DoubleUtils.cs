namespace VerticalSliceModularMonolith.Shared.Utils;

public static class DoubleUtils
{
    public static bool Equals(double x, double y, double tolerance = 1e-10)
    {
        var diff = Math.Abs(x - y);
        return diff <= tolerance ||
               diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
    }
}