using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alterview.Core.Models
{
    public class SportEvent
    {
        [Key]
        public int EventId { get; set; }
        public int SportId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Team1Price { get; set; }
        public decimal DrawPrice { get; set; }
        public decimal Team2Price { get; set; }
    }
}
