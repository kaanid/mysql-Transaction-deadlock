using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppMysqllock
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 10000; i++)
            {
                Task.Run(() =>
                {
                    //mysqlData.Run2(56);
                    mysqlData.Run6(56);
                });
                Thread.Sleep(100);
            }

            Console.Read();
        }
    }
}
