using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace DynamicDataVNext.Benchmarks;

public static class EntryPoint
{
    public static void Main(string[] args)
        => BenchmarkSwitcher
            .FromAssembly(Assembly.GetExecutingAssembly())
            .Run(args, DefaultConfig.Instance
                .WithArtifactsPath(Path.Combine(
                    GetProjectRootDirectory(),
                    Path.GetFileName(DefaultConfig.Instance.ArtifactsPath))));

    // Cheesy way to get the project path, by getting the compiler to inject it.
    private static string GetProjectRootDirectory([CallerFilePath] string? callerFilePath = null)
        => Path.GetDirectoryName(callerFilePath)!;
}
