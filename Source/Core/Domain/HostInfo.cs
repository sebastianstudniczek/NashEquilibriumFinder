namespace NashEquilibriumFinder.Core.Domain;
public class HostInfo
{
    public string? MachineName { get; init; }
    public string? OSVersion { get; init; }
    public string? RuntimeEnvironmentVersion { get; init; }
    public int NumberOfProcessors { get; init; }
}
