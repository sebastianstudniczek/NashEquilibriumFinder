using Rationals;
using System.Collections;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Algorithms.UnitTests;

internal class GameDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new()
    {
        new object[] { 1, tableaux1, tableaux1AfterPivot },
        new object[] { 4, tableaux2, tableaux2AfterPivot },
        new object[] { 2, tableaux3, tableaux3AfterPivot },
        new object[] { 5, tableaux4, tableaux4AfterPivot },
    };
    public IEnumerator<object[]> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    static readonly Rational[,] tableaux1 = new Rational[,]
    {
        { Q.Zero, Q.Zero,      1,      2,      3,      4,      5 },
        {     -1,      1, Q.Zero, Q.Zero,     -5,     -5,    -10 },
        {     -2,      1, Q.Zero, Q.Zero,     -1,     -8,     -3 },
        {     -3,      1,     -2,     -3, Q.Zero, Q.Zero, Q.Zero },
        {     -4,      1,     -8,     -2, Q.Zero, Q.Zero, Q.Zero },
        {     -5,      1,     -2,     -7, Q.Zero, Q.Zero, Q.Zero }
    };

    static readonly Rational[,] tableaux1AfterPivot = new Rational[,]
    {
        { Q.Zero, Q.Zero,     -4,       2,      3,      4,      5 },
        {     -1,      1, Q.Zero,  Q.Zero,     -5,     -5,    -10 },
        {     -2,      1, Q.Zero,  Q.Zero,     -1,     -8,     -3 },
        {     -3, (Q)3/4, (Q)1/4,-(Q) 5/2, Q.Zero, Q.Zero, Q.Zero },
        {      1, (Q)1/8,-(Q)1/8,-(Q) 1/4, Q.Zero, Q.Zero, Q.Zero },
        {     -5, (Q)3/4, (Q)1/4,-(Q)13/2, Q.Zero, Q.Zero, Q.Zero }
    };

    static readonly Rational[,] tableaux2 = tableaux1AfterPivot;

    static readonly Rational[,] tableaux2AfterPivot = new Rational[,]
    {
        { Q.Zero, Q.Zero,     -4,       2,       3,     -2,       5 },
        {     -1, (Q)3/8, Q.Zero,  Q.Zero,-(Q)35/8, (Q)5/8,-(Q)65/8 },
        {      4, (Q)1/8, Q.Zero,  Q.Zero,-(Q) 1/8,-(Q)1/8,-(Q) 3/8 },
        {     -3, (Q)3/4, (Q)1/4,-(Q) 5/2,  Q.Zero, Q.Zero,  Q.Zero },
        {      1, (Q)1/8,-(Q)1/8,-(Q) 1/4,  Q.Zero, Q.Zero,  Q.Zero },
        {     -5, (Q)3/4, (Q)1/4,-(Q)13/2,  Q.Zero, Q.Zero,  Q.Zero }
    };

    static readonly Rational[,] tableaux3 = tableaux2AfterPivot;

    static readonly Rational[,] tableaux3AfterPivot = new Rational[,]
    {
        { Q.Zero,  Q.Zero,      -4,      -5,       3,     -2,       5 },
        {     -1, (Q) 3/8,  Q.Zero,  Q.Zero,-(Q)35/8, (Q)5/8,-(Q)65/8 },
        {      4, (Q) 1/8,  Q.Zero,  Q.Zero,-(Q) 1/8,-(Q)1/8,-(Q) 3/8 },
        {     -3, (Q)6/13, (Q)2/13, (Q)5/13,  Q.Zero, Q.Zero,  Q.Zero },
        {      1, (Q)5/52,-(Q)7/52, (Q)1/26,  Q.Zero, Q.Zero,  Q.Zero },
        {      2, (Q)3/26, (Q)1/26,-(Q)2/13,  Q.Zero, Q.Zero,  Q.Zero }
    };

    static readonly Rational[,] tableaux4 = tableaux3AfterPivot;

    static readonly Rational[,] tableaux4AfterPivot = new Rational[,]
    {
        { Q.Zero,  Q.Zero,      -4,      -5,       3,      -2,      -1 },
        {      5, (Q)3/65,  Q.Zero,  Q.Zero,-(Q)7/13, (Q)1/13,-(Q)8/65 },
        {      4, (Q)7/65,  Q.Zero,  Q.Zero, (Q)1/13,-(Q)2/13, (Q)3/65 },
        {     -3, (Q)6/13, (Q)2/13, (Q)5/13,  Q.Zero,  Q.Zero,  Q.Zero },
        {      1, (Q)5/52,-(Q)7/52, (Q)1/26,  Q.Zero,  Q.Zero,  Q.Zero },
        {      2, (Q)3/26, (Q)1/26,-(Q)2/13,  Q.Zero,  Q.Zero,  Q.Zero }
    };
}
