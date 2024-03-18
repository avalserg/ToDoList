using FluentValidation;
using Todos.Service.Dto;

namespace Todos.Service.Validators
{
    public class CreateToDoDtoValidators:AbstractValidator<CreateTodoDto>
    {
        public CreateToDoDtoValidators()
        {
            //RuleFor(e => e.OwnerId).GreaterThan(0).WithMessage($"Id must have value from 1");
            RuleFor(e => e.Label).MinimumLength(10).MaximumLength(200).Must(e=>e.StartsWith("Todo")).WithMessage("Label start with \"Todo\" word");
            RuleFor(e => e).NotNull();
        }
    }
}
