using Stowage;
using Xunit;

namespace Delta.Net.Test {
    public class DeltaLogTest {

        private readonly IFileStorage _storage;

        public DeltaLogTest() {
            _storage = Stowage.Files.Of.LocalDisk(Path.GetFullPath(Path.Combine("data")));
        }

        [Fact]
        public async Task GoldenLogTestAsync() {
            DeltaTable table = await DeltaTable.OpenAsync(_storage, new IOPath("golden", "data-reader-array-primitives"));


        }
    }
}