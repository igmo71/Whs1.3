using System.Collections.Generic;

namespace Whs.Client.Pages
{
    public class PrinDocSettings
    {
        public const string PrinDoc = "PrinDoc";

        public string BaseAddress { get; set; }
        public Dictionary<string, Doc> Values { get; set; }
    }

    public class Doc
    {
        public string Template { get; set; }
        public string Service { get; set; }
        public string Endpoint { get; set; }
        public string Page { get; set; }
    }
}
