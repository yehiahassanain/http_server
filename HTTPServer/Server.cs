using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        int port;
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            this.port = portNumber;
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint end_Point = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(end_Point);

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            this.serverSocket.Listen(1000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket client_socket = this.serverSocket.Accept();
                Thread server_Thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                server_Thread.Start(client_socket);
            }
        }
        string recived_Request;
        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            int Recive_Timeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] Recive_Request = new byte[1024];
                    int request_length = clientSock.Receive(Recive_Request);
                    // TODO: break the while loop if receivedLen==0
                    if (request_length == 0)
                    {
                        Console.WriteLine("Timout you can not recive any message client:{0}/n", clientSock.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request req_object = new Request(recived_Request);
                    // TODO: Call HandleRequest Method that returns the response
                    Response respose=HandleRequest(req_object);
                    // TODO: Send Response back to client
                    string message = respose.ToString();
                    byte[] send_responce= Encoding.ASCII.GetBytes(message);
                    clientSock.Send(send_responce);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                }
            }

            // TODO: close client socket
            clientSock.Shutdown(SocketShutdown.Both);
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
            throw new NotImplementedException();
            string content;
            try
            {
                /////////////////////////////// StatusCode.OK;
                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    Console.WriteLine("this is bad request");
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                string path= GetRedirectionPagePathIFExist(content);
                //TODO: check for redirect
               
                //TODO: check file exists

                //TODO: read the physical file

                // Create OK response
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Environment.Exit(1);
            }
        }
    }
}
