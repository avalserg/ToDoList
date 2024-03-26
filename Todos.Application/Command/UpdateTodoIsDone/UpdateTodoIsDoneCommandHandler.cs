﻿using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Serilog;
using Todos.Application.Dtos;

namespace Todos.Application.Command.UpdateTodoIsDone
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateTodoIsDoneCommand, UpdateTodoIsDoneDto>
    {
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IBaseRepository<ApplicationUserRole> _userRoles;
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly TodosMemoryCache _todosMemoryCache;
        private readonly IMapper _mapper;

        public UpdateUserPasswordCommandHandler(
            IBaseRepository<ApplicationUser> userRepository,
            IBaseRepository<ApplicationUserRole> userRoles,
            IBaseRepository<Common.Domain.Todos> todosRepository,
            ICurrentUserService currentUserService,
            TodosMemoryCache todosMemoryCache,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userRoles = userRoles;
            _todosRepository = todosRepository;
            _currentUserService = currentUserService;
            _todosMemoryCache = todosMemoryCache;
            _mapper = mapper;
        }
        public async Task<UpdateTodoIsDoneDto> Handle(UpdateTodoIsDoneCommand request, CancellationToken cancellationToken)
        {
            var todoEntity = await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            
            if (todoEntity == null)
            {
                Log.Error($"Todo {request.Id} does not exist");
                throw new NotFoundException();
            }

            var ownerTodo = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == todoEntity.OwnerId, cancellationToken);
            if (ownerTodo is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new BadRequestException();
            }

            var currentLoggedInUserId = _currentUserService.CurrentUserId;
            var currentLoggedInUserRoles = _currentUserService.CurrentUserRoles;

            if (currentLoggedInUserRoles.Any(r => r != "Admin") && todoEntity.OwnerId.ToString() != currentLoggedInUserId)
            {
                Log.Error($"Todos {request.Id} cannot be updated by User {ownerTodo.Login}");
                throw new BadRequestException();
            }
           

            todoEntity.UpdateDate = DateTime.UtcNow;
            todoEntity.IsDone=request.IsDone;
            _todosMemoryCache.Cache.Clear();
            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return _mapper.Map <UpdateTodoIsDoneDto>( await _todosRepository.UpdateAsync(todoEntity, cancellationToken));
        }
    }
}
