using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ServerAttempt3
{
    // this is a websocket type server, that will service at most 5 requests and then stop
    // this means it always accepts only GET type HTTP requests

    // this supports calls in URL localhost:8085
    // and a get request at URL lcaolhost:8085/Greet with Parameter kvp: {"Name", <your name>}
    class Server
    {
        // for httpServer attempt using websockets
        private Socket httpServer;
        private int serverPort = 8085;
        private Thread listenerThread;

        public Server()
        {
            Console.WriteLine("We will now be starting a TCP\\IP local server");
            Console.WriteLine("At port " + serverPort);
        }

        public void TestLocalServerInDOTNET()
        {
            try
            {
                // creating a socket
                httpServer = new Socket(SocketType.Stream, ProtocolType.Tcp);

                listenerThread = new Thread(new ThreadStart(this.connectionThreadMethod));
                listenerThread.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void connectionThreadMethod()
        {
            try
            {
                // denoting an endpoint
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, serverPort);
                httpServer.Bind(endpoint); // binding to server
                httpServer.Listen(3);   // setting a max of 1 active connection, any more will be rejected
                startListening();   // start listening for requests
                stop(); // stop - does not actually belong here, as in any other server has to live *outside* the thread

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not start connection.\nException details:\n");
                Console.WriteLine(ex.Message);
            }

        }

        // listening for requests via socket
        private void startListening()
        {
            int counter = 0;
            while (true)
            {
                DateTime time = DateTime.Now;

                String data = "";
                byte[] byteArr = new byte[2048];    // TCP\IP requests occur as byteStreams - translatable to byte arrays

                Socket client = httpServer.Accept(); // blocking - waits for a return connection to be made from server
                // this is the 3-way-handshake TCP\IP model in implementation

                while (true)
                {
                    // Receive(arr) loads bytes into arr and returns an integer - the number of bytes loaded
                    int numBytes = client.Receive(byteArr); // the data the server is sending via the connection

                    // accumulate data from byte arr into string format
                    data += Encoding.ASCII.GetString(byteArr, 0, numBytes);

                    // if we read the end of the message, exit reading loop
                    if (data.IndexOf("\r\n") > -1)
                        break;
                }

                // at this point a connection was made client -> server and a message passed fully
                // let's print it - regardless of what it is
                Console.WriteLine(time + " || Client: ");
                Console.WriteLine(data);

                // send back a response - let them know the time
                // creation
                String response = "";
                String functionName = ServerFunctions.getFunction(data);
                switch (functionName)
                {
                    case "default":
                        response = ServerFunctions.defaultResp(time);
                        break;
                    case "greeter":
                        response = ServerFunctions.greeter(data);
                        break;
                }

                // translating to byte arr as we are talking over TCP\IP
                byte[] resData = Encoding.ASCII.GetBytes(response);

                // sending response to client
                client.SendTo(resData, client.RemoteEndPoint);

                // closing connection with client
                client.Close();

                counter++;
                if (counter >= 5)
                    break;
            }
        }

        // stop connection
        private void stop()
        {
            // close socket connection
            httpServer.Close();

            /*// kill thread - for when thread is going on forever as a server should
            listenerThread.Abort();*/
        }
    }
}
