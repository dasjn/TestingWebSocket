using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Uri uri = new Uri("ws://1xx.1xx.1.1xx:4456"); // Replace with the WebSocket server URI
        using (ClientWebSocket clientWebSocket = new ClientWebSocket())
        {
            try
            {
                await clientWebSocket.ConnectAsync(uri, CancellationToken.None);
                Console.WriteLine("Connected to WebSocket server.");

                // Send test message
                string message = "Hello from C# client!";
                await SendWebSocketMessage(clientWebSocket, message);

                // Receive response
                string response = await ReceiveWebSocketMessage(clientWebSocket);
                Console.WriteLine($"Received message: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static async Task SendWebSocketMessage(ClientWebSocket clientWebSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    static async Task<string> ReceiveWebSocketMessage(ClientWebSocket clientWebSocket)
    {
        byte[] buffer = new byte[1024];
        StringBuilder stringBuilder = new StringBuilder();

        while (clientWebSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

            if (result.EndOfMessage)
            {
                break;
            }
        }

        return stringBuilder.ToString();
    }
}
