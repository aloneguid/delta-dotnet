using System.Text.Json;
using Stowage;

namespace Delta.Net.Log {
    public class DeltaLog {
        private readonly IFileStorage _storage;
        private readonly IOPath _location;
        private List<IOEntry> _entries;
        private readonly List<Action> _actions = new List<Action>();

        public DeltaLog(IFileStorage storage, IOPath location) {
            _storage = storage;
            _location = location;
        }

        private async Task ReadActions() {
            _actions.Clear();

            foreach(IOEntry entry in _entries) {
                if(entry.Name.EndsWith(".json")) {
                    string content = await _storage.ReadText(entry.Path);
                    foreach(string jsonLine in content.Split('\n')) {
                        Dictionary<string, object>? uDoc = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonLine);
                        if(uDoc == null || uDoc.Count != 1 || uDoc.Values.First() is not JsonElement je)
                            throw new ApplicationException("unparseable action: " + jsonLine);
                        _actions.Add(Action.CreateFromJsonObject(uDoc.Keys.First(), je));
                    }
                }
            }
        }

        public async Task OpenAsync() {
            _entries = (await _storage.Ls(_location.Combine("_delta_log/"))).ToList();

            await ReadActions();
        }
    }
}