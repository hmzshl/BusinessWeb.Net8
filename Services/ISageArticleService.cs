using BusinessWeb.Models.DB;
using BusinessWeb.Models.Perso;

namespace BusinessWeb.Services
{
    public interface ISageArticleService
    {
        // ── Add ──────────────────────────────────────────────────────────────
        Task<OperationResult<F_ARTICLE>> AddArticle(F_ARTICLE row);
        Task<List<F_ARTICLE>> AddArticles(List<F_ARTICLE> rows);
        Task BulkInsertArticles(List<F_ARTICLE> rows, bool validateDuplicates = true);

        // ── Read ─────────────────────────────────────────────────────────────
        Task<F_ARTICLE> GetArticle(string arRef);
        Task<F_ARTICLE> GetArticleByCodeBarre(string codeBarre);
        Task<List<F_ARTICLE>> GetAllArticles();
        Task<List<F_ARTICLE>> GetArticlesByFamille(string codeFamille);
        Task<List<F_ARTICLE>> GetArticlesByType(int type);
        Task<List<F_ARTICLE>> SearchArticles(string searchTerm, string famille = null, int? type = null);
        Task<List<string>> GetAllFamilles();
        Task<Dictionary<string, int>> GetArticlesCountByFamille();

        // ── Update ───────────────────────────────────────────────────────────
        Task<bool> UpdateArticle(F_ARTICLE updatedRow);

        // ── Delete ───────────────────────────────────────────────────────────
        Task<bool> DeleteArticle(string arRef);

        // ── Code generation ──────────────────────────────────────────────────
        Task<string> GetNextArticleReference(string famille, string prefix = "ART");

        // ── Validation ───────────────────────────────────────────────────────
        bool ReferenceExists(string arRef);
    }
}
