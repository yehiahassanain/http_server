using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString());
            string status = GetStatusLine(code);
            if (redirectoinPath!=null)
            {
                headerLines.Add(redirectoinPath);
                // TODO: Create the request string
                responseString = status + "\r\n" + "content-type: " + headerLines[0] + "\r\n" + "Content-Length: " 
               + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + "Location: " + headerLines[3]+"\r\n"+"\r\n"+content;
            }
            else
            {
                responseString = status + "\r\n" + "content-type: " + headerLines[0] + "\r\n" + "Content-Length: "
              + headerLines[1] + "\r\n" + "Date: " + headerLines[2] + "\r\n" + "\r\n" + content;
            }

        }
        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            //.....
            string statusLine = string.Empty;
            if (code == StatusCode.OK)
                statusLine = Configuration.ServerHTTPVersion+" "+ (int)code + " "+"ok";
            else if (code == StatusCode.Redirect)
                statusLine = Configuration.ServerHTTPVersion + " " + (int)code + " " + "redirect";
            else if (code == StatusCode.BadRequest)
                statusLine = Configuration.ServerHTTPVersion + " " + (int)code + " " + "bad";
            else if (code == StatusCode.NotFound)
                statusLine = Configuration.ServerHTTPVersion + " " + (int)code + " " + "not_found";
            else
                statusLine = Configuration.ServerHTTPVersion + " " + (int)code + " " + "internal_server_error";

            return statusLine;
        }
    }
}
