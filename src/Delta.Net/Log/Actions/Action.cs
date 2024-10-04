using System.Text.Json;

namespace Delta.Net.Log.Actions {

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
            else
                throw new NotSupportedException($"action '{name}' is not implemented");

            throw new NotSupportedException("action not supported");
        }
    }
}
