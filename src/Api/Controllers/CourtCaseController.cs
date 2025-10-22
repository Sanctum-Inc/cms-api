using Application.Common.Models;
using Application.CourtCase.Commands.Add;
using Application.CourtCase.Commands.Delete;
using Application.CourtCase.Commands.Update;
using Application.CourtCase.Queries.Get;
using Application.CourtCase.Queries.GetById;
using Contracts.CourtCases.Requests;
using Contracts.CourtCases.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Handles CRUD operations for court cases.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourtCaseController : CrudControllerBase<
    CourtCaseResult,                // TResult — Domain result type
    CourtCasesResponse,             // TResponse — DTO response
    GetCommand,                     // TGetCommand
    GetByIdCommand,                 // TGetByIdCommand
    AddCourtCaseRequest,            // TAddRequest
    AddCommand,                     // TAddCommand
    UpdateCourtCaseRequest,         // TUpdateRequest
    UpdateCommand,                  // TUpdateCommand
    DeleteCommand                   // TDeleteCommand
>
{
    public CourtCaseController(IMapper mapper, ISender sender)
        : base(mapper, sender) { }
}
