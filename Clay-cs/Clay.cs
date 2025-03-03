using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace Clay_cs;

public unsafe delegate Clay_Dimensions ClayMeasureTextDelegate(Clay_StringSlice slice, Clay_TextElementConfig* config, void* userData);

public delegate void ClayErrorDelegate(Clay_ErrorData data);

public delegate void ClayOnHoverDelegate(Clay_ElementId id, Clay_PointerData data, nint userData);

public unsafe delegate Clay_Vector2 ClayQueryScrollOffsetDelegate(uint elementId, void* userdata);

public static class Clay
{
	internal static readonly ClayStringCollection ClayStrings = new();
	internal static readonly Dictionary<nint, ClayManagedContext> ClayContexts = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetMaxElementCount()
	{
		return ClayInterop.Clay_GetMaxElementCount();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetMaxElementCount(int maxElementCount)
	{
		ClayInterop.Clay_SetMaxElementCount(maxElementCount);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetMaxMeasureTextCacheWordCount()
	{
		return ClayInterop.Clay_GetMaxMeasureTextCacheWordCount();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetMaxMeasureTextCacheWordCount(int maxMeasureTextCacheWordCount)
	{
		ClayInterop.Clay_SetMaxMeasureTextCacheWordCount(maxMeasureTextCacheWordCount);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ResetMeasureTextCache()
	{
		ClayInterop.Clay_ResetMeasureTextCache();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint MinMemorySize() => ClayInterop.Clay_MinMemorySize();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe ClayArenaHandle CreateArena(uint memorySize)
	{
		var ptr = Marshal.AllocHGlobal((int)memorySize);
		var arena = ClayInterop.Clay_CreateArenaWithCapacityAndMemory(memorySize, (void*)ptr);
		return new ClayArenaHandle { Arena = arena, Memory = ptr };
	}
	
	internal static void FreeArena(ClayArenaHandle handle)
	{
		// use the memory to find the linked context
		nint key = default;
		foreach (var clayContext in ClayContexts)
		{
			if (clayContext.Value.ArenaMemory == handle.Memory)
			{
				key = clayContext.Key;
			}
		}
		
		// free everything
		ClayContexts.Remove(key);
		Marshal.FreeHGlobal(handle.Memory);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Clay_Context* GetCurrentContext()
	{
		return ClayInterop.Clay_GetCurrentContext();
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static unsafe ClayManagedContext GetManagedContext()
	{
		return ClayContexts[(IntPtr)GetCurrentContext()];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void SetCurrentContext(Clay_Context* context)
	{
		ClayInterop.Clay_SetCurrentContext(context);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void SetMeasureTextFunction(ClayMeasureTextDelegate measureText)
	{
		// TODO: do we need to dealloc the ptr?
		var ptr = Marshal.GetFunctionPointerForDelegate(measureText);
		var castPtr = (delegate* unmanaged[Cdecl]<Clay_StringSlice, Clay_TextElementConfig*, void*, Clay_Dimensions>)ptr;
		ClayInterop.Clay_SetMeasureTextFunction(castPtr, null);
	}

	public static unsafe Clay_Context* Initialize(
		ClayArenaHandle handle,
		Clay_Dimensions dimensions,
		ClayErrorDelegate errorHandler)
	{
		// TODO: handle userdata
		var ptr = Marshal.GetFunctionPointerForDelegate(errorHandler);
		var castPtr = (delegate* unmanaged[Cdecl]<Clay_ErrorData, void>)ptr;

		var context = ClayInterop.Clay_Initialize(handle.Arena, dimensions, new Clay_ErrorHandler
		{
			errorHandlerFunction = castPtr,
		});

		ClayContexts[(IntPtr)context] = new ClayManagedContext
		{
			ErrorHandler = errorHandler,
			ArenaMemory = handle.Memory,
		};
		
		return context;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetCullingEnabled(bool state)
	{
		ClayInterop.Clay_SetCullingEnabled(state);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetDebugModeEnabled(bool state)
	{
		ClayInterop.Clay_SetDebugModeEnabled(state);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsDebugModeEnabled()
	{
		return ClayInterop.Clay_IsDebugModeEnabled();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void BeginLayout()
	{
		var context = GetManagedContext();
		context.OnHover.Clear();
		ClayInterop.Clay_BeginLayout();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_RenderCommandArray EndLayout()
	{
		return ClayInterop.Clay_EndLayout();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Clay_RenderCommand* RenderCommandArrayGet(Clay_RenderCommandArray arr, int index)
	{
		return ClayInterop.Clay_RenderCommandArray_Get(&arr, index);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetPointerState(Vector2 position, bool isMouseDown)
	{
		ClayInterop.Clay_SetPointerState(position, isMouseDown);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsPointerOver(Clay_ElementId elementId)
	{
		return ClayInterop.Clay_PointerOver(elementId);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsHovered()
	{
		return ClayInterop.Clay_Hovered();
	}

	public static unsafe void OnHover(ClayOnHoverDelegate onHover, nint userData = 0)
	{
		var context = GetManagedContext();
		context.OnHover.Add(onHover);
		
		var ptr = Marshal.GetFunctionPointerForDelegate(onHover);
		var castPtr = (delegate* unmanaged[Cdecl]<Clay_ElementId, Clay_PointerData, nint, void>)ptr;
		ClayInterop.Clay_OnHover(castPtr, userData);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ScrollContainerData GetScrollContainerData(Clay_ElementId id)
	{
		return ClayInterop.Clay_GetScrollContainerData(id);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void UpdateScrollContainers(bool enableDragScrolling, Vector2 moveDelta, float timeDelta)
	{
		ClayInterop.Clay_UpdateScrollContainers(enableDragScrolling, moveDelta, timeDelta);
	}

	public static unsafe void SetQueryScrollOffsetFunction(ClayQueryScrollOffsetDelegate queryScrollOffsetFunction)
	{
		var context = GetManagedContext();
		context.QueryScrollOffset = queryScrollOffsetFunction;
		
		var ptr = Marshal.GetFunctionPointerForDelegate(queryScrollOffsetFunction);
		var castPtr = (delegate* unmanaged[Cdecl]<uint, void*, Clay_Vector2>)ptr;
		ClayInterop.Clay_SetQueryScrollOffsetFunction(castPtr, null);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetLayoutDimensions(Clay_Dimensions dimensions)
	{
		ClayInterop.Clay_SetLayoutDimensions(dimensions);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId GetElementId(Clay_String idString)
	{
		return ClayInterop.Clay_GetElementId(idString);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId GetElementId(Clay_String idString, uint index)
	{
		return ClayInterop.Clay_GetElementIdWithIndex(idString, index);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementData GetElementData(Clay_ElementId id)
	{
		return ClayInterop.Clay_GetElementData(id);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void OpenElement()
	{
		ClayInterop.Clay__OpenElement();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void OpenTextElement(string text, Clay_TextElementConfig c)
	{
		OpenTextElement(ClayStrings.Get(text), c);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void OpenTextElement(Clay_String text, Clay_TextElementConfig c)
	{
		ClayInterop.Clay__OpenTextElement(text, ClayInterop.Clay__StoreTextElementConfig(c));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ConfigureOpenElement(Clay_ElementDeclaration declaration)
	{
		ClayInterop.Clay__ConfigureOpenElement(declaration);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CloseElement()
	{
		ClayInterop.Clay__CloseElement();
	}

	public static ClayElement Element()
	{
		return ClayElement.Open();
	}
	
	public static ClayElement Element(Clay_ElementDeclaration declaration)
	{
		return ClayElement.Open().Configure(declaration);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static uint GetParentElementId()
	{
		return ClayInterop.Clay__GetParentElementId();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId Id(string text)
	{
		return Id(ClayStrings.Get(text));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId Id(Clay_String text)
	{
		return HashId(text, 0, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId Id(string text, int offset)
	{
		return Id(ClayStrings.Get(text), offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId Id(Clay_String text, int offset)
	{
		return HashId(text, (uint)offset, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId IdLocal(string text)
	{
		return IdLocal(ClayStrings.Get(text));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId IdLocal(Clay_String text)
	{
		return HashId(text, 0, GetParentElementId());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId IdLocal(string text, int offset)
	{
		return IdLocal(ClayStrings.Get(text), offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Clay_ElementId IdLocal(Clay_String text, int offset)
	{
		return HashId(text, (uint)offset, GetParentElementId());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Clay_ElementId HashId(Clay_String text, uint offset, uint seed)
	{
		return ClayInterop.Clay__HashString(text, offset, seed);
	}
}
