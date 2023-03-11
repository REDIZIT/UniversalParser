namespace Bridge
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Main, arg = " + args.FirstOrDefault());
            Console.Read();
        }
    }
}