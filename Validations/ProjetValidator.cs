using FluentValidation;
using BusinessWeb.Models.DB;
namespace BusinessWeb.Validations
{
    public class ProjetValidator : AbstractValidator<API_T_Projet>
    {
        public ProjetValidator()
        {
            RuleFor(a => a.CT_Num).NotEmpty().WithMessage("Le maitre d'ouvrage est obligatoire!");
            RuleFor(a => a.CA_Num).NotEmpty().WithMessage("L'affaire est obligatoire!");
            RuleFor(a => a.Objet).NotEmpty().WithMessage("L'objet est obligatoire!");
            RuleFor(a => a.Site).GreaterThan(0).WithMessage("La zone est obligatoire!");
            RuleFor(a => a.Ville).GreaterThan(0).WithMessage("La ville est obligatoire!");
            //RuleFor(a => a.Utilisateur).NotEmpty().WithMessage("Le responsable est obligatoire!");
        }

    }
}
