using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models
{
    public class Response1c
    {
        public string Ошибка { get; set; }
    }

    public class Response1cIn : Response1c
    {
        public WhsOrderIn Результат { get; set; }
    }
    public class Response1cOut : Response1c
    {
        public WhsOrderOut Результат { get; set; }
    }
}
