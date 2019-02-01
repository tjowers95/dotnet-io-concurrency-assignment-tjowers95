using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
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
                Fields = new List<QuoteField> {QuoteField.High, QuoteField.Low, QuoteField.Close},
                Symbols = new List<string> {"AAPL", "TSLA", "TWTR"}
            };
            
            try
            {
                // Initialize connection to server
                var client = new TcpClient(address, port);
            
                using (var stream = client.GetStream())
                {
                    // Serialize request to server
                    requestSerializer.Serialize(stream, request);
                    
                    // Temporary hacky fix for data preventing read blocking
                    client.Client.Shutdown(SocketShutdown.Send);
                    
                    // Receive QuoteResponse from server
                    QuoteResponse response = (QuoteResponse) responseSerializer.Deserialize(stream);
                    Console.WriteLine(response.QuoteString);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}