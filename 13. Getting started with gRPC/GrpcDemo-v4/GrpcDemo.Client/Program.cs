﻿// See https://aka.ms/new-console-template for more information
using GrpcDemo.Client;

Console.WriteLine("Hello, World!");

// var contactClient = new InvoiceClient();
// await contactClient.CreateContactAsync();

var serverStreamingClient = new ServerStreamingClient();
await serverStreamingClient.GetRandomNumbers();