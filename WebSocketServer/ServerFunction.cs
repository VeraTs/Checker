using System;
using System.Collections.Generic;
using System.Text;

namespace ServerAttempt3
{
    static class ServerFunctions
    {
        static internal String getFunction(String request)
        {
            String functionName = "";

            functionName = request.Substring(5);
            bool hasParams = false;
            try
            {
                functionName = functionName.Substring(0, functionName.IndexOf(' '));
                hasParams = functionName.IndexOf('?') >= 0 ? true : false;
                if (hasParams)
                {
                    functionName = functionName.Substring(0, functionName.IndexOf('?'));
                    functionName = functionName.Trim();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: \n");
                Console.WriteLine(ex.Message);
                functionName = "";
            }

            if (!hasParams)
            {
                functionName = "default";
            }
            else
            {
                switch (functionName)
                {
                    case "":
                        functionName = "default";
                        break;
                    case null:
                        functionName = "default";
                        break;
                    case "Greet":
                        functionName = "greeter";
                        break;
                }
            }

            return functionName;
        }

        static internal String greeter(String request)
        {
            String response = "";

            String temp = request.Substring(request.IndexOf("?") + 1);
            String param = temp.Substring(0, temp.IndexOf("="));
            String name = temp.Substring(temp.IndexOf("=") + 1);
            name = name.Substring(0, name.IndexOf(" "));

            String resHeader = "HTTP/1.1 200 Everything is Fine\n" +
        "Server: ServerAttempt2\n" +
        "Content-Type: text/plain\n\n";
            String resBody = "Hello, your " + param + " must be " + name + "!";
            response = resHeader + resBody;

            return response;
        }

        static internal String defaultResp(DateTime time)
        {
            String response = "";
            String resHeader = "HTTP/1.1 200 Everything is Fine\n" +
                    "Server: ServerAttempt2\n" +
                    "Content-Type: text/plain\n\n";
            String resBody = "Some Plain Text" + time;
            response = resHeader + resBody;

            return response;
        }

    }
}
