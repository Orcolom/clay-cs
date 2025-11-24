using ZeroElectric.Vinculum;

namespace Clay_cs.Example.Examples;

public class QuickStart
{
	public unsafe void Run()
	{
		// initialize raylib
		Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_WINDOW_HIGHDPI
			| ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
		Raylib.InitWindow(1024, 768, "C# Quickstart Clay");

		// initialize clay
		using var arena = Clay.CreateArena(Clay.MinMemorySize());
		Clay.Initialize(arena, new Clay_Dimensions(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()), ErrorHandler);
		Clay.SetDebugModeEnabled(true);

		// setup text measuring and fonts
		Clay.SetMeasureTextFunction(RaylibClay.MeasureText);
		RaylibClay.Fonts[0] = Raylib.LoadFont("resources/Roboto-Regular.ttf");
		Raylib.SetTextureFilter(RaylibClay.Fonts[0].texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

		while (Raylib.WindowShouldClose() == false)
		{
			// update these every frame
			Clay.SetLayoutDimensions(new Clay_Dimensions(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
			Clay.SetPointerState(Raylib.GetMousePosition(), Raylib.IsMouseButtonDown(0));
			Clay.UpdateScrollContainers(true, Raylib.GetMouseWheelMoveV(), Raylib.GetFrameTime());

			// build your layout
			Clay.BeginLayout();

			using (Clay.Element(Clay.Id("QuickStart"), new()
			{
				backgroundColor = new Clay_Color(25, 0, 25),
				layout = new()
				{
					sizing = new Clay_Sizing(Clay_SizingAxis.Fixed(100), Clay_SizingAxis.Grow())
				}
			}))
			{
				Clay.TextElement("Some text here", new()
				{
					fontSize = 16,
					textColor = new Clay_Color(255, 255, 255)
				});
			}

			var commands = Clay.EndLayout();

			// draw the layout
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Raylib.BEIGE);
			RaylibClay.RenderCommands(commands);
			Raylib.EndDrawing();
		}
	}

	private static void ErrorHandler(Clay_ErrorData data)
	{
		Console.WriteLine($"{data.errorType}: {data.errorText.ToCSharpString()}");
	}
}
