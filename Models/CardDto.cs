using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStudyTool.Models
{
    public class CardDto
    {
        public int CardId { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }        
    }
}
