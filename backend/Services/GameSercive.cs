using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    // Metoda, którą klient wywołuje, aby dołączyć do pokoju gry
    public async Task JoinGameRoom(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
    }

    // Metoda, którą klient wywołuje, aby opuścić pokój
    public async Task LeaveGameRoom(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"game-{gameId}");
    }
}