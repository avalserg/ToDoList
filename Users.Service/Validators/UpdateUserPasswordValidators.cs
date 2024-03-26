using FluentValidation;
using Users.Application.Dtos;
using Users.Service.Dtos;

namespace Users.Service.Validators
{
    public class UpdateUserPasswordValidators : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordValidators()
        {
             RuleFor(e => e.PasswordHash).MinimumLength(5).MaximumLength(20).NotNull().NotEmpty();
            
        }
    }
}
