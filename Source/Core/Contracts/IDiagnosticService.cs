using NashEquilibriumFinder.Core.Domain;

namespace NashEquilibriumFinder.Core.Contracts
{
    public interface IDiagnosticService
    {
        HostInfo GetHostInfo();
    }
}
