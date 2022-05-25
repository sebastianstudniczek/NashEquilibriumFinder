using Microsoft.Extensions.DependencyInjection;
using NashEquilibriumFinder.Core.Contracts;
using NashEquilibriumFinder.Core.Services;

namespace NashEquilibriumFinder.Core;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddTransient<IGameTheoryService, GameTheoryService>();
        services.AddTransient<IDiagnosticService, DiagnosticService>();

        return services;
    }
}
