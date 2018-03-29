using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace hkexnews.Model
{
    public class Records
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Stock_Name { get; set; }

        public string Num { get; set; }

        public string Rate { get; set; }

        public string Date { get; set; }
    }
}
