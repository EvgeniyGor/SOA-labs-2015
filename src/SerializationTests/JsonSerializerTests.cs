using NUnit.Framework;
using Serialization;

namespace SerializationTests
{
    public class JsonSerializerTests : TestBase
    {
        private JsonSerializer _jsonSerializer;

        public override void SetUp()
        {
            _jsonSerializer = new JsonSerializer();
        }

        [Test]
        public void SerializeTest()
        {
            var inputObj = new Input
            {
                K = 5,
                Muls = new[] {3, -1, 2},
                Sums = new[] {3.5m, -1.5m, 2.5m}
            };

            var expected = "{\"K\":5,\"Sums\":[3.5,-1.5,2.5],\"Muls\":[3,-1,2]}";
            var actual = _jsonSerializer.Serialize(inputObj);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DeserializeTest()
        {
            var serializedString = "{\"K\":5,\"Sums\":[3.5,-1.5,2.5],\"Muls\":[3,-1,2]}";

            var exprected = new Input
            {
                K = 5,
                Muls = new[] { 3, -1, 2 },
                Sums = new[] { 3.5m, -1.5m, 2.5m }
            };

            var actual = _jsonSerializer.Deserialize<Input>(serializedString);
            
            Assert.AreEqual(exprected.K, actual.K);
            CollectionAssert.AreEquivalent(exprected.Muls, actual.Muls);
            CollectionAssert.AreEquivalent(exprected.Sums, actual.Sums);
        }
    }
}
