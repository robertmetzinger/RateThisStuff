using RateThisStuff_Server.Services;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RateThisStuff_ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceUrl = new Uri("http://localhost:8733/Design_Time_Addresses/RateThisStuff_Server/Services/ClientService/");
            // Create the ServiceHost.
            using (var host = new ServiceHost(typeof(ClientService), serviceUrl))
            {
                var smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);
                host.AddServiceEndpoint(typeof(IClientService), new WSHttpBinding(), serviceUrl);
                host.Open();
                Console.WriteLine("The service is ready at {0}" + serviceUrl);
                Console.WriteLine("Press enter to stop the service");
                Console.Read();
                host.Close();
            }
        }
    }
}
