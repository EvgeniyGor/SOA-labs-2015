using NUnit.Framework;
using Serialization;

namespace SerializationTests
{
    public class XmlSerializerTests : TestBase
    {
        private XmlSerializer _xmlSerializer;

        public override void SetUp()
        {
            _xmlSerializer = new XmlSerializer();
        }

        [Test]
        public void SerializeTest()
        {
            var inputObj = new Input
            {
                K = 5,
                Muls = new[] { 3, -1, 2 },
                Sums = new[] { 3.5m, -1.5m, 2.5m }
            };

            var expected = "<Input><K>5</K><Sums><decimal>3.5</decimal><decimal>-1.5</decimal><decimal>2.5</decimal></Sums><Muls><int>3</int><int>-1</int><int>2</int></Muls></Input>";
            var actual = _xmlSerializer.Serialize(inputObj);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DeserializeTest()
        {
            var serializedString = "<Input><K>5</K><Sums><decimal>3.5</decimal><decimal>-1.5</decimal><decimal>2.5</decimal></Sums><Muls><int>3</int><int>-1</int><int>2</int></Muls></Input>";

            var exprected = new Input
            {
                K = 5,
                Muls = new[] { 3, -1, 2 },
                Sums = new[] { 3.5m, -1.5m, 2.5m }
            };

            var actual = _xmlSerializer.Deserialize<Input>(serializedString);

            Assert.AreEqual(exprected.K, actual.K);
            CollectionAssert.AreEquivalent(exprected.Muls, actual.Muls);
            CollectionAssert.AreEquivalent(exprected.Sums, actual.Sums);
        }
    }
}