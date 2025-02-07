// See https://aka.ms/new-console-template for more information


Console.WriteLine("Hello, World!");

// var contactClient = new InvoiceClient();
// await contactClient.CreateContactAsync();

// var serverStreamingClient = new ServerStreamingClient();
// await serverStreamingClient.GetRandomNumbers();

var clientStreamingClient = new ClientStreamingClient();
await clientStreamingClient.SendRandomNumbers();