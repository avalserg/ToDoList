using FluentValidation;

namespace Users.Application.Command.CreateUser
{
    public class RemoveUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public RemoveUserCommandValidator()
        {
            RuleFor(e => e.Login).MinimumLength(5).MaximumLength(20).NotEmpty();
            RuleFor(e => e.Password).MinimumLength(5).MaximumLength(20).NotNull().NotEmpty();
        }
    }
}
