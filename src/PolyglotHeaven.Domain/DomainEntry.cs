﻿using System;
using System.Collections.Generic;
using System.Linq;
using PolyglotHeaven.Contracts.Commands;
using PolyglotHeaven.Domain.CommandHandlers;
using PolyglotHeaven.Infrastructure;

namespace PolyglotHeaven.Domain
{
    public class DomainEntry
    {
        private readonly CommandDispatcher _commandDispatcher;

        public DomainEntry(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe = null, IEnumerable<Action<object>> postExecutionPipe = null)
        {
            preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            postExecutionPipe = CreatePostExecutionPipe(postExecutionPipe);
            _commandDispatcher = CreateCommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandDispatcher.ExecuteCommand(command);
        }

        private CommandDispatcher CreateCommandDispatcher(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            var commandDispatcher = new CommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);

            var customerCommandHandler = new CustomerCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler(customerCommandHandler);

            var productCommandHandler = new ProductCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler(productCommandHandler);

            var orderCommandHandler = new OrderCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler(orderCommandHandler);

            return commandDispatcher;
        }

        private IEnumerable<Action<object>> CreatePostExecutionPipe(IEnumerable<Action<object>> postExecutionPipe)
        {
            if (postExecutionPipe != null)
            {
                foreach (var action in postExecutionPipe)
                {
                    yield return action;
                }
            }
        }
    }
}
