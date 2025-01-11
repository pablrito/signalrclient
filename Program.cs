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
            Console.WriteLine($"{user}: {message} {DateTime.Now}");
        });

        // Start the connection
        // Start the connection
        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub. Waiting for messages...");
            
            // Start a task to send messages while listening for incoming ones
            Task.Run(() => SendMessages(connection));

            // Keep the console app running to listen for messages
            Console.WriteLine("Press 'q' to quit.");

            while (true)
            {
                // Exit condition for the entire app
                string input = Console.ReadLine();
                if (input?.ToLower() == "q")
                {
                    Console.WriteLine("Exiting application...");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR Hub: {ex.Message}");
        }
    }

     
     static async Task SendMessages(HubConnection connection)
    {
        try
        {
            while (true)
            {
                Console.WriteLine("\nEnter a message to send to the server (or type 'q' to quit): ");
                string message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    continue;

                if (message.ToLower() == "q")
                {
                    Console.WriteLine("Exiting message loop...");
                    break;
                }

                try
                {
                    // Send the message to the server (this will be broadcast to all connected clients)
                    await connection.SendAsync("SendMessageToAll", "ConsoleClient", message);
                    Console.WriteLine("Message sent to the server.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in message sending loop: {ex.Message}");
        }
    }
}

