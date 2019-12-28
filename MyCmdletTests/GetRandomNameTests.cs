using MyCmdlets;
using NUnit.Framework;
using System.Linq;

namespace MyCmdletTests
{
    [TestFixture]
    public class GetRandomNameTests
    {
        [Test]
        public void TestReplacingNathan()
        {
            var cmdlet = new GetRandomName() { Name = "Nathan" };
            var results = cmdlet.Invoke<string>().ToList();
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Length, 6);
        }

        [Test]
        public void TestReplacingJohn()
        {
            var cmdlet = new GetRandomName() { Name = "John" };
            var results = cmdlet.Invoke<string>().ToList();
            Assert.AreEqual(results.Count, 1);
            Assert.AreEqual(results[0].Length, 4);
        }
    }
}
