using System.Text.Json;
using System.Text.Json.Serialization;

namespace Delta.Net.Log {

    enum DeltaAction {
        /// <summary>
        /// Commit Provenance Information: https://github.com/delta-io/delta/blob/master/PROTOCOL.md#commit-provenance-information
        /// </summary>
        CommitInfo,

        /// <summary>
        /// Protocol Evolution: https://github.com/delta-io/delta/blob/master/PROTOCOL.md#protocol-evolution
        /// </summary>
        Protocol,

        /// <summary>
        /// Change metadata: https://github.com/delta-io/delta/blob/master/PROTOCOL.md#change-metadata
        /// </summary>
        Metadata
    }

    abstract class Action {
        public DeltaAction DeltaAction { get; set; }

        protected Action(DeltaAction action) {
            DeltaAction = action;
        }

        /// <summary>
        /// Creates an action from the raw json object.
        /// For list of actions see https://github.com/delta-io/delta/blob/master/PROTOCOL.md#actions
        /// </summary>
        public static Action CreateFromJsonObject(string name, JsonElement je) {

            if(name == "commitInfo") {
                return new CommitInfoAction(je.Deserialize<Dictionary<string, object>>()!);
            } else if(name == "protocol") {
                return new ProtocolEvolutionAction(je.Deserialize<ProtocolEvolutionActionPoco>()!);
            } else {
                throw new NotSupportedException($"action '{name}' is not implemented");
            }

            throw new NotSupportedException("action not supported");
        }
    }

    /// <summary>
    /// Additional provenance information about what higher-level operation was being performed as well as who executed it.
    /// Implementations are free to store any valid JSON-formatted data via the commitInfo action.
    /// </summary>
    class CommitInfoAction : Action {
        public CommitInfoAction(Dictionary<string, object> data) : base(DeltaAction.CommitInfo) {
            Data = data;
        }

        public Dictionary<string, object> Data { get; }
    }

    class ProtocolEvolutionActionPoco {
        /// <summary>
        /// The minimum version of the Delta read protocol that a client must implement in order to correctly read this table
        /// </summary>
        [JsonPropertyName("minReaderVersion")]
        public int MinReaderVersion { get; set; }

        /// <summary>
        /// The minimum version of the Delta write protocol that a client must implement in order to correctly write this table
        /// </summary>
        [JsonPropertyName("minWriterVersion")]
        public int MinWriterVersion { get; set; }

        /// <summary>
        /// A collection of features that a client must implement in order to correctly read this table (exist only when minReaderVersion is set to 3)
        /// </summary>
        [JsonPropertyName("readerFeatures")]
        public string[]? ReaderFeatures { get; set; }

        /// <summary>
        /// A collection of features that a client must implement in order to correctly write this table (exist only when minWriterVersion is set to 7)
        /// </summary>
        [JsonPropertyName("writerFeatures")]
        public string[]? WriterFeatures { get; set; }
    }


    class ProtocolEvolutionAction : Action {
        public ProtocolEvolutionAction(ProtocolEvolutionActionPoco data) : base(DeltaAction.Protocol) {
            Data = data;
        }

        public ProtocolEvolutionActionPoco Data { get; }
    }

    class ChangeMetadataPoco {

    }
}
