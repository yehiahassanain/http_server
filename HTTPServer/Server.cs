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
        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint end_Point = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(end_Point);
        }

        public void StartServer()
        {
            Console.WriteLine("start listening......");
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

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout = 0;
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
                    string recived_Request = Encoding.ASCII.GetString(Recive_Request);
                    Request req_object = new Request(recived_Request);
                    // TODO: Call HandleRequest Method that returns the response
                    Response respose = HandleRequest(req_object);
                    string display_response = respose.ResponseString;
                    Console.WriteLine(display_response);
                    // TODO: Send Response back to client
                    string message = respose.ResponseString;
                    byte[] send_responce = Encoding.ASCII.GetBytes(message);
                    clientSock.Send(send_responce);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            try
            {
                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    Response check_response_bad = new Response(StatusCode.BadRequest, "text/html", content, null);
                    return check_response_bad;
                }
                //TODO: map the relativeURI in request to get the physical path of the resource
                string content_uri = request.relativeURI;
                //string path= GetRedirectionPagePathIFExist(request.relativeURI);
                //TODO: check for redirect
                string redirec_path = GetRedirectionPagePathIFExist(content_uri);
                if (redirec_path!=string.Empty)
                {
                    string physical_path = Configuration.RootPath + "\\" + redirec_path;
                    if (!File.Exists(physical_path))
                    {
                        content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                        Response check_response_notfound = new Response(StatusCode.NotFound, "text/html", content, string.Empty);
                        return check_response_notfound;
                    }
                    else
                    {
                        content = LoadDefaultPage(GetRedirectionPagePathIFExist(content_uri));
                        Response check_response_found = new Response(StatusCode.Redirect, "text/html",
                           content, redirec_path);
                        return check_response_found;
                    }
                }
                //TODO: check file exists
                string file = Configuration.RootPath + "\\"+request.relativeURI;
                if (!File.Exists(file))
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    Response check_response_notfound = new Response(StatusCode.NotFound, "text/html", content, null);
                    return check_response_notfound;
                }
                //TODO: read the physical file
                // Create OK response
                else
                {
                    content = LoadDefaultPage(content_uri);
                    Response response_ok = new Response(StatusCode.OK, "text/html", content, null);
                    return response_ok;
                }

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                Response response_internal = new Response(StatusCode.InternalServerError, "text/html", content, null);
                return response_internal;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            foreach (KeyValuePair<string, string> redirection in Configuration.RedirectionRules)
            {
                if (redirection.Key.Equals(relativePath))
                {
                    string value = redirection.Value;
                    return value;
                }
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string       
            if (!File.Exists(filePath))
            {
                FileNotFoundException ex = new FileNotFoundException();
                Logger.LogException(ex);
                return string.Empty;
            }
            // else read file and return its content
            else
            {
                StreamReader read_file = new StreamReader(filePath);
                string read_content = read_file.ReadToEnd();
                read_file.Close();
                return read_content;

            }
        }

            private void LoadRedirectionRules(string filePath)
            {
                try
                {
                    // TODO: using the filepath paramter read the redirection rules from file
                    Configuration.RedirectionRules = new Dictionary<string, string>();
                    string reader = filePath;
                    string[] read_redirection = File.ReadAllLines(reader);
                    // then fill Configuration.RedirectionRules dictionary
                    foreach (string waleed in read_redirection)
                    {
                        string[] content_file = waleed.Split(',');
                        Configuration.RedirectionRules.Add(content_file[0], content_file[1]);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                    Environment.Exit(1);
                }
            }
        }
    }