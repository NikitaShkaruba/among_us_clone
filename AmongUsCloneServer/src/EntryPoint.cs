using System;

namespace AmongUsCloneServer
{
    internal class Program
    {
        public static void Main()
        {
            Console.Title = "Among Us Server";
            
            Server.Start(5, 26950);

            // Temporary solution to not hang up the server
            Console.ReadKey();
        }
    }
}
