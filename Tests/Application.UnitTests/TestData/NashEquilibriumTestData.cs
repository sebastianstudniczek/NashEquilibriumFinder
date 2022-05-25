using NashEquilibriumFinder.Core.Domain;
using Xunit;

namespace NashEquilibriumFinder.Application.UnitTests;

internal class NashEquilibriumTestData : TheoryData<Game, NashEquilibrium>
{
    public NashEquilibriumTestData()
    {
        Add(_game1, _eq1);
        Add(_game2, _eq2);
        Add(_game3, _eq3);
        Add(_game4, _eq4);
        Add(_game5, _eq5);
        Add(_game6, _eq6);
        Add(_game7, _eq7);
        Add(_game8, _eq8);
        Add(_game9, _eq9);
    }

    private readonly Game _game1 = new Game()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 1, -1 },
                new[] { -1, 1 }
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { -1, 1 },
                new[] { 1, -1 }
            }
        }
    };

    private readonly NashEquilibrium _eq1 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(1,2),
            new(1,2)
        },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(1,2),
            new(1,2)
        }
    };

    private readonly Game _game2 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 0,  2, 3 },
                new[] { 3, -2, 2 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 1, -1, 0 },
                new[] {-1,  1,-2 }
            }
        }
    };

    private readonly NashEquilibrium _eq2 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>
        {
            new(1,2),
            new(1,2)
        },
        SecondPlayerStrategyProbabilities  = new List<SimpleFraction>
        {
            new(4,7),
            new(3,7),
            0
        }
    };

    private readonly Game _game3 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 1, 3, 0 },
                new[] { 0, 0, 2 },
                new[] { 2, 1, 1 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 2, 1, 0 },
                new[] { 1, 3, 1 },
                new[] { 0, 0, 3 }
            }
        }
    };

    private readonly NashEquilibrium _eq3 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(6,13),
            new(3,13),
            new(4,13)
        },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(1,9),
            new(1,3),
            new(5,9)
        }
    };

    private readonly Game _game4 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 1, 2 },
                new[] { 3, 4 },
                new[] { 5, 6 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] {  7,  8 },
                new[] {  9, 10 },
                new[] { 11, 12 }
            }
        }
    };

    private readonly NashEquilibrium _eq4 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 0, 1 },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 1 }
    };

    private readonly Game _game5 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts  = new int[][]
            {
                new[] { 3, 1 },
                new[] {-1, 2 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] {2, 3 },
                new[] {4, 1 }
            }
        }
    };

    private readonly NashEquilibrium _eq5 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(3,4),
            new(1,4)
        },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>()
        {
            new(1,5),
            new(4,5)
        }
    };

    private readonly Game _game6 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][] { new[] { 0, 1 } }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][] { new[] { 2, 3 } }
        }
    };

    private readonly NashEquilibrium _eq6 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>() { 1 },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 1 }
    };

    private readonly Game _game7 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 0 },
                new[] { 1 },
                new[] { 2 },
                new[] { 3 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 4 },
                new[] { 5 },
                new[] { 6 },
                new[] { 7 }
            }
        }
    };

    private readonly NashEquilibrium _eq7 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 0, 0, 1 },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>() { 1 }
    };

    private readonly Game _game8 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 124, 170, 197 },
                new[] { 146, 253, 114 },
                new[] { 267, 110, 262 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] {270, 194, 100 },
                new[] {148, 161, 175 },
                new[] {163, 260, 268 }
            }
        }
    };

    private readonly NashEquilibrium _eq8 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 0, 1 },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>() { 0, 0, 1 }
    };

    private readonly Game _game9 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][] { new[] { 1 } },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][] { new[] { 2 } }
        }
    };

    private readonly NashEquilibrium _eq9 = new()
    {
        FirstPlayerStrategyProbabilities = new List<SimpleFraction>() { 1 },
        SecondPlayerStrategyProbabilities = new List<SimpleFraction>() { 1 }
    };
}
