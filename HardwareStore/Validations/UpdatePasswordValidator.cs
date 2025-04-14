using FluentValidation;
using HardwareStore.Models;

namespace HardwareStore.Validations
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePassword>
    {
        public UpdatePasswordValidator() 
        {
            RuleFor(employee => employee.Password)
            .NotNull().WithMessage("La contraseña es requerida");

            RuleFor(employee => employee.RePassword)
                .NotNull().WithMessage("Por favor Confirme la contraseña")
                .Equal(employee => employee.Password).WithMessage("Las contraseñas no coinciden");
        }
    }
}
