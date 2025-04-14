using FluentValidation;
using HardwareStore.Models;

namespace HardwareStore.Validations
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(client => client.Name)
                .Length(4, 50).WithMessage("El numero de caracteres debe estar entre 4 y 50")
                .NotNull().WithMessage("El nombre es requerido");
        }
    }
}