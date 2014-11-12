using System;
using System.Collections.Generic;
using System.Linq;

namespace PolyglotHeaven.Infrastructure
{
    public class CommandDispatcher
    {
        private IDomainRepository _domainRepository;
        private readonly IEnumerable<Action<object>> _postExecutionPipe;
        private readonly IEnumerable<Action<ICommand>> _preExecutionPipe;
        private ActionRouter _actionRouter;

        public CommandDispatcher(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            _domainRepository = domainRepository;
            _postExecutionPipe = postExecutionPipe;
            _preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            _actionRouter = new ActionRouter();
        }

        public void RegisterHandler<TCommand>(IHandle<TCommand> handler) where TCommand : ICommand
        {
            _actionRouter.Add<TCommand>(command => ExecuteCommand(command, handler.Handle));
        }

        public void ExecuteCommand<TCommand>(TCommand command)
        {
            if (!_actionRouter.Execute<TCommand>(command))
            {
                var commandType = typeof (TCommand);
                throw new ApplicationException("Missing handler for " + commandType.Name);
            }
        }

        public void ExecuteCommand<TCommand>(TCommand command, Func<TCommand, IAggregate> handler) where TCommand : ICommand
        {
            RunPreExecutionPipe(command);
            var aggregate = handler(command);
            var savedEvents = _domainRepository.Save(aggregate);
            RunPostExecutionPipe(savedEvents);
        }

        private void RunPostExecutionPipe(IEnumerable<object> savedEvents)
        {
            foreach (var savedEvent in savedEvents)
            {
                foreach (var action in _postExecutionPipe)
                {
                    action(savedEvent);
                }
            }
        }

        private void RunPreExecutionPipe(ICommand command)
        {
            foreach (var action in _preExecutionPipe)
            {
                action(command);
            }
        }
    }
}