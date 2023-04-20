using Application._Common.Models;
using Application.Users.Commands.Create;
using Application.Users.Queries.GetAll;
using Application.Users.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.MyAPI.Models.Users;

namespace Web.MyAPI.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsersController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        /// <summary>
        /// Create a user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser(CreateUserRequest request, CancellationToken cancellationToken)
        {
            Guid newUserId = await _mediator.Send(new CreateUserCommand() { Name = request.Name }, cancellationToken);

            return Ok(newUserId);
        }

        /// <summary>
        /// Get users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaginatedList<UserModel>>>> GetUsers(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            PaginatedList<User> queryResult = await _mediator.Send(new GetUsersQuery() { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);

            return Ok(new PaginatedList<UserModel>(_mapper.Map<IEnumerable<UserModel>>(queryResult.Items), pageNumber, pageSize));
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(string id, CancellationToken cancellationToken)
        {
            User user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);

            return user != null ? Ok(_mapper.Map<UserModel>(user)) : NotFound();
        }
    }
}