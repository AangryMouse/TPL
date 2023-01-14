using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TPL
{
    public class AsyncAwaitScanner : IPScanner
    {
        public Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            return Task.WhenAll(ipAddrs.Select(ip => PingAddress(ip, ports)));
        }  
  
        private static async Task PingAddress(IPAddress ipAddr, int[] ports, int timeout = 3000)  
        {  
            Console.WriteLine($"Pinging {ipAddr}");

            var pingReply = await new Ping().SendPingAsync(ipAddr, timeout);
            
            Console.WriteLine($"Pinged {ipAddr}: {pingReply.Status}");
            
            var tasks = new List<Task>();
            if (pingReply.Status == IPStatus.Success)
            {
                tasks.AddRange(ports.Select(port => CheckPort(ipAddr, port, timeout)));
            }
            await Task.WhenAll(tasks);
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