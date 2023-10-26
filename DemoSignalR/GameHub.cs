using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace DemoSignalR;

public class GameHub : Hub
{
    public GameHub(IMemoryCache cache)
    {
        Cache = cache;
    }

    private IMemoryCache Cache { get; set; }
    
    public void SetAvailable(string uniqueId)
    {
        Cache.Set(uniqueId + "_available", Context.ConnectionId, options: new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30),
        });
    }

    public async Task RequestGame(string userId, string targetUserId)
    {
        if (Cache.TryGetValue(targetUserId + "_available", out string? connectionId))
        {
            var client = Clients.Client(connectionId!);

            await client.SendAsync("GameRequested", userId);
        }
    }

    public async Task AcceptGame(string userId, string senderUserId)
    {
        if (Cache.TryGetValue(senderUserId + "_available", out string? connectionId))
        {
            var client = Clients.Client(connectionId!);

            await client.SendAsync("GameAccepted", userId);
        }
    }

    public async Task SendMove(string userId, string targetUserId, string move)
    {
        if (Cache.TryGetValue(targetUserId + "_available", out string? connectionId))
        {
            var client = Clients.Client(connectionId!);

            await client.SendAsync("Move", move);
        }
    }

    public async Task EndGame(string userId, string targetUserId)
    {
        if (Cache.TryGetValue(targetUserId + "_available", out string? connectionId))
        {
            var client = Clients.Client(connectionId!);

            await client.SendAsync("EndGame", userId);
        }
    }
}