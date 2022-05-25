using NashEquilibriumFinder.Core.Domain;
using Xunit;

namespace NashEquilibriumFinder.Application.UnitTests;

internal class CorrelatedEquilibriumTestData : TheoryData<Game, CorrelatedEquilibrium>
{
    public CorrelatedEquilibriumTestData()
    {
        Add(_game1, _eq1);
        Add(_game2, _eq2);
        Add(_game3, _eq3);
        Add(_game4, _eq4);
        Add(_game5, _eq5);
        Add(_game6, _eq6);
        Add(_game7, _eq7);
    }

    private readonly Game _game1 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 4, 1 },
                new[] { 5, 0 }
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 4, 5 },
                new[] { 1, 0 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq1 = new()
    {
        FirstPlayerGamePayoff = (double)10 / 3,
        SecondPlayerGamePayoff = (double)10 / 3,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            new(1,3),
            new(1,3),
            new(1,3),
            0
        }
    };

    private readonly Game _game2 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 2, 0 },
                new[] { 0, 1 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 1, 0 },
                new[] { 0, 2 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq2 = new()
    {
        FirstPlayerGamePayoff = 2,
        SecondPlayerGamePayoff = 1,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            1, 0, 0, 0 // TODO: Is it really correct?
        }
    };

    private readonly Game _game3 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 10, 50 },
                new[] {  1,  2 },
                new[] {  3,  4 },
                new[] {  5,  6 },
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 10, 50 },
                new[] {  1,  2 },
                new[] {  3,  4 },
                new[] {  5,  6 },
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq3 = new()
    {
        FirstPlayerGamePayoff = 50,
        SecondPlayerGamePayoff = 50,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            0, 1, 0, 0, 0, 0, 0, 0
        }
    };

    // TODO: Think about recognizing dominant strategies
    private readonly Game _game4 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 10, 8 },
                new[] {  5, 7 }
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 2, 4 },
                new[] { 4, 3 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq4 = new()
    {
        FirstPlayerGamePayoff = 8,
        SecondPlayerGamePayoff = 4,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            0, 1, 0, 0
        }
    };

    private readonly Game _game5 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 5, 4 },
                new[] { 4, 5 }
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 4, 5 },
                new[] { 5, 4 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq5 = new()
    {
        FirstPlayerGamePayoff = 4.5,
        SecondPlayerGamePayoff = 4.5,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            new(1,4),
            new(1,4),
            new(1,4),
            new(1,4)
        }
    };

    private readonly Game _game6 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 10, 20 },
                new[] {  0, 19 } // I guess the 10/10 payoffs are due to this strategy is dominanted by the first one
            },
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { 10,  0 },
                new[] {  0, 19 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq6 = new()
    {
        FirstPlayerGamePayoff = 10,
        SecondPlayerGamePayoff = 10,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            1, 0, 0, 0
        }
    };

    private readonly Game _game7 = new()
    {
        FirstPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { -10,  0 },
                new[] { -20, -1 }
            }
        },
        SecondPlayer = new Player()
        {
            Payouts = new int[][]
            {
                new[] { -10, -20 },
                new[] {   0,  -1 }
            }
        }
    };

    private readonly CorrelatedEquilibrium _eq7 = new()
    {
        FirstPlayerGamePayoff = -10,
        SecondPlayerGamePayoff = -10,
        StrategyProfilesProbabilities = new List<SimpleFraction>()
        {
            1, 0, 0, 0
        }
    };
}