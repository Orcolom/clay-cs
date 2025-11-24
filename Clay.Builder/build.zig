const std = @import("std");

// list of targets to build

const TargetData = struct {
    target: std.Target.Query,
    as_dll: bool,
};

const TARGETS = [_]TargetData{
    .{ .target = .{ .cpu_arch = .x86_64, .os_tag = .windows }, .as_dll = true },
    .{ .target = .{ .cpu_arch = .x86_64, .os_tag = .windows }, .as_dll = false },
};

pub fn build(b: *std.Build) void {

    // Standard optimization options allow the person running `zig build` to select
    // between Debug, ReleaseSafe, ReleaseFast, and ReleaseSmall. Here we do not
    // set a preferred release mode, allowing the user to decide how to optimize.
    const optimize = b.standardOptimizeOption(.{});

    for (TARGETS) |TARGETDATA| {
        const name = std.fmt.allocPrint(b.allocator, "{s}-{s}-{s}-Clay", .{
            @tagName(TARGETDATA.target.cpu_arch.?),
            @tagName(TARGETDATA.target.os_tag.?),
            if (TARGETDATA.as_dll) "dll" else "lib",
        }) catch |err| {
            std.debug.print("Failed to allocate memory {}\n", .{err});
            return;
        };

        const resolvedTarget = b.resolveTargetQuery(TARGETDATA.target);

        const flags = [_][]const u8{
            "-std=c99",
        };

        var lib: *std.Build.Step.Compile = undefined;

        if (TARGETDATA.as_dll) {
            lib = b.addSharedLibrary(.{
                .name = name,
                .target = resolvedTarget,
                .optimize = optimize,
            });
        } else {
            lib = b.addStaticLibrary(.{
                .name = name,
                .target = resolvedTarget,
                .optimize = optimize,
            });
        }

        lib.linkLibC();
        lib.addIncludePath(b.path("src/clay/clay.h"));
        lib.addCSourceFile(.{ .file = b.path("src/clay.c"), .flags = &flags });
        if (TARGETDATA.as_dll) {
            lib.root_module.addCMacro("CLAY_DLL", "1");
        }

        b.installArtifact(lib);
    }
}
