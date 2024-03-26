using FluentValidation;
using Users.Application.Dtos;
using Users.Service.Dtos;

namespace Users.Service.Validators
{
    public class CreateUserDtoValidators:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidators()
        {
            RuleFor(e => e.Login).MinimumLength(5).MaximumLength(20).NotEmpty();
            RuleFor(e => e.Password).MinimumLength(5).MaximumLength(20).NotNull().NotEmpty();
        }
    }
}
