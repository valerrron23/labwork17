﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace TCPClient
{
    internal class Client
    {
        string host = "127.0.0.1";
        int port = 8888;
        string userName = "basename";


        public void handlerAsync()
        {

            using TcpClient client = new TcpClient();
            Console.Write("Введите свое имя: ");
            userName = Console.ReadLine();
            Console.WriteLine($"Добро пожаловать, {userName}");
            StreamReader? Reader = null;
            StreamWriter? Writer = null;

            try
            {
                client.Connect(host, port); //подключение клиента
                Reader = new StreamReader(client.GetStream());
                Writer = new StreamWriter(client.GetStream());
                if (Writer is null || Reader is null) return;
                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(Reader));
                // запускаем ввод сообщений
                SendMessageAsync(Writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Writer?.Close();
            Reader?.Close();
        }

        // отправка сообщений
        async Task SendMessageAsync(StreamWriter writer)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                string? message = Console.ReadLine(); string poka = "пока";
                string ew = $"Сообщение {userName} удалено";
                if (message.Contains(poka))
                {
                    Print(message);//вывод сообщения
                    Console.WriteLine($"Пока, {userName}");
                }

                await writer.WriteLineAsync(message);
                await writer.FlushAsync();

                //вывод сообщения
                // если пустой ответ, ничего не выводим на консоль
            }
        }
        // получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    string? message = await reader.ReadLineAsync();

                    if (string.IsNullOrEmpty(message)) continue;



                }
                catch
                {
                    break;
                }
            }

        }
        // чтобы полученное сообщение не накладывалось на ввод нового сообщения
        void Print(string message)
        {
            if (OperatingSystem.IsWindows())    // если ОС Windows
            {
                var position = Console.GetCursorPosition(); // получаем текущую позицию курсора
                int left = position.Left;   // смещение в символах относительно левого края
                int top = position.Top;     // смещение в строках относительно верха
                                            // копируем ранее введенные символы в строке на следующую строку
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                // устанавливаем курсор в начало текущей строки
                Console.SetCursorPosition(0, top);
                // в текущей строке выводит полученное сообщение
                Console.WriteLine(message);
                // переносим курсор на следующую строку
                // и пользователь продолжает ввод уже на следующей строке
                Console.SetCursorPosition(left, top + 1);
            }

        }
    }
}
