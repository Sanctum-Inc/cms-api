using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.User.Responses;
public record LoginResponse(
    string Token,
    DateTime Expiration,
    string RefreshToken);
