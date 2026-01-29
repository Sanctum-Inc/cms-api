using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using ErrorOr;
using MediatR;

namespace Application.CourtCase.Queries.GetCaseNumbers;
public record GetCaseNumbersQuery() : IRequest<ErrorOr<IEnumerable<string>?>>;
