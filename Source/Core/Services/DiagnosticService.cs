using NashEquilibriumFinder.Core.Contracts;
using NashEquilibriumFinder.Core.Domain;

namespace NashEquilibriumFinder.Core.Services;

public class DiagnosticService : IDiagnosticService
{
    public HostInfo GetHostInfo() => new()
    {
        MachineName = Environment.MachineName,
        OSVersion = Environment.OSVersion.ToString(),
        RuntimeEnvironmentVersion = Environment.Version.ToString(),
        NumberOfProcessors = Environment.ProcessorCount
    };
}
