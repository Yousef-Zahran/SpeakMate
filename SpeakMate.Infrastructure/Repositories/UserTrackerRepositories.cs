using SpeakMate.Core.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SpeakMate.Infrastructure.Repositories
{
    public class UserTrackerRepositories : IUserTracker
    {
        private readonly ConcurrentDictionary<string, string> _connections = new();
        public Task AddAsync(string connectionId, string? username)
        {
            if (!string.IsNullOrEmpty(username))
                _connections.TryAdd(connectionId, username);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetOnlineUsersAsync()
        {
            var users = _connections.Values.Distinct();
            return Task.FromResult(users);
        }

        public bool IsOnline(string username)
        {
           return _connections.Values.Contains(username);
        }

        public Task RemoveAsync(string connectionId)
        {
            _connections.TryRemove(connectionId, out _);
            return Task.CompletedTask;
        }
    }
}
