using BusinessWeb.Models.DB;

namespace BusinessWeb.Services
{
    public interface ISageComptetService
    {
        // ── Add ──────────────────────────────────────────────────────────────
        Task<F_COMPTET> AddComptet(F_COMPTET row);
        Task<List<F_COMPTET>> AddComptets(List<F_COMPTET> rows);
        Task BulkInsertComptets(List<F_COMPTET> rows, bool validateDuplicates = true);

        // ── Read ─────────────────────────────────────────────────────────────
        Task<F_COMPTET> GetComptet(string ctNum);
        Task<F_COMPTET> GetComptetByIdentifiantMaroc(string identifiant);
        Task<List<F_COMPTET>> GetAllClientsMaroc();
        Task<List<F_COMPTET>> GetAllFournisseursMaroc();
        Task<List<F_COMPTET>> GetAllSalariesMaroc();
        Task<List<F_COMPTET>> GetClientsByVilleMaroc(string ville);
        Task<List<F_COMPTET>> GetFournisseursBySecteurMaroc(string secteur);
        Task<List<F_COMPTET>> SearchComptetsMaroc(string searchTerm, int? type = null, string ville = null);

        // ── Update ───────────────────────────────────────────────────────────
        Task<bool> UpdateComptet(F_COMPTET updatedRow);

        // ── Delete ───────────────────────────────────────────────────────────
        Task<bool> DeleteComptet(string ctNum);

        // ── Code generation ──────────────────────────────────────────────────
        Task<string> GetNextClientCodeMaroc();
        Task<string> GetNextFournisseurCodeMaroc();
        Task<string> GetNextSalarieCodeMaroc();

        // ── Quick-add helpers ────────────────────────────────────────────────
        Task<string> QuickAddClientMaroc(string nom, string ville, string ice = null, string cin = null,
            string patente = null, string email = null, string telephone = null, string secteur = null);
        Task<string> QuickAddFournisseurMaroc(string nom, string ville, string ice = null,
            string patente = null, string email = null, string telephone = null, string secteur = null);
        Task<string> QuickAddSalarieMaroc(string nomPrenom, string cin, string ville,
            string email = null, string telephone = null, string poste = null);
        Task<string> QuickAddAdministrationMaroc(string nom, string typeAdmin, string ville, string reference = null);

        // ── Validation ───────────────────────────────────────────────────────
        bool CodeExists(string ctNum);
        bool ValidateIceMaroc(string ice);
        bool ValidateCinMaroc(string cin);

        // ── Statistics ───────────────────────────────────────────────────────
        Task<Dictionary<string, int>> GetStatisticsMaroc();

        // ── Import ───────────────────────────────────────────────────────────
        Task<int> ImportClientsFromCsv(List<Dictionary<string, string>> data);
    }
}
