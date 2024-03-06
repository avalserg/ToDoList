using FluentValidation;
using Users.Service.Dto;

namespace Users.Service.Validators
{
    public class CreateUserDtoValidators:AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidators()
        {
            RuleFor(e => e.Name).MinimumLength(2).MaximumLength(20).WithMessage("Name Error");
            RuleFor(e => e).NotNull();
        }
    }
}
