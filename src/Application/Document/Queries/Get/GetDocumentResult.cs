using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Document.Queries.Get;
public record GetDocumentResult(
    Guid Id,
    string Name,
    string FileName,
    long Size,
    DateTime CreatedAt,
    Guid CaseId
);
