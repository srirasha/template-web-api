using Application._Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.Delete.DeleteUserById
{
    public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserByIdCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.Id, out Guid userId))
            {
                await _context.DeleteAsync(new User() { Id = userId }, cancellationToken);
            }

            return Unit.Value;
        }
    }
}