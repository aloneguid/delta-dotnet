namespace Delta.Net.Log.Actions {
    public class RemoveFileAction : FileAction {
        public RemoveFileAction(AddRemoveFilePoco data) : base(data, false) {
        }
    }
}
