using Microsoft.AspNetCore.SignalR;

public class AudioHub : Hub
{
    public Task JoinRoom(string roomId) =>
        Groups.AddToGroupAsync(Context.ConnectionId, roomId);

    public Task LeaveRoom(string roomId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

    public Task SendAudioChunk(byte[] data, string format, string senderId, string? roomId = null)
    {
        if (!string.IsNullOrEmpty(roomId))
            return Clients.OthersInGroup(roomId).SendAsync("ReceiveAudioChunk", data, format, senderId);

        return Clients.Others.SendAsync("ReceiveAudioChunk", data, format, senderId);
    }
}
