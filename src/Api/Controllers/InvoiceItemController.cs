using Application.Common.Models;
using Application.InvoiceItem.Commands.Add;
using Application.InvoiceItem.Commands.Delete;
using Application.InvoiceItem.Commands.Update;
using Application.InvoiceItem.Queries.Get;
using Application.InvoiceItem.Queries.GetById;
using Contracts.InvoiceItem.Requests;
using Contracts.InvoiceItem.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InvoiceItemController : CrudControllerBase<
    InvoiceItemResult,
    InvoiceItemResponse,
    GetCommand,
    GetByIdCommand,
    AddInvoiceItemRequest,
    AddCommand,
    UpdateInvoiceItemRequest,
    UpdateCommand,
    DeleteCommand>
{
    public InvoiceItemController(IMapper mapper, ISender sender) : base(mapper, sender)
    {
    }
}
