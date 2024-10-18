using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Token
{
    public interface IJwtToken
    {
        string GenerateJwtToken(string userId, string userName);
    }
}
