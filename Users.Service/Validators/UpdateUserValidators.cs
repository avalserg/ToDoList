using FluentValidation;
using Users.Service.Dto;
using Users.Service.Dtos;

namespace Users.Service.Validators
{
    public class UpdateUserValidators : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidators()
        {
            RuleFor(e => e.Login).MinimumLength(10).MaximumLength(20).WithMessage("Login Error");
            RuleFor(e => e).NotNull();
        }
    }
}
