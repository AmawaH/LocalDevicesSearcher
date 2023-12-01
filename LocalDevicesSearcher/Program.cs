
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace LocalDevicesSearcher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var runner = new Builder().Build();
            runner.Run();
        }
    }
}

