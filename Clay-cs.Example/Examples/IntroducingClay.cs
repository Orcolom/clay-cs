using System.Numerics;
using ZeroElectric.Vinculum;

namespace Clay_cs.Example.Examples;

public class IntroducingClay : IDisposable
{
	private struct Document
	{
		public Clay_String Title;
		public Clay_String Contents;
	}

	private Document[] _documents;
	private ClayStringCollection _clayString = new ClayStringCollection();

	private int _selectedDocumentIndex;

	private Clay_Color _contentBackgroundColor = new Clay_Color(90, 90, 90);

	void ErrorHandler(Clay_ErrorData data)
	{
		Console.WriteLine($"{data.errorType}: {data.errorText.ToCSharpString()}");
	}


	public unsafe void Run()
	{
		_documents =
		[
			new Document
			{
				Title = _clayString.Get("Squirrels"),
				Contents = _clayString.Get(
					"""The Secret Life of Squirrels: Nature's Clever Acrobats\n""Squirrels are often overlooked creatures, dismissed as mere park inhabitants or backyard nuisances. Yet, beneath their fluffy tails and twitching noses lies an intricate world of cunning, agility, and survival tactics that are nothing short of fascinating. As one of the most common mammals in North America, squirrels have adapted to a wide range of environments from bustling urban centers to tranquil forests and have developed a variety of unique behaviors that continue to intrigue scientists and nature enthusiasts alike.\n""\n""Master Tree Climbers\n""At the heart of a squirrel's skill set is its impressive ability to navigate trees with ease. Whether they're darting from branch to branch or leaping across wide gaps, squirrels possess an innate talent for acrobatics. Their powerful hind legs, which are longer than their front legs, give them remarkable jumping power. With a tail that acts as a counterbalance, squirrels can leap distances of up to ten times the length of their body, making them some of the best aerial acrobats in the animal kingdom.\n""But it's not just their agility that makes them exceptional climbers. Squirrels' sharp, curved claws allow them to grip tree bark with precision, while the soft pads on their feet provide traction on slippery surfaces. Their ability to run at high speeds and scale vertical trunks with ease is a testament to the evolutionary adaptations that have made them so successful in their arboreal habitats.\n""\n""Food Hoarders Extraordinaire\n""Squirrels are often seen frantically gathering nuts, seeds, and even fungi in preparation for winter. While this behavior may seem like instinctual hoarding, it is actually a survival strategy that has been honed over millions of years. Known as \"scatter hoarding,\" squirrels store their food in a variety of hidden locations, often burying it deep in the soil or stashing it in hollowed-out tree trunks.\n""Interestingly, squirrels have an incredible memory for the locations of their caches. Research has shown that they can remember thousands of hiding spots, often returning to them months later when food is scarce. However, they don't always recover every stash some forgotten caches eventually sprout into new trees, contributing to forest regeneration. This unintentional role as forest gardeners highlights the ecological importance of squirrels in their ecosystems.\n""\n""The Great Squirrel Debate: Urban vs. Wild\n""While squirrels are most commonly associated with rural or wooded areas, their adaptability has allowed them to thrive in urban environments as well. In cities, squirrels have become adept at finding food sources in places like parks, streets, and even garbage cans. However, their urban counterparts face unique challenges, including traffic, predators, and the lack of natural shelters. Despite these obstacles, squirrels in urban areas are often observed using human infrastructure such as buildings, bridges, and power lines as highways for their acrobatic escapades.\n""There is, however, a growing concern regarding the impact of urban life on squirrel populations. Pollution, deforestation, and the loss of natural habitats are making it more difficult for squirrels to find adequate food and shelter. As a result, conservationists are focusing on creating squirrel-friendly spaces within cities, with the goal of ensuring these resourceful creatures continue to thrive in both rural and urban landscapes.\n""\n""A Symbol of Resilience\n""In many cultures, squirrels are symbols of resourcefulness, adaptability, and preparation. Their ability to thrive in a variety of environments while navigating challenges with agility and grace serves as a reminder of the resilience inherent in nature. Whether you encounter them in a quiet forest, a city park, or your own backyard, squirrels are creatures that never fail to amaze with their endless energy and ingenuity.\n""In the end, squirrels may be small, but they are mighty in their ability to survive and thrive in a world that is constantly changing. So next time you spot one hopping across a branch or darting across your lawn, take a moment to appreciate the remarkable acrobat at work a true marvel of the natural world.\n""")
			},
			new Document
			{
				Title = _clayString.Get("Lorem Ipsum"),
				Contents = _clayString.Get(
					"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")
			},
			new Document
			{
				Title = _clayString.Get("Vacuum Instructions"),
				Contents = _clayString.Get(
					"""Chapter 3: Getting Started - Unpacking and Setup\n""\n""Congratulations on your new SuperClean Pro 5000 vacuum cleaner! In this section, we will guide you through the simple steps to get your vacuum up and running. Before you begin, please ensure that you have all the components listed in the ""Package Contents\" section on page 2.\n""\n""1. Unboxing Your Vacuum\n""Carefully remove the vacuum cleaner from the box. Avoid using sharp objects that could damage the product. Once removed, place the unit on a flat, stable surface to proceed with the setup. Inside the box, you should find:\n""\n""    The main vacuum unit\n""    A telescoping extension wand\n""    A set of specialized cleaning tools (crevice tool, upholstery brush, etc.)\n""    A reusable dust bag (if applicable)\n""    A power cord with a 3-prong plug\n""    A set of quick-start instructions\n""\n""2. Assembling Your Vacuum\n""Begin by attaching the extension wand to the main body of the vacuum cleaner. Line up the connectors and twist the wand into place until you hear a click. Next, select the desired cleaning tool and firmly attach it to the wand's end, ensuring it is securely locked in.\n""\n""For models that require a dust bag, slide the bag into the compartment at the back of the vacuum, making sure it is properly aligned with the internal mechanism. If your vacuum uses a bagless system, ensure the dust container is correctly seated and locked in place before use.\n""\n""3. Powering On\n""To start the vacuum, plug the power cord into a grounded electrical outlet. Once plugged in, locate the power switch, usually positioned on the side of the handle or body of the unit, depending on your model. Press the switch to the \"On\" position, and you should hear the motor begin to hum. If the vacuum does not power on, check that the power cord is securely plugged in, and ensure there are no blockages in the power switch.\n""\n""Note: Before first use, ensure that the vacuum filter (if your model has one) is properly installed. If unsure, refer to \"Section 5: Maintenance\" for filter installation instructions.""")
			},
			new Document { Title = _clayString.Get("Article 4"), Contents = _clayString.Get("Article 4") },
			new Document { Title = _clayString.Get("Article 5"), Contents = _clayString.Get("Article 5") },
		];

		Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_WINDOW_HIGHDPI
			| ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
		Raylib.InitWindow(1024, 768, "C# Introducing Clay Demo");

		var requiredSize = Clay.MinMemorySize();
		using var arena = Clay.CreateArena(requiredSize);
		Clay.Initialize(arena, new Clay_Dimensions
		{
			width = Raylib.GetScreenWidth(),
			height = Raylib.GetScreenHeight(),
		}, ErrorHandler);

		Clay.SetMeasureTextFunction(RaylibClay.MeasureText);
		RaylibClay.Fonts[0] = Raylib.LoadFont("resources/Roboto-Regular.ttf");
		Raylib.SetTextureFilter(RaylibClay.Fonts[0].texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

		while (Raylib.WindowShouldClose() == false)
		{
			Clay.SetLayoutDimensions(new Clay_Dimensions
			{
				width = Raylib.GetScreenWidth(),
				height = Raylib.GetScreenHeight(),
			});

			Clay.SetPointerState(Raylib.GetMousePosition(), Raylib.IsMouseButtonDown(0));
			Clay.UpdateScrollContainers(true, Raylib.GetMouseWheelMoveV(), Raylib.GetFrameTime());

			Clay.BeginLayout();

			using (Clay.Element(new()
			{
				id = Clay.Id(_clayString["OuterContainer"]),
				backgroundColor = new Clay_Color(43, 41, 51),
				layout = new()
				{
					layoutDirection = Clay_LayoutDirection.CLAY_TOP_TO_BOTTOM,
					sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), Clay_SizingAxis.Grow()),
					padding = Clay_Padding.All(16),
					childGap = 16,
				}
			}))
			{
				using (Clay.Element(new()
				{
					id = Clay.Id(_clayString["HeaderBar"]),
					backgroundColor = _contentBackgroundColor,
					cornerRadius =  Clay_CornerRadius.All(8),
					layout = new()
					{
						sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), Clay_SizingAxis.Fixed(60)),
						padding = Clay_Padding.Hor(16),
						childGap = 16,
						childAlignment = new Clay_ChildAlignment(default, Clay_LayoutAlignmentY.CLAY_ALIGN_Y_CENTER)
					}
				}))
				{
					var fileButtonStr = _clayString["FileButton"];
					var fileMenuStr = _clayString["FileMenu"];

					using (Clay.Element(new()
					{
						id = Clay.Id(fileButtonStr),
						layout = new()
						{
							padding = Clay_Padding.HorVer(16, 8)
						},
						backgroundColor = new Clay_Color(140, 140, 140),
						cornerRadius = Clay_CornerRadius.All(5),
					}))
					{
						Clay.TextElement("File", new()
						{
							fontSize = 16,
							textColor = new Clay_Color(255, 255, 255),
						});

						bool isMenuVisible = Clay.IsPointerOver(Clay.GetElementId(fileButtonStr))
							|| Clay.IsPointerOver(Clay.GetElementId(fileMenuStr));

						if (isMenuVisible)
						{
							using (Clay.Element(new()
							{
								id = Clay.Id(fileMenuStr),
								floating = new()
								{
									attachTo = Clay_FloatingAttachToElement.CLAY_ATTACH_TO_PARENT,
									attachPoints = new Clay_FloatingAttachPoints
									{
										parent = Clay_FloatingAttachPointType.CLAY_ATTACH_POINT_LEFT_BOTTOM,
									}
								},
								layout = new()
								{
									padding = Clay_Padding.Ver(8),
								}
							}))
							{
								using (Clay.Element(new()
								{
									layout = new()
									{
										layoutDirection = Clay_LayoutDirection.CLAY_TOP_TO_BOTTOM,
										sizing = new Clay_Sizing(Clay_SizingAxis.Fixed(200), default),
									},
									backgroundColor = new Clay_Color(40, 40, 40),
									cornerRadius = Clay_CornerRadius.All(8),
								}))
								{
									RenderDropdownMenuItem(_clayString["New"]);
									RenderDropdownMenuItem(_clayString["Open"]);
									RenderDropdownMenuItem(_clayString["Close"]);
								}
							}
						}
					}

					RenderHeaderButton(_clayString["Edit"]);
					using (Clay.Element(new()
					{
						layout = new()
						{
							sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), Clay_SizingAxis.Grow())
						}
					}))
					{
					}

					RenderHeaderButton(_clayString["Upload"]);
					RenderHeaderButton(_clayString["Media"]);
					RenderHeaderButton(_clayString["Support"]);
					RenderHeaderButton(_clayString["Upload"]);
				}

				using (Clay.Element(new()
				{
					id = Clay.Id(_clayString["LowerContent"]),
					layout = new()
					{
						sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), Clay_SizingAxis.Grow()),
						childGap = 16,
					}
				}))
				{
					using (Clay.Element(new()
					{
						id = Clay.Id(_clayString["Sidebar"]),
						backgroundColor = _contentBackgroundColor,
						layout = new()
						{
							layoutDirection = Clay_LayoutDirection.CLAY_TOP_TO_BOTTOM,
							padding = Clay_Padding.All(16),
							childGap = 8,
							sizing = new Clay_Sizing(Clay_SizingAxis.Fixed(250), Clay_SizingAxis.Grow()),
						}
					}))
					{
						var sidebarButtonLayout = new Clay_LayoutConfig()
						{
							sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), default),
							padding = Clay_Padding.All(8)
						};

						for (int documentIndex = 0; documentIndex < _documents.Length; documentIndex++)
						{
							var document = _documents[documentIndex];

							if (documentIndex == _selectedDocumentIndex)
							{
								using (Clay.Element(new()
								{
									layout = sidebarButtonLayout,
									backgroundColor = new Clay_Color(120, 120, 120, 255),
									cornerRadius = Clay_CornerRadius.All(8),
								}))
								{
									Clay.TextElement(document.Title, new()
									{
										fontSize = 20,
										textColor = new Clay_Color(255, 255, 255),
									});
								}
							}
							else
							{
								using (var sidebarButton = Clay.OpenElement())
								{
									sidebarButton.Configure(new()
									{
										layout = sidebarButtonLayout,
										backgroundColor = Clay.IsHovered() == false
											? default
											: new Clay_Color(120, 120, 120, 255),
										cornerRadius = Clay_CornerRadius.All(8),
									});
									
									var index = documentIndex;
									Clay.OnHover((_, data, _) =>
									{
										if (data.state == Clay_PointerDataInteractionState.CLAY_POINTER_DATA_PRESSED_THIS_FRAME)
										{
											_selectedDocumentIndex = index;
										}
									});
									Clay.TextElement(document.Title, new()
									{
										fontSize = 20,
										textColor = new Clay_Color(255, 255, 255),
									});
								}
							}
						}
					}

					using (Clay.Element(new()
					{
						id = Clay.Id(_clayString["MainContent"]),
						scroll = new()
						{
							vertical = true
						},
						layout = new()
						{
							layoutDirection = Clay_LayoutDirection.CLAY_TOP_TO_BOTTOM,
							childGap = 16,
							padding = Clay_Padding.All(16),
							sizing = new Clay_Sizing(Clay_SizingAxis.Grow(), Clay_SizingAxis.Grow())
						},
						backgroundColor = _contentBackgroundColor,
					}))
					{
						var doc = _documents[_selectedDocumentIndex];
						Clay.TextElement(doc.Title, new()
						{
							fontSize = 24,
							textColor = new Clay_Color(255, 255, 255),
						});
						Clay.TextElement(doc.Contents, new()
						{
							fontSize = 24,
							textColor = new Clay_Color(255, 255, 255),
						});
					}
				}
			}

			var commands = Clay.EndLayout();


			Raylib.BeginDrawing();
			Raylib.ClearBackground(Raylib.RED);
			RaylibClay.RenderCommands(commands);
			Raylib.EndDrawing();
		}
	}

	private void RenderHeaderButton(Clay_String text)
	{
		using (Clay.Element(new()
		{
			layout = new()
			{
				padding = Clay_Padding.HorVer(16, 8),
			},
			backgroundColor = new Clay_Color(140, 140, 140),
			cornerRadius = Clay_CornerRadius.All(5),
		}))
		{
			Clay.TextElement(text, new Clay_TextElementConfig
			{
				fontSize = 16,
				textColor = new Clay_Color(255, 255, 255),
			});
		}
	}

	private void RenderDropdownMenuItem(Clay_String text)
	{
		using (Clay.Element(new()
		{
			layout = new()
			{
				padding = Clay_Padding.All(16),
			}
		}))
		{
			Clay.TextElement(text, new Clay_TextElementConfig
			{
				fontSize = 16,
				textColor = new Clay_Color(255, 255, 255),
			});
		}
	}

	public void Dispose()
	{
		_clayString.Dispose();
	}
}
