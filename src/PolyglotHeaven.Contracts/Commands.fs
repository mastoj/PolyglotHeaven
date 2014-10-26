namespace PolyglotHeaven.Contracts.Commands
open PolyglotHeaven.Infrastructure
open System

// Customer commands
type CreateCustomer = {Id: Guid; Name: string } with interface ICommand

// Product commands
type CreateProduct = {Id: Guid; Name: string; Price: int } with interface ICommand

// Order commands
type PlaceOrder = { Id: Guid; ProductId: Guid; Quantity: int } with interface ICommand
