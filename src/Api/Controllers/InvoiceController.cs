using System.ComponentModel.DataAnnotations;
using Application.Common.Models;
using Application.Invoice.Commands.Add;
using Application.Invoice.Commands.Delete;
using Application.Invoice.Commands.Update;
using Application.Invoice.Queries.Get;
using Application.Invoice.Queries.GetById;
using Contracts.Invoice.Requests;
using Contracts.Invoice.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles operations related to invoices.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController
    : CrudControllerBase<
        InvoiceResult,        // TResult
        InvoiceResponse,      // TResponse
        GetCommand,           // TGetCommand
        GetByIdCommand,       // TGetByIdCommand
        AddInvoiceRequest,    // TAddRequest
        AddCommand,           // TAddCommand
        UpdateCommand,        // TUpdateRequest
        UpdateCommand,        // TUpdateCommand
        DeleteCommand         // TDeleteCommand
    >
{
    public InvoiceController(IMapper mapper, ISender sender)
        : base(mapper, sender)
    {
    }

    // Optionally override any method if you need custom behavior.
    // For example, you could override Create or Update if special handling is required.
    // Otherwise, base implementations handle the CRUD operations.
}
