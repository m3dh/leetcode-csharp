using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace LeetCode.Csharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LeetCode.Csharp.Common;
    using LeetCode.Csharp.Solutions;
    using LeetCode.Csharp.Solutions2;
    using Newtonsoft.Json;

    internal class Program
    {
        public static void Main()
        {
            JObject j = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(@"C:\Users\liamy\Desktop\Worklog\Json.txt"));
            JToken[] ja = j["DeviceProblem"].ToArray();
            foreach (var token in ja.OrderBy(t=>t["@Name"]))
            {
                StringBuilder ssb = new StringBuilder();

                string svv;
                var actions = token["Actions"];
                if (actions.Type == JTokenType.Array)
                {
                    svv = $"{string.Join(", ", actions.ToArray().Select(a => $"\"{a["@Name"]}\""))}";
                }
                else
                {
                    svv = $"\"{actions["Action"]["@Name"]}\"";
                }

                ssb.Append($"{{ \"{token["@Name"]}\", new List<string>{{ {svv} }} }},");

                Console.WriteLine(ssb);
            }
        }
    }
}