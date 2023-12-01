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

