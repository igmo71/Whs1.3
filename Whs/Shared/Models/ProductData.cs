using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Whs.Shared.Models
{
    public class ProductDataIn : Product
    {
        public Guid Id { get; set; }
        [ForeignKey("EditingCauseId")]
        public EditingCauseIn EditingCause { get; set; }
    }

    public class ProductDataOut : Product
    {
        public Guid Id { get; set; }
        [ForeignKey("EditingCauseId")]
        public EditingCauseOut EditingCause { get; set; }
    }
}
