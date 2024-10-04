using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delta.Net.Log;
using Stowage;

namespace Delta.Net {
    public class DeltaTable {
        private readonly IFileStorage _storage;
        private readonly IOPath _location;

        private DeltaTable(IFileStorage storage, IOPath location) {
            _storage = storage;
            _location = location;
            Log = new DeltaLog(storage, location);
        }

        public DeltaLog Log { get; }

        private async Task OpenAsync() {
            await Log.OpenAsync();
        }

        public static async Task<DeltaTable> OpenAsync(IFileStorage storage, IOPath location) {
            var r = new DeltaTable(storage, location);
            await r.OpenAsync();
            return r;
        }

    }
}
