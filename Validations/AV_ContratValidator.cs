using FluentValidation;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Validations
{
    public class AV_ContratValidator : AbstractValidator<API_T_AgenceContrat>
    {
        public AV_ContratValidator()
        {
            RuleFor(a => a.Tiers).GreaterThan(0).WithMessage("Le Tiers est obligatoire!");
            RuleFor(a => a.Taux).GreaterThan(0).WithMessage("Le cours de change est obligatoire!");
        }
    }
}
