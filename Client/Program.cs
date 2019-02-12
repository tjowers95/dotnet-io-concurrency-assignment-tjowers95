using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using Core;

namespace Client
{
    class Program
    {
        public static void Main(string[] args)
        {
            var port = 3000;
            var address = "127.0.0.1";
            
            var requestSerializer = new XmlSerializer(typeof(QuoteRequest));
            var responseSerializer = new XmlSerializer(typeof(QuoteResponse));

            // Example QuoteRequest containing symbols and fields client wishes to fetch data about
            var request = new QuoteRequest
            {
                Fields = new List<QuoteField> { QuoteField.High, QuoteField.Low, QuoteField.Close },
                Symbols = new List<string> { "AAPL", "TSLA", "TWTR" },
                Interval = 3
            };
            
            try
            {
                // Initialize connection to server
                var client = new TcpClient(address, port);
                byte[] fromServer = new byte[8192];

                Console.WriteLine("Client Bound to Socket");
                using (var stream = client.GetStream())
                {
                    // Serialize request to server
                    requestSerializer.Serialize(stream, request);
                    // Temporary hacky fix for data preventing read blocking
                    client.Client.Shutdown(SocketShutdown.Send);

                loop:
                    // write network stream to file stream
                    FileStream Disk = new FileStream(@"C:\Users\ftd-01\assignments\dotnet-io-concurrency-assignment-tjowers95\Client\data.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    Disk.SetLength(0);
                    Disk.Seek(0, SeekOrigin.Begin);

                    // prevent null/uninitialized bytes from being read
                    do
                    {
                        Thread.Sleep(20);
                    } while (!stream.DataAvailable);

                    // read a fixed size so there's no infinite hang
                    stream.Read(fromServer, 0, 8192);

                    // find the size of the actual data by searching for the injected EOF character (unicode 26)
                    int i = 0;
                    while ((int)fromServer[i++] != 26);

                    // allocate a new buffer without uninitialized / null bytes 
                    byte[] toDisk = new byte[--i];
                    int j = 0;
                    while (j < i)
                        toDisk[j] = fromServer[j++];

                    Disk.Write(toDisk, 0, i);

                    Disk.Seek(0, SeekOrigin.Begin);
                    Disk.CopyTo(Console.OpenStandardOutput());
                    Disk.Close();

                    if (request.Interval > 0)
                        goto loop;  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}