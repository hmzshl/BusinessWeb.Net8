using FluentValidation;
using BusinessWeb.Models.DB;
namespace BusinessWeb.Validations
{
    public class ConsommationChantierValidator : AbstractValidator<F_DOCENTETE>
    {
        public ConsommationChantierValidator()
        {
            RuleFor(a => a.DO_Tiers).NotEmpty().WithMessage("Le maitre d'ouvrage est obligatoire!");
            RuleFor(a => a.CA_Num).NotEmpty().WithMessage("L'objet est obligatoire!");
            RuleFor(a => a.DE_No).GreaterThan(0).WithMessage("Le dépot est obligatoire!");
        }

    }
}
