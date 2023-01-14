using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TPL
{
    public class ParallelScanner: IPScanner
    {
        public Task Scan(IPAddress[] ipAdrrs, int[] ports)  
        {
            return Task.WhenAll(
                ipAdrrs 
                    .AsParallel()  
                    .SelectMany(ip => PingAddress(ip, ports).Result)
            );  
        }
        
        private Task<IEnumerable<Task>> PingAddress(IPAddress ipAddr, int[] ports, int timeout = 3000)  
        {  
            using var ping = new Ping();  
            Console.WriteLine($"Pinging {ipAddr}");  
             
            return ping 
                .SendPingAsync(ipAddr, timeout)  
                .ContinueWith(p =>  
                {  
                    Console.WriteLine($"Pinged {ipAddr}: {p.Result.Status}");
                    return p.Result.Status == IPStatus.Success ? ports.Select(port => CheckPort(ipAddr, port)) : Enumerable.Empty<Task>();
                });  
        }  
        
        private static async Task CheckPort(IPAddress ipAddr, int port, int timeout = 3000)  
        {  
            using var tcpClient = new TcpClient();
            
            Console.WriteLine($"Checking {ipAddr}:{port}");  
            var portStatus = await tcpClient.ConnectAsync(ipAddr, port, timeout);  
            Console.WriteLine($"Checked {ipAddr}:{port} - {portStatus}");  
        }
    }
}