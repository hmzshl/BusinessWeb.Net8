using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_License", Schema = "dbo")]
    public partial class TLicense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string Activation { get; set; }
    }
}