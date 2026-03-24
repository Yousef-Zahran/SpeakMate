using SpeakMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);

    }
}
