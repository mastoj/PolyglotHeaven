namespace PolyglotHeaven.Contracts.Commands
open PolyglotHeaven.Infrastructure
open System

type OrderItem = {ProductId: Guid; Quantity: int}

// Customer commands
type CreateCustomer = {Id: Guid; Name: string } with interface ICommand

// Product commands
type CreateProduct = {Id: Guid; Name: string; Price: int } with interface ICommand

// Order commands
type PlaceOrder = { Id: Guid; CustomerId: Guid; Items: OrderItem list } with interface ICommand
