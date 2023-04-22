using MediatR;

namespace Application.Users.Commands.Delete.DeleteUserById
{
    public class DeleteUserByIdCommand : IRequest<Unit>
    {
        public string Id { get; init; }
    }
}