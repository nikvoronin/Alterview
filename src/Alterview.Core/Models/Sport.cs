using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alterview.Core.Models
{
    public class Sport
    {
        [Key]
        public int SportId { get; set; }
        public string Name { get; set; }
    }
}
