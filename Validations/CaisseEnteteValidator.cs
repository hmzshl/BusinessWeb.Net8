using FluentValidation;
using BusinessWeb.Models.DB;

namespace BusinessWeb.Validations
{
    public class CaisseEnteteValidator : AbstractValidator<API_T_CaisseEntete>
    {
        public CaisseEnteteValidator()
        {
            RuleFor(a => a.Caisse).GreaterThan(0).WithMessage("La caisse est obligatoire!");
        }
    }
}
