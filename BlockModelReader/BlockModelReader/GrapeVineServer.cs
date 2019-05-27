using Grapevine.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockModelReader
{
    class GrapeVineServer
    {

        public static void StartServer()
        {
            using (var server = new RestServer())
            {
                server.LogToConsole().Start();
                Console.ReadLine();
                server.Stop();
            }
        }
    }
}
