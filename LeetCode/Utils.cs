namespace LeetCode.Csharp
{
    using System;
    using Newtonsoft.Json;

    public static class Utils
    {
        public static void JsonPrint(this object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
    }
}
