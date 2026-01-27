using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces.Session;

namespace Api.Integration.Tests.Mocks;
public class SessionResolverMock : ISessionResolver
{
    public string? UserId => "6ec0df63-8960-46ec-9163-2de98e04d5e9";

    public string? UserEmail => "testuser@example.com";

    public bool IsAuthenticated => true;

    public string? Token => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJ0ZXN0IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc3VybmFtZSI6InRlc3RTdXJuYW1lIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoidXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAZ21haWwuY29tIiwiY3VzdG9tOnVzZXJfaWQiOiJiNjEzZmZkMC0xNjY3LTRkZmEtYTlkMi1lYjY5ODBkNjM1N2MiLCJleHAiOjE3NjA3Njg1NDYsImlzcyI6ImNtcy1hcGkiLCJhdWQiOiJ5b3VyYXBwX3VzZXJzIn0.kzQWaqv2kYEtXUVfrY2QAod53p38h0LP21r3mTzoQW4";

    public string? FirmId => "1";
}
