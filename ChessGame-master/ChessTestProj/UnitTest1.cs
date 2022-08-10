using NUnit.Framework;
using Chess;
using System.Text;
using System.Linq;

namespace ChessTestProj
{
    public class Tests
    {
        [Test]
        public void DefaultRookPlacement()
        {
            Form1 form = new Form1();

            Assert.AreEqual(15, form.map[0, 0]);
        }

        [Test]
        public void DefaultBoardTest()
        {
            Form1 form = new Form1();

            int[,] map = new int[8, 8]
            {{15,14,13,12,11,13,14,15 },{16,16,16,16,16,16,16,16 },{0,0,0,0,0,0,0,0 },{0,0,0,0,0,0,0,0 },{0,0,0,0,0,0,0,0 },{0,0,0,0,0,0,0,0 },{26,26,26,26,26,26,26,26 },{25,24,23,22,21,23,24,25 } };

            Assert.AreEqual(map, form.map);
        }

        [Test]
        public void DefaultKingCheck()
        {
            //kings: 11 & 21

            Form1 form = new Form1();
            
            int whiteKing = form.map[0, 4];
            int blackKing = form.map[7, 4];

            if(blackKing - whiteKing == 10 || blackKing - whiteKing == -10)
            {
                Assert.Pass();
            }            
        }

        [Test]
        public void DefaultQueenCheck()
        {
            //queens: 12 & 22

            Form1 form = new Form1();

            int whiteQueen = form.map[0, 3];
            int blackQueen = form.map[7, 3];

            if (blackQueen - whiteQueen == 10 || blackQueen - whiteQueen == -10)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void C960_BadSeed_Low()
        {
            Form1 form = new Form1();

            int seed = -1;

            string result = form.Chess960Positions(seed);

            Assert.AreEqual("rnbqkbnr", result);
        }

        [Test]
        public void C960_BadSeed_High()
        {
            Form1 form = new Form1();

            int seed = 961;

            string result = form.Chess960Positions(seed);

            Assert.AreEqual("rnbqkbnr", result);
        }

        [Test]
        public void C960_BadKRNcode()
        {
            Form1 form = new Form1();

            int kern_input = 10; // Should be too high for switch

            string result = form.GetKernCode(kern_input);

            int knight_count = 0; // Any valid pattern should have exactly 2

            foreach (char c in result)
            {
                if (c == 'n')
                {
                    knight_count++;
                }
            }

            Assert.IsFalse(knight_count == 2);
        }

        [Test]
        public void C960_GoodKRNcode()
        {
            Form1 form = new Form1();

            int kern_input = 5; // Switch range from 0-9

            string result = form.GetKernCode(kern_input);

            int knight_count = 0; // Any valid pattern should have exactly 2

            foreach (char c in result)
            {
                if (c == 'n')
                {
                    knight_count++;
                }
            }

            Assert.IsTrue(knight_count == 2);
        }

        [Test]
        public void C960_ConsistentSeed()
        {
            Form1 form = new Form1();

            int seed = 69;

            string result1 = form.Chess960Positions(seed);

            string result2 = form.Chess960Positions(seed);

            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void C960_RandomSeed()
        {
            Form1 form = new Form1();

            int seed = 960;

            string result1 = form.Chess960Positions(seed);

            // the test is fast enough that the randoms resolve to the same number. kinda cool
            // a half second pause is used to fix this
            System.Threading.Thread.Sleep(500);

            string result2 = form.Chess960Positions(seed);

            Assert.AreNotEqual(result1, result2);

            // Note this test can fail if you are lucky enough to get the same random seed twice
        }
    }
}