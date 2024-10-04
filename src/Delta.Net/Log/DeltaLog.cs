using System.Text.Json;
using Delta.Net.Log.Actions;
using Stowage;
using Action = Delta.Net.Log.Actions.Action;

namespace Delta.Net.Log {
    public class DeltaLog {
        private readonly IFileStorage _storage;
        private readonly IOPath _location;
        private readonly List<IOEntry> _entries = new List<IOEntry>();
        private readonly List<Action> _actions = new List<Action>();

        public DeltaLog(IFileStorage storage, IOPath location) {
            _storage = storage;
            _location = location;
        }

        public IReadOnlyList<Action> Actions => _actions;

        private async Task ReadActions() {
            _actions.Clear();

            foreach(IOEntry entry in _entries) {
                if(entry.Name.EndsWith(".json")) {
                    string? content = await _storage.ReadText(entry.Path);
                    if(content == null)
                        continue;
                    foreach(string jsonLineRaw in content.Split('\n')) {
                        string jsonLine = jsonLineRaw.Trim();
                        if(string.IsNullOrEmpty(jsonLine))
                            continue;
                        Dictionary<string, object?>? uDoc = JsonSerializer.Deserialize<Dictionary<string, object?>>(jsonLine);
                        if(uDoc == null || uDoc.Count != 1 || uDoc.Values.First() is not JsonElement je)
                            throw new ApplicationException("unparseable action: " + jsonLine);
                        _actions.Add(Action.CreateFromJsonObject(uDoc.Keys.First(), je));
                    }
                }
            }
        }

        public async Task OpenAsync() {
            // Delta files are stored as JSON in a directory at the root of the table named _delta_log, and together with checkpoints make up the log of all changes that have occurred to a table.
            IReadOnlyCollection<IOEntry> entries = await _storage.Ls(_location.Combine("_delta_log/"));
            _entries.Clear();
            _entries.AddRange(entries);

            await ReadActions();
        }
    }
}