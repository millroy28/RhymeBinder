using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RhymeBinder.ClientApp
{
    public class ViteManifestService
    {
        private readonly Dictionary<string, ViteManifestEntry> _manifest;

        public ViteManifestService(IWebHostEnvironment env)
        {
            var path = Path.Combine(env.WebRootPath, "dist", ".vite", "manifest.json");
            var json = File.ReadAllText(path);
            _manifest = JsonSerializer.Deserialize<Dictionary<string, ViteManifestEntry>>(json);
        }

        public string GetScript(string entryKey) => "/dist/" + _manifest[entryKey].File;

        public IEnumerable<string> GetStyles(string entryKey) =>
            _manifest[entryKey].Css?.Select(css => "/dist/" + css) ?? Enumerable.Empty<string>();
    }

    public class ViteManifestEntry
    {
        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("css")]
        public List<string> Css { get; set; }
    }
}
