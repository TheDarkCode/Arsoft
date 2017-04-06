﻿
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;


namespace ArsoftTestServer
{


    // https://docs.ar-soft.de/arsoft.tools.net/DNS%20Server.html
    public class SimpleServer
    {


        private static async Task OnQueryReceived(object sender, QueryReceivedEventArgs e)
        {
            DnsMessage query = e.Query as DnsMessage;

            if (query == null)
                return;

            DnsMessage response = query.CreateResponseInstance();


            // check for valid query
            if ((query.Questions.Count == 1)
                && (query.Questions[0].RecordType == RecordType.Txt)
                && (query.Questions[0].Name.Equals(DomainName.Parse("example.com"))))
            {
                response.ReturnCode = ReturnCode.NoError;
                response.AnswerRecords.Add(new TxtRecord(DomainName.Parse("example.com"), 3600, "Hello world"));
            }
            else
            {
                response.ReturnCode = ReturnCode.ServerFailure;
            }

            // set the response
            e.Response = response;
        } // End Function OnQueryReceived 


        public static void Test()
        {
            
            using (DnsServer server = new DnsServer(10, 10))
            {
                server.QueryReceived += OnQueryReceived;

                server.Start();

                Console.WriteLine("Press any key to stop server");
                System.Console.ReadKey();
            } // End Using server 

        } // End Sub Test 


    } // End Class SimpleServer 


} // End Namespace ArsoftTestServer 
