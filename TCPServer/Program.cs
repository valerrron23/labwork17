﻿using System.Net;
using System.Net.Sockets;
using TCPServer;


internal class Program
{
    private static async Task Main(string[] args)
    {

        ServerObject server = new ServerObject();// создаем сервер
        while (true)
        {
            await server.ListenAsync(); // запускаем сервер
        }

    }

}