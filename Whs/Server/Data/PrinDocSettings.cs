using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Whs.Server.Data
{
    public class PrinDocSettings
    {
        public const string PrinDoc = "PrinDoc";

        public Dictionary<string, string> Values { get; set; }
    }

    public class Doc
    {
        public string TemplateName { get; set; }
        public string Endpoint { get; set; }
    }
}
