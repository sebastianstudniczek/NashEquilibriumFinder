using NashEquilibriumFinder.Core.Domain;

namespace NashEquilibriumFinder.Core.Contracts;

public interface IGameTheoryService
{
    NashEquilibrium GetNashEquilibriumFromLemkeHowson(Game twoPersonGameData);
    CorrelatedEquilibrium GetCorrelatedEquilibriumFromSimplex(Game twoPersonGameData);
    int[][] GetParetoFront(int[,] firstPlayerMatrix, int[,] secondPlayerMatrix);
}
