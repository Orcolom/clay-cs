# 0.4.x - 2025-11-15
- UPDATED CLAY: from 0.13.0 `b33ba4f` to 0.14.0 `c13feb2`
- Fixed: MeasureTextDelegate now gets cached correctly to avoid being collected by Gc
- BREAKING: Merged `Clay.Element()` (without argument) into `Clay.OpenElement` to avoid confusion with `Clay.Element(Clay_ElementDeclaration)` as using `Clay.Element()` would cause unrecoverable exceptions if not closed.
- BREAKING: Renamed `Clay.OpenTextElement` to `Clay.TextElement` as you don't have to close it.
- Added: `Clay_CornerRadius` utility methods `Top()`, `Bottom()`, `Left()`,  `Right()`
- Added support for `netstandard2.1`.

# 0.3.1 - 2024-03-02
- First automated release
- Added: nugget package to readme
- Added: Clay.Builder can now build multiple targets.

# 0.3.0 - 2024-02-25
- Added: Nuget package
- Added: Easier ClayElement shorthands `Clay.Element()` and `Clay.Element(...)`. These mimic `ClayElement.Open()` and `ClayElement.OpenAndConfigure()` respectively. 
- Fixed: Callbacks no longer get garbage collected.
- Fixed: Clay.Builder did not account for the moved clay.h file

# 0.2.0 - 2024-02-19
- clay 0.13.0
- Added clay.h as a submodule
- Added support for .net8

# 0.1.0 - 2024-02-01
- Clay 0.11.0
- Published project
