using Apache.Arrow;
using Delta.Net.Log.Actions;
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

            Assert.Equal(5, table.Log.Actions.Count);

            // 0
            CommitInfoAction a0 = (CommitInfoAction)table.Log.Actions[0];
            Assert.Equal(DeltaAction.CommitInfo, a0.DeltaAction);

            // 1
            ProtocolEvolutionAction a1 = (ProtocolEvolutionAction)table.Log.Actions[1];
            Assert.Equal(DeltaAction.Protocol, a1.DeltaAction);
            Assert.Equal(1, a1.MinReaderVersion);
            Assert.Equal(2, a1.MinWriterVersion);

            // 2
            ChangeMetadataAction a2 = (ChangeMetadataAction)table.Log.Actions[2];
            Assert.Equal(DeltaAction.Metadata, a2.DeltaAction);

        }
    }
}