namespace Delta.Net.Log.Actions {
    class ProtocolEvolutionAction : Action {
        public ProtocolEvolutionAction(ProtocolEvolutionActionPoco data) : base(DeltaAction.Protocol) {
            Data = data;
        }

        public ProtocolEvolutionActionPoco Data { get; }
    }
}
