using FluentValidation;

namespace Todos.Application.Command.CreateTodo
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(e => e.Label).MinimumLength(5).MaximumLength(1000).NotEmpty();
          
        }
    }
}
