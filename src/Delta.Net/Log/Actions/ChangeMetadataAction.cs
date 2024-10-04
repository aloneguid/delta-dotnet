namespace Delta.Net.Log.Actions {
    public class ChangeMetadataAction : Action {
        public ChangeMetadataAction(ChangeMetadataPoco data) : base(DeltaAction.Metadata) {
            Data = data;
        }

        public ChangeMetadataPoco Data { get; }
    }
}
