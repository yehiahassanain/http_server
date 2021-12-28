using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        static public StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //FileStream logger_file = new FileStream(@"F:\level3 subject\computer network\project\Template[2021-2022]\HTTPServer\bin\Debug", FileMode.OpenOrCreate);
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            //StreamWriter write_on_file = new StreamWriter(logger_file);
            sr.WriteLine("DateTime is :" + DateTime.Now.ToString());
            sr.WriteLine("Error message is :" + ex.Message);
            sr.Flush();
        }
    }
}
