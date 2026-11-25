using System;

namespace AcManager.Internal {
    /// <summary>Same shape as the original <c>ChangelogEntry</c>, which only stores these values.</summary>
    public class ChangelogEntry {
        public ChangelogEntry(string version, string changes, DateTime date) {
            Version = version;
            Changes = changes;
            Date = date;
        }

        public string Version { get; }
        public string Changes { get; }
        public DateTime Date { get; }
    }
}
