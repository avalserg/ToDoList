﻿using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Common.Service.Exceptions;
using Serilog;
using Todos.Service.Dto;


namespace Todos.Service
{
    public class TodosService : ITodosService
    {
       
        private readonly IBaseRepository<Common.Domain.Todos> _todosRepository;
        private readonly IBaseRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;
    

        public TodosService(IBaseRepository<Common.Domain.Todos> todosRepository, IBaseRepository<ApplicationUser> userRepository,IMapper mapper)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            
        }
       
        public IReadOnlyCollection<Common.Domain.Todos> GetAllToDo(int? offset, string? labelFreeText, int? limit)
        {
            limit ??= 10;
            var list = _todosRepository.GetList(
                offset,
                limit,
                labelFreeText == null ? null : t => t.Label.Contains(labelFreeText),
                t => t.Id);

            return list;
        }    
        public async Task<IReadOnlyCollection<Common.Domain.Todos>> GetAllToDoAsync(int? offset, string? labelFreeText, int? limit, bool? descending,CancellationToken cancellationToken)
        {
            limit ??= 10;
            var list = await _todosRepository.GetListAsync(
                offset,
                limit,
                labelFreeText == null ? null : t => t.Label.Contains(labelFreeText),
                t => t.Id,
                descending,
                cancellationToken);

            return list;
        }

        public  Common.Domain.Todos? GetToDoById(int id)
        {
            return  _todosRepository.GetSingleOrDefault(t=>t.Id==id);
        }
        public async Task<Common.Domain.Todos?> GetToDoByIdAsync(int id,CancellationToken cancellationToken)
        {
            return await _todosRepository.GetSingleOrDefaultAsync(t=>t.Id==id,cancellationToken);
        }

        public Common.Domain.Todos CreateToDo(CreateTodoDto createToDo)
        {
            var owner = _userRepository.GetSingleOrDefault(u=>u.Id==createToDo.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {createToDo.OwnerId} does not exist");
                throw new BadRequestException();
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Common.Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            Log.Information($"Todo with id={createToDo} was created");
           
            return _todosRepository.Add(todoEntity);
        } 
        public async Task<Common.Domain.Todos?> CreateToDoAsync(CreateTodoDto createToDo, CancellationToken cancellationToken = default)
        {
            var owner = await _userRepository.GetSingleOrDefaultAsync(u=>u.Id==createToDo.OwnerId,cancellationToken);
            if (owner is null)
            {
                Log.Error($"User {createToDo.OwnerId} does not exist");
                throw new BadRequestException();
            }
            var todoEntity = _mapper.Map<CreateTodoDto,Common.Domain.Todos>(createToDo);
            
            todoEntity.CreatedDate = DateTime.UtcNow;
            // always false after created
            todoEntity.IsDone = false;

            Log.Information($"Todo with id={createToDo} was created");
           
            return await _todosRepository.AddAsync(todoEntity,cancellationToken);
        }
        public Common.Domain.Todos? UpdateToDo(UpdateToDoDto updateToDo)
        {
            var todoEntity = _todosRepository.GetSingleOrDefault(t=>t.Id==updateToDo.Id);
            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                throw new NotFoundException();
            }

            var owner = _userRepository.GetSingleOrDefault(u => u.Id == todoEntity.OwnerId);
            if (owner is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new BadRequestException();
            }

            _mapper.Map(updateToDo, todoEntity);
            
           
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return _todosRepository.Update(todoEntity);
        }  
        public async Task<Common.Domain.Todos?> UpdateToDoAsync(UpdateToDoDto updateToDo, CancellationToken cancellationToken = default)
        {
            var todoEntity =await _todosRepository.GetSingleOrDefaultAsync(t=>t.Id==updateToDo.Id, cancellationToken);
            if (todoEntity == null)
            {
                Log.Error($"Todo {updateToDo.Id} does not exist");
                throw new NotFoundException();
            }

            var owner = await _userRepository.GetSingleOrDefaultAsync(u => u.Id == todoEntity.OwnerId,cancellationToken);
            if (owner is null)
            {
                Log.Error($"User {todoEntity.OwnerId} does not exist");
                throw new BadRequestException();
            }

            _mapper.Map(updateToDo, todoEntity);
            
           
            todoEntity.UpdateDate = DateTime.UtcNow;

            Log.Information($"Todo with id={todoEntity.Id} was updated");

            return await _todosRepository.UpdateAsync(todoEntity, cancellationToken);
        }

        public int Count(string? labelFreeText)
        {
            return _todosRepository.Count(labelFreeText == null
                ? null
                : t => t.Label.Contains(labelFreeText));
        } 
        public async Task<int> CountAsync(string? labelFreeText, CancellationToken cancellationToken = default)
        {
            return await _todosRepository.CountAsync(labelFreeText == null
                ? null
                : t => t.Label.Contains(labelFreeText), cancellationToken);
        }

        public bool RemoveToDo(int id)
        {
            var todoToRemove = _todosRepository.GetSingleOrDefault(t => t.Id == id);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                throw new NotFoundException();
            }
            
            Log.Information($"Todo with id={id} was deleted");
            
            return _todosRepository.Delete(todoToRemove);
        }
        public async Task<bool> RemoveToDoAsync(int id, CancellationToken cancellationToken = default)
        {
            var todoToRemove =await _todosRepository.GetSingleOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (todoToRemove == null)
            {
                Log.Error($"Todo with id={id} does not exist");
                throw new NotFoundException();
            }
            
            Log.Information($"Todo with id={id} was deleted");
            
            return await _todosRepository.DeleteAsync(todoToRemove, cancellationToken);
        }

    }
}
