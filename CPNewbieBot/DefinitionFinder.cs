using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPNewbieBot
{
    internal class DefinitionFinder
    {
        static Definition[] definitions;

        public string Message { get; private set; }

        public DefinitionFinder()
        {
            Message = "Not parsed yet...";
        }

        public async Task Parse(string msg, string responder)
        {
            if (definitions == null) definitions = await Definition.ParseFile("definitions.txt");

            var foundDefs = new Dictionary<int,string[]>();
            int index = 0;

            foreach (var def in definitions)
            {
                var matches = def.Pattern.Matches(msg);
                if (matches.Any()) foundDefs.Add(index, matches.Select(m => m.Value).ToArray());
                index++;
            }

            var msgDefs = new List<string>();
            foreach (var kvp in foundDefs)
            {
                var def = definitions[kvp.Key];
                msgDefs.Add($"_{string.Join(',', kvp.Value)}_ = *{def.Title}*\n" +
                    $"{def.Description}\n" +
                    $"*Useful links:*\n{string.Join('\n', def.Urls)}");
            }

            if (msgDefs.Count > 0)
                Message = $"Hello, {responder}, here are the definitions found on the scanned message:\n\n" + string.Join("\n\n", msgDefs);
            else Message = $"I'm sorry, {responder}, I have not found any definitions in the scanned message.";

            Message += "\n\nFor more definitions you'd like to know, use the command /search.";
        }
    }
}