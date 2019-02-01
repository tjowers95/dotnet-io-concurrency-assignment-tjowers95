# .NET Assignment - IO and Concurrency

# Assignment Overview

For this assignment, you will be expanding and enhancing a client/server **Stock Ticker** application. Clients will send the [Ticker Symbol](https://en.wikipedia.org/wiki/Ticker_symbol)s that they wish to get quotes on, as well as fields that they may be interested in. Example fields include 'high', 'low', 'latest price', etc. 

### What you are given

*This is a description of what the skeleton **currently** does and does not reflect the end result.*

This assignment comes with a skeleton which contains working client/server applications. Below is a description of what each application does and how they communicate with each other.

When the client application is started, it creates in memory a hard-coded `QuoteRequest` object. This contains the ticker symbols and fields that the client will be requesting. It creates a TcpClient connection and serializes the `QuoteRequest` over a NetworkStream obtained from the client connection. It then deserializes an XML response given from the server containing a QuoteString property which contains the requested information.

When the server is started, it waits for an incoming `TcpClient` connection via `listener.AcceptTcpClient()`. Once the server recieves a connection, it creates a NetworkStream which is initialized to `client.GetStream()`. It then deserializes the XML request into the `QuoteRequest` object. Once it has the `QuoteRequest` object, it calls a provided `Stocks.Api` method. This class contains an async static method that will return a `Task<List<IIEXTradingResponse_STOCK_QUOTE_Content>>` from an [open-source stock market API](https://iextrading.com/developer/docs/) using a [3rd party .NET wrapper library](https://www.nuget.org/packages/IEXTradingApi/). This library wraps the API in easy-to-use C# classes and methods. Once the server awaits the list of quotes, it uses the provided `Stocks.Utils` class to format the quotes to a string which it then sends back to the client via an `XmlSerializer` and the client stream.

The server and client both close after a single request is made.

---

# Assignment Requirements

You are tasked with building on top an existing client/server application and configuring it to support concurrent requests. The DTO's (Data transfer objects) contained in the `Core` project will need to be expanded to allow the client to provide an 'interval' (1-10 seconds) which the server will use in order to continue sending real-time quotes back to the client application. The DTO classess will need to be modified to match the example specification below. The client will then save the XML response to an XML file on the client machine.

You are free to change or expand upon the hard-coded values on the client side to get more example data. The `Additional Notes` section contains more information regarding these values and contains an extra-credit challenge to make the application more flexible.

The server should accept client requests and create start a new `Task` to manage each client's connection and provide the client with quotes at their requested interval. The server will use the `interval` to determine how frequently to fetch quotes from the provided API which it will immediately give to the client. The task servicing the client with responses should remain running until either the client closes their application or the server is forcibly stopped.

The server should remain open to new requests until the server is forcibly stopped.

*Due to latency when communicating with Stock API, the timing for the interval does not have have to be precise*.

The response from the server should be changed from a formatted string to a marshalled XML file. The required format of this response is shown below.

#### Example XML response

*The stock market is constantly changing, so there may be differences in values*

```xml
<quotes>
    <quote>
        <symbol>TWTR</symbol>
        <fields>
            <high>29.79</high>
            <latestPrice>29.38</latestPrice>
        </fields>
    </quote>
    <quote>
        <symbol>FB</symbol>
        <fields>
            <high>155.54</high>
            <latestPrice>155.07</latestPrice>
        </fields>
    </quote>
</quotes>
```

Once the server sends the XML response to the client, the client application should save the XML file to the client's machine. The location of where this file is stored is up to you. Each time the client gets a response, it should overwrite the existing file.

## Additional Information

* The client will be passing a single set of fields to view. They should not be able to view different fields for different ticker symbols.

* The client's request data is hard-coded in the given skeleton. For 'extra-credit', you can implement functionality that allows this information to be read in from an XML file (easy) OR read in from the command-line (more difficult). If read in from an XML file, the format should not be the same format as the `QuoteRequest`.

* The required fields have already been included in the project skeleton inside of `QuoteField.java` - a DTO [enum](https://docs.oracle.com/javase/tutorial/java/javaOO/enum.html).
