using FluentValidation;
using BusinessWeb.Models.DB;
namespace BusinessWeb.Validations
{
    public class PointageProjetValidator : AbstractValidator<API_T_PointageProjet>
    {
        public PointageProjetValidator() 
        {
            RuleFor(a => a.CT_Num).NotEmpty().WithMessage("Le maitre d'ouvrage est obligatoire!");
            RuleFor(a => a.Projet).GreaterThan(0).WithMessage("Le projet est obligatoire!");
            RuleFor(a => a.Responsable).GreaterThan(0).WithMessage("Le responsable est obligatoire!");
        }
        
    }
}
