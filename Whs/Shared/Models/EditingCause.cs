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
        public List<ProductDataIn> ProductsData { get; set; }
    }
    public class EditingCauseOut : EditingCause
    {
        public List<ProductDataOut> ProductsData { get; set; }
    }

}
