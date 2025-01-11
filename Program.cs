using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
      string hubUrl = "https://signalrserver220250111105952.azurewebsites.net/command";
       var connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();

        // Define the method to handle receiving messages
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Console.WriteLine($"{user} : {message} : {DateTime.Now}");
        });

        

        // Start the connection
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub. Waiting for messages...");

            await connection.InvokeAsync("Echo", "imei", "connecting");
                
            // Keep the console app running to listen for messages
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR Hub: {ex.Message}");
        }
    }
}

