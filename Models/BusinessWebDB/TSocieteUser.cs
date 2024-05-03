using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_SocieteUser", Schema = "dbo")]
    public partial class TSocieteUser
    {
        [Required]
        public int SocieteID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public AspNetUser AspNetUser { get; set; }

    }
}