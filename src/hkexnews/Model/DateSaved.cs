using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace hkexnews.Model
{
    public class DateSaved
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }
    }
}
