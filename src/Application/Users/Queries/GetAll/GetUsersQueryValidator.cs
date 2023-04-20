using FluentValidation;

namespace Application.Users.Queries.GetAll
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(m => m.PageNumber).GreaterThan(0);
            RuleFor(m => m.PageSize).GreaterThan(0);
        }
    }
}