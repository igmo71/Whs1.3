using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models
{
    public class EditingCause
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class EditingCauseIn : EditingCause
    {
        public List<ProductIn> ProductsIn { get; set; }
    }
    public class EditingCauseOut : EditingCause
    {
        public List<ProductOut> ProductsOut { get; set; }
    }

}
