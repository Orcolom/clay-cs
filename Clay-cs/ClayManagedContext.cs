namespace Clay_cs;

/// <summary>
/// stores managed c# data so it doesn't get collected by the GC before we want it.
/// </summary>
internal class ClayManagedContext
{
	public nint ArenaMemory = default;
	public ClayMeasureTextDelegate MeasureText = null!;
	public ClayErrorDelegate ErrorHandler = null!;
	public ClayQueryScrollOffsetDelegate QueryScrollOffset = null!;
	public readonly List<ClayOnHoverDelegate> OnHover = [];
}
