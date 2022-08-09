using NUnit.Framework;
using Chess;

namespace ChessTestProj
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Form1 form = new Form1();

            Assert.AreEqual(15, form.map[0, 0]);
        }
    }
}