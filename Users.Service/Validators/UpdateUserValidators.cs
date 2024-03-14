using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class UpdateUserValidators : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidators()
        {
            RuleFor(e => e.Name).MinimumLength(10).MaximumLength(20).WithMessage("Login Error");
            RuleFor(e => e).NotNull();
        }
    }
}
