using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Core;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var requestSerializer = new XmlSerializer(typeof(QuoteRequest));
                var responseSerializer = new XmlSerializer(typeof(QuoteResponse));
                var port = 3000; 
                var address = IPAddress.Parse("127.0.0.1");
                var listener = new TcpListener(address, port);
                listener.Start();
         
                bool Accepting = true;
                while (Accepting)
                {
                    TcpClient Client = listener.AcceptTcpClient();
#pragma warning disable CS4014
                   Task.Run(async() =>
                   {
                        using (NetworkStream ClientStream = Client.GetStream())
                        {
                            QuoteRequest APIRequest = (QuoteRequest)requestSerializer.Deserialize(ClientStream);  
                           
                            int interval = APIRequest.Interval;
                            SendData:

                           var APIResponse = Stocks.Api.FetchQuotes(APIRequest).Result;
                           QuoteResponse serveResponse = new QuoteResponse();
                           int index = 0;
                           foreach (var i in APIResponse)
                           {
                               serveResponse.Quote.Add(new Quote());
                               serveResponse.Quote[index].symbol = i.Symbol;

                               foreach (var j in APIRequest.Fields)
                               {
                                    if (j == QuoteField.Open)
                                    {
                                        serveResponse.Quote[index].open = i.Open;
                                    }
                                    else if (j == QuoteField.Close)
                                    {
                                        serveResponse.Quote[index].close = i.Close;
                                    }
                                    else if (j == QuoteField.High)
                                    {
                                         serveResponse.Quote[index].high = i.High;
                                    }   
                                    else if (j == QuoteField.Low)
                                    {
                                        serveResponse.Quote[index].low = i.Low;
                                    }
                               }
                               index++;
                           }
                            // send data
                            responseSerializer.Serialize(ClientStream, serveResponse);

                            // send EOF character so we can find the size of the data on the other side
                            ClientStream.Write(new byte[] {26}, 0, 1);

                            if (interval != 0)
                            {
                                await Task.Delay(1000*interval);
                                goto SendData;
                            }
                        }
                   });
#pragma warning restore CS4014
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