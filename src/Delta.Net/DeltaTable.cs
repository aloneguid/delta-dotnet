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
        private readonly DeltaLog _log;

        private DeltaTable(IFileStorage storage, IOPath location) {
            _storage = storage;
            _location = location;
            _log = new DeltaLog(storage, location);
        }

        private async Task OpenAsync() {
            await _log.OpenAsync();
        }

        public static async Task<DeltaTable> OpenAsync(IFileStorage storage, IOPath location) {
            var r = new DeltaTable(storage, location);
            await r.OpenAsync();
            return r;
        }

    }
}
