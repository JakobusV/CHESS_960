using NUnit.Framework;
using Chess;

namespace ChessTestProj
{
    public class Tests
    {
        [Test]
        public void Test1()
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
        public void NormalKingCheck()
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
        public void NormalQueenCheck()
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
        public void NinesixtyTest1()
        {
            Form1 form = new Form1();



        }
    }
}