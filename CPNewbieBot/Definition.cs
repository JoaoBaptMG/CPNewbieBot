using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPNewbieBot
{
    static class StringUtils
    {
        internal static string RemoveAfter(this string str, string pat)
        {
            var idx = str.IndexOf(pat);
            if (idx > 0) return str.Substring(0, idx);
            return str;
        }
    }

    internal struct Definition
    {
        public Regex Pattern;
        public string Title;
        public string Description;
        public string[] Urls;

        public static async Task<Definition[]> ParseFile(string fileName)
        {
            return ParseString(await File.ReadAllTextAsync(fileName));
        }

        public static Definition[] ParseString(string str)
        {
            var lines = str.Split('\n', '\r').Select(s => s.Trim().RemoveAfter("##")).Where(s => s.Length > 0).ToArray();

            var defs = new List<Definition>();

            foreach (var line in lines)
            {
                if (line.StartsWith("definition")) defs.Add(new Definition());
                else if (defs.Count == 0)
                    Console.WriteLine("WARNING: trying to set parameters of no definition. Skipping...");
                else
                {
                    var def = defs[defs.Count - 1];

                    if (line.StartsWith("pattern:"))
                    {
                        if (def.Pattern == null)
                            def.Pattern = new Regex(line.Substring("pattern:".Length).Trim(),
                                RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        else Console.WriteLine("WARNING: overwriting a definition's pattern. Skipping...");
                    }
                    else if (line.StartsWith("title:"))
                    {
                        if (def.Title == null) def.Title = line.Substring("title:".Length).Trim();
                        else Console.WriteLine("WARNING: overwriting a definition's title. Skipping...");
                    }
                    else if (line.StartsWith("description:"))
                    {
                        if (def.Description == null) def.Description = line.Substring("description:".Length).Trim();
                        else Console.WriteLine("WARNING: overwriting a definition's description. Skipping...");
                    }
                    else if (line.StartsWith("url:"))
                    {
                        var url = line.Substring("url:".Length).Trim();
                        if (def.Urls == null) def.Urls = new string[] { url };
                        else
                        {
                            Array.Resize(ref def.Urls, def.Urls.Length + 1);
                            def.Urls[def.Urls.Length - 1] = url;
                        }
                    }

                    defs[defs.Count - 1] = def;
                }
            }

            return defs.ToArray();
        }
    }
}
