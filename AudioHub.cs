using Microsoft.AspNetCore.SignalR;

public class AudioHub : Hub
{
    public Task JoinRoom(string roomId) =>
        Groups.AddToGroupAsync(Context.ConnectionId, roomId);

    public Task LeaveRoom(string roomId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

    public async Task SendAudioChunk(byte[] data, string format, string senderId, string? roomId = null)
    {
        try
        {
            var len = data?.Length ?? 0;
            Console.WriteLine($"[AudioHub] Recibido chunk de {len} bytes, fmt={format}, sender={senderId}");

            if (len == 0) return;

            if (!string.IsNullOrEmpty(roomId))
                await Clients.OthersInGroup(roomId).SendAsync("ReceiveAudioChunk", data, format, senderId);
            else
                await Clients.Others.SendAsync("ReceiveAudioChunk", data, format, senderId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AudioHub] ERROR reenviando: {ex}");
            throw; // deja que el cliente vea el error
        }
    }
}
