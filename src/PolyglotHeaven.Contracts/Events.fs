namespace PolyglotHeaven.Contracts.Events
open PolyglotHeaven.Infrastructure
open System

// Customer events
type CustomerCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id

// Product events
type ProductCreated = {Id: Guid; Name: string; Price: int }
    with interface IEvent with member this.Id with get() = this.Id

// Order commands
type OrderPlaced = { Id: Guid; ProductId: Guid; Quantity: int; Price: int; Discount: int; DiscountedPrice: int } 
    with interface IEvent with member this.Id with get() = this.Id

