using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace hkexnews.Model
{
    public class Names
    {
        [Key]
        public int Id { get; set; }

        public string EN { get; set; }

        public string CN { get; set; }

        public string Code { get; set; }

    }
}
