namespace Clay_cs;

public struct ClayElement : IDisposable
{
	public static ClayElement Open()
	{
		ClayInterop.Clay__OpenElement();
		return new ClayElement();
	}
	
	public static ClayElement Open(Clay_ElementId id)
	{
		ClayInterop.Clay__OpenElementWithId(id);
		return new ClayElement();
	}
	
	public static ClayElement OpenAndConfigure(Clay_ElementDeclaration declaration)
	{
		return Open().Configure(declaration);
	}

	public ClayElement Configure(Clay_ElementDeclaration declaration)
	{
		Clay.ConfigureOpenElement(declaration);
		return this;
	}

	public void Dispose()
	{
		Clay.CloseElement();
	}
}
