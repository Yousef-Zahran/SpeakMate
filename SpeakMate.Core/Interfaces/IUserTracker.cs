using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Core.Repositories
{
   
    public interface IUserTracker
    {
        Task AddAsync(string connectionId, string? username);
        Task RemoveAsync(string connectionId);
        Task<IEnumerable<string>> GetOnlineUsersAsync();
        bool IsOnline(string username);
    }
}
