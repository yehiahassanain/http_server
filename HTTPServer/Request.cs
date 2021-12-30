using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            headerLines = new Dictionary<string, string>();
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>

        string[] seperate_Request;
        string[] check_num_blankline;
          public bool ParseRequest()
        {
            //throw new NotImplementedException();
 
            //TODO: parse the receivedRequest using the \r\n delimeter
            //bakr
            string[] parse_receivedrequest =new String[] { "\r\n" };
            requestLines = requestString.Split(parse_receivedrequest,StringSplitOptions.None);
            check_num_blankline = requestString.Split('\r');
            string Request_Line = requestLines[0];
             seperate_Request = Request_Line.Split(' ');
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)
            {
                return false;
            }
            // Parse Request line
            this.relativeURI = seperate_Request[1];
            this.relativeURI=this.relativeURI.Remove(0,1);

            bool validate_parserequestline = ParseRequestLine();
            bool URI = ValidateIsURI(relativeURI);
            // Validate blank line exists
            bool blank_line = ValidateBlankLine();
            bool header = LoadHeaderLines();
            //if (seperate_Request[0].Equals("POST"))
            //{
            //    contentLines[0] = requestLines[3];
            //}
            if ((!validate_parserequestline) || (!URI) || (!blank_line)||(!header))
            {
                return false;
            }
            else
                return true;
            // Load header lines into HeaderLines dictionary
            }
 
        private bool ParseRequestLine()
        {
           
            if (!seperate_Request[0].Equals(RequestMethod.GET.ToString())||
                !seperate_Request[0].Equals(RequestMethod.POST.ToString())||
                !seperate_Request[0].Equals(RequestMethod.HEAD.ToString()))
            {
               return false;
            }
            if (seperate_Request[2].Equals("HTTP/0.9"))
            {
                this.httpVersion = HTTPVersion.HTTP09;
                return true;
            }
            else if (seperate_Request[2].Equals("HTTP/1.0"))
            {
                this.httpVersion = HTTPVersion.HTTP10;
                return true; 
            }
            else if (seperate_Request[2].Equals("HTTP/1.1"))
            {
                this.httpVersion = HTTPVersion.HTTP11;
                return true;
            }
            else
                return false;
 
        }
 
        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            //throw new NotImplementedException();
            for (int i = 1; i < requestLines.Length - 2; i++)
            {
                string[] separte_header = requestLines[i].Split(':');
                headerLines.Add(separte_header[0], separte_header[1]);
            }
            //if (seperate_Request[0].Equals("POST"))
            //{
            //    headerLines.Add("Content-Length", contentLines[0]);
            //}
            return true;
        }


        private bool ValidateBlankLine()
        {
            //return requestLines.Contains(string.Empty);
            if (check_num_blankline.Length>=2)
                return true;
            else
                return false;
        }
 
    }
}