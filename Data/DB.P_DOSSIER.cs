using BusinessWeb.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Data;

public partial class DB
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Sage schema on this server has no D_Edi column; model was generated from a newer template.
        modelBuilder.Entity<P_DOSSIER>().Ignore(e => e.D_Edi);
    }
}
