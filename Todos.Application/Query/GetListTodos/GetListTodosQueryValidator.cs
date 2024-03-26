using FluentValidation;

namespace Todos.Application.Query.GetListTodos
{
    public class GetListTodosQueryValidator : AbstractValidator<GetListTodosQuery>
    {
        public GetListTodosQueryValidator()
        {
            RuleFor(e => e.Limit).GreaterThan(0).When(e => e.Limit.HasValue);
            RuleFor(e => e.Offset).GreaterThan(0).When(e => e.Offset.HasValue);
            RuleFor(e => e.NameFreeText).MaximumLength(100);
        }
    }
}
