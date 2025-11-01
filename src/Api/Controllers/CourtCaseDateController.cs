
using Application.Common.Models;
using Application.CourtCaseDates.Commands.Add;
using Application.CourtCaseDates.Commands.Delete;
using Application.CourtCaseDates.Commands.Update;
using Application.CourtCaseDates.Queries.Get;
using Application.CourtCaseDates.Queries.GetById;
using Contracts.CourtCaseDates.Requests;
using Contracts.CourtCaseDates.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourtCaseDateController : CrudControllerBase<
        CourtCaseDateResult,                   // TResult
        CourtCaseDatesResponse,                 // TResponse
        GetCommand,                       // TGetCommand
        GetByIdCommand,                   // TGetByIdCommand
        AddCourtCaseDateRequest,               // TAddRequest
        AddCommand,                       // TAddCommand
        UpdateCourtCaseDateRequest,            // TUpdateRequest
        UpdateCommand,                    // TUpdateCommand
        DeleteCommand                     // TDeleteCommand
    >
{
    public CourtCaseDateController(IMapper mapper, ISender sender)
        : base(mapper, sender)
    {
    }
}
