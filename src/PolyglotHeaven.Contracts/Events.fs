namespace PolyglotHeaven.Contracts.Events

open PolyglotHeaven.Contracts.Types
open PolyglotHeaven.Infrastructure
open System


// Customer events
type CustomerCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id

// Product events
type ProductCreated = {Id: Guid; Name: string; Price: int }
    with interface IEvent with member this.Id with get() = this.Id

// Order commands
type OrderPlaced = { Id: Guid; CustomerId: Guid; Items: OrderItem list } 
    with interface IEvent with member this.Id with get() = this.Id

