using System;
using System.Diagnostics;
using System.Net;

namespace TPL
{
    public static class Program
	{
		public static void Main(string[] args)
		{
			var ipAddrs = new[] {IPAddress.Parse("192.168.0.1"), IPAddress.Parse("127.0.0.1")/*, Place your ip addresses here*/};
			var ports = new[] {21, 25, 80, 443, 3389 };
			
			Scanner(new SequentialScanner(), ipAddrs, ports);
			Scanner(new ParallelScanner(), ipAddrs, ports);
			Scanner(new AsyncAwaitScanner(), ipAddrs, ports);
			
		}

		private static void Scanner(IPScanner scanner, IPAddress[] ipAddrs, int[] ports)
		{
			var timer = new Stopwatch();
			timer.Start();
			scanner.Scan(ipAddrs, ports).Wait();
			timer.Stop();
			
			Console.WriteLine($"{scanner.GetType()} time: {timer.ElapsedMilliseconds}ms");
		}
	}
}
