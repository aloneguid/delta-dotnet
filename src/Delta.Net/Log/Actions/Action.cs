using System.Text.Json;

namespace Delta.Net.Log.Actions {

    public class AddRemoveFilePoco {
        /// <summary>
        /// A relative path to a data file from the root of the table or an absolute path to a file that should be added to the table.
        /// The path is a URI as specified by RFC 2396 URI Generic Syntax, which needs to be decoded to get the data file path.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// A map from partition column to value for this logical file. See also Partition Value Serialization.
        /// </summary>
        public Dictionary<string, string>? PartitionValues { get; set; }

        /// <summary>
        /// The size of this data file in bytes.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// The time this logical file was created, as milliseconds since the epoch.
        /// </summary>
        public long? ModificationTime { get; set; }

        /// <summary>
        /// When false the logical file must already be present in the table or the records in the added file must
        /// be contained in one or more remove actions in the same version.
        /// </summary>
        public bool DataChange { get; set; }

        /// <summary>
        /// Map containing metadata about this logical file.
        /// </summary>
        public Dictionary<string, string>? Tags { get; set; }

        /// <summary>
        /// Default generated Row ID of the first row in the file.
        /// The default generated Row IDs of the other rows in the file can be reconstructed by adding the physical index
        /// of the row within the file to the base Row ID. See also Row IDs.
        /// </summary>
        public long? BaseRowId { get; set; }

        /// <summary>
        /// First commit version in which an add action with the same path was committed to the table.
        /// </summary>
        public long? DefaultRowCommitVersion { get; set; }

        /// <summary>
        /// The name of the clustering implementation.
        /// </summary>
        public string? ClusteringProvider { get; set; }
    }

    public class AddFileAction : Action {
        public AddFileAction(AddRemoveFilePoco data) : base(DeltaAction.AddFile) {
            Data = data;
        }

        private AddRemoveFilePoco Data { get; }
    }

    public class RemoveFileAction : Action {
        public RemoveFileAction(AddRemoveFilePoco data) : base(DeltaAction.RemoveFile) {
            Data = data;
        }

        private AddRemoveFilePoco Data { get; }
    }

    public abstract class Action {
        public DeltaAction DeltaAction { get; set; }

        protected Action(DeltaAction action) {
            DeltaAction = action;
        }

        /// <summary>
        /// Creates an action from the raw json object.
        /// For list of actions see https://github.com/delta-io/delta/blob/master/PROTOCOL.md#actions
        /// </summary>
        public static Action CreateFromJsonObject(string name, JsonElement je) {

            if(name == "commitInfo")
                return new CommitInfoAction(je.Deserialize<Dictionary<string, object?>>()!);
            else if(name == "protocol")
                return new ProtocolEvolutionAction(je.Deserialize<ProtocolEvolutionActionPoco>()!);
            else if(name == "metaData")
                return new ChangeMetadataAction(je.Deserialize<ChangeMetadataPoco>()!);
            else if(name == "add")
                return new AddFileAction(je.Deserialize<AddRemoveFilePoco>()!);
            else if(name == "remove")
                return new RemoveFileAction(je.Deserialize<AddRemoveFilePoco>()!);

            throw new NotSupportedException($"action '{name}' is not supported");
        }
    }
}
