using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessWeb.Models.BusinessWebDB
{
    [Table("T_Authorize", Schema = "dbo")]
    public partial class TAuthorize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int Societe { get; set; }

        public int SelectedAPP { get; set; }

        [MaxLength(450)]
        public string UserID { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public bool Visible { get; set; }
    }
}