using SpeakMate.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SpeakMate.API.Hubs
{

    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IUserTracker _tracker;

        public PresenceHub(IUserTracker tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            var username = Context.User?.Identity?.Name;
            await _tracker.AddAsync(Context.ConnectionId, username);

            var onlineUsers = await _tracker.GetOnlineUsersAsync();
            await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);

            await Clients.Others.SendAsync("UserJoined",
                Context.User?.FindFirstValue(ClaimTypes.Email));
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var username = Context.User?.Identity?.Name;
            await _tracker.RemoveAsync(Context.ConnectionId);
            await Clients.Others.SendAsync("UserLeft", username);

            await base.OnDisconnectedAsync(ex);
        }
    }

}