using Xunit;
using Q = Rationals.Rational;

namespace NashEquilibriumFinder.Algorithms.UnitTests {
    public class ContraintsTestData : TheoryData<int, Q[,], IEnumerable<Q[]>> {
        public ContraintsTestData() {
            // First game
            Add(1, _firstPlayerPayouts1, _firstPlayerContraints1);
            Add(2, _secondPlayerPayouts1, _secondPlayerConstraints1);

            // Second game
            Add(1, _firstPlayerPayouts2, _firstPlayerContraints2);
            Add(2, _secondPlayerPayouts2, _secondPlayerContraints2);
        }

        // First game
        private readonly Q[,] _firstPlayerPayouts1 = new Q[,] {
            { 0, 0, 2 }, // A
            { 0, 2, 1 }, // B
            { 2, 1, 1 }  // C
        };

        private readonly IEnumerable<Q[]> _firstPlayerContraints1 = new List<Q[]> {
                    //p1 p2 p3 p4 p5 p6 p7 p8 p9
            new Q[] { 0, 2,-1, 0, 0, 0, 0, 0, 0 },
            new Q[] { 2, 1,-1, 0, 0, 0, 0, 0 ,0 },
            new Q[] { 0, 0, 0, 0,-2, 1, 0, 0, 0 },
            new Q[] { 0, 0, 0, 2,-1, 0, 0, 0, 0 },
            new Q[] { 0, 0, 0, 0, 0, 0,-2,-1, 1 },
            new Q[] { 0, 0, 0, 0, 0, 0,-2, 1, 0 }
        };

        private readonly Q[,] _secondPlayerPayouts1 = new Q[,] {
            //D  E  F
            {-1, 2, 3 },
            { 0, 1,-1 },
            { 2, 4,-1 }
        };

        private readonly IEnumerable<Q[]> _secondPlayerConstraints1 = new List<Q[]> {
            new Q[] { 3, 0, 0, 1, 0, 0, 2, 0, 0 },
            new Q[] { 4, 0, 0,-1, 0, 0,-3, 0 ,0 },
            new Q[] { 0,-3, 0, 0,-1, 0, 0,-2, 0 },
            new Q[] { 0, 1, 0, 0,-2, 0, 0,-5, 0 },
            new Q[] { 0, 0,-4, 0, 0, 1, 0, 0, 3 },
            new Q[] { 0, 0,-1, 0, 0, 2, 0, 0, 5 },
        };

        // Second game
        private readonly Q[,] _firstPlayerPayouts2 = new Q[,] {
            { 10, 50 },
            {  1,  2 },
            {  3,  4 },
            {  5,  6 }
        };

        private readonly IEnumerable<Q[]> _firstPlayerContraints2 = new List<Q[]>
        {
            new Q[] {-9, -48, 0, 0,  0,  0, 0,  0 },
            new Q[] {-7, -46, 0, 0,  0,  0, 0,  0 },
            new Q[] {-5, -44, 0, 0,  0,  0, 0,  0 },
            new Q[] { 0,   0, 9,48,  0,  0, 0,  0 },
            new Q[] { 0,   0, 2, 2,  0,  0, 0,  0 },
            new Q[] { 0,   0, 4, 4,  0,  0, 0,  0 },
            new Q[] { 0,   0, 0, 0,  7, 46, 0,  0 },
            new Q[] { 0,   0, 0, 0, -2, -2, 0,  0 },
            new Q[] { 0,   0, 0, 0,  2,  2, 0,  0 },
            new Q[] { 0,   0, 0, 0,  0,  0, 5, 44 },
            new Q[] { 0,   0, 0, 0,  0,  0,-4, -4 },
            new Q[] { 0,   0, 0, 0,  0,  0,-2, -2 }
        };

        private readonly Q[,] _secondPlayerPayouts2 = new Q[,] {
            { 10, 50 },
            {  1,  2 },
            {  3,  4 },
            {  5,  6 }
        };

        private readonly IEnumerable<Q[]> _secondPlayerContraints2 = new List<Q[]>
        {
            new Q[] { 40,  0, 1, 0, 1, 0, 1, 0 },
            new Q[] {  0,-40, 0,-1, 0,-1, 0,-1 },
        };
    }
}
