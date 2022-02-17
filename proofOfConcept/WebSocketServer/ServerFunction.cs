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
            try
            {
                functionName = getServiceName(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: \n");
                Console.WriteLine(ex.Message);
                functionName = "";
            }

            switch (functionName)
            {
                case null:
                    functionName = "default";
                    break;
                case "":
                    functionName = "default";
                    break;
                case "Greet":
                    functionName = "greeter";
                    break;
            }

            return functionName;
        }

        static internal String greeter(String request)
        {
            String response = "";
            bool isError = false;
            Dictionary<String, String> paramsAndValues = extractParams(request);

            String param = "";
            String name = "";
            String resHeader;
            String resBody;

            if (paramsAndValues.Count > 0)
            {
                if (paramsAndValues.ContainsKey("Name"))
                {
                    param = "Name";
                    name = paramsAndValues[param];
                }
                else
                    isError = true;
            }
            else
                isError = true;

            if (!isError)
            {
                resHeader = "HTTP/1.1 200 Everything is Fine\n" +
        "Server: ServerAttempt2\n" +
        "Content-Type: text/plain\n\n";
                resBody = "Hello, your " + param + " must be " + name + "!\n" + resHeader;
            }
            else
            {
                // error occured   
                resHeader = "HTTP/1.1 403 An error occured\n" +
        "Server: ServerAttempt2\n" +
        "Content-Type: text/plain\n\n";
                resBody = "You must enter a parameter 'Name' for this service\n" + resHeader;
            }

            
            response = resHeader + resBody;

            return response;
        }

        static internal String defaultResp(DateTime time)
        {
            String response = "";
            String resHeader = "HTTP/1.1 200 Everything is Fine\n" +
                    "Server: ServerAttempt2\n" +
                    "Content-Type: text/plain\n\n";
            String resBody = "Some Plain Text - and here's the time: " + time;
            response = resHeader + resBody;

            return response;
        }

        static private Dictionary<String, String> extractParams(String request)
        {
            Dictionary<String, String> paramsAndValues = new Dictionary<string, string>();

            if (hasParams(request))
            {
                String paramsOnly = request.Substring(request.IndexOf('?') + 1);
                paramsOnly = paramsOnly.Substring(0, paramsOnly.IndexOf(' '));
                string[] keyAndValues = paramsOnly.Split('&');
                foreach(String kvp in keyAndValues)
                {
                    paramsAndValues.Add(kvp.Substring(0, kvp.IndexOf('=')),
                        kvp.Substring(kvp.IndexOf('=') + 1));
                }
            }

            return paramsAndValues;
        }

        static private bool hasParams(String request)
        {
            bool res = false;
            String temp;
            temp = request.Substring(request.IndexOf('/'));
            temp = temp.Substring(0, temp.IndexOf(' '));
            res = temp.IndexOf('?') >= 0 ? true : false;
            if (res)
                res = res && temp.Substring(temp.IndexOf('?')+1).Length > 0;

            return res;
        }

        static private String getServiceName(String request)
        {
            String name = "";
            String temp = request.Substring(request.IndexOf('/'));
            temp = temp.Substring(0, temp.IndexOf(' '));
            if(temp.IndexOf('?') > 0)
            {
                name = temp.Substring(1, temp.IndexOf('?') - 1);
            }


            
            return name;
        }

    }
}
