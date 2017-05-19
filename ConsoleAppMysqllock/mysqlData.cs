using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Transactions;
using System.Threading;
using System.Data;

namespace ConsoleAppMysqllock
{
    public class mysqlData
    {
        public static void Create()
        {
            using (var conn = DataManage.Conn())
            {
                using (MySqlTransaction tr = conn.BeginTransaction())
                {
                    conn.Execute(@"CREATE TABLE `OCRRecord` (
                                  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
                                  `Aid` bigint(20) NOT NULL DEFAULT '0' COMMENT 'Aid',
                                  PRIMARY KEY(`Id`)
                                ) ENGINE = InnoDB AUTO_INCREMENT = 1 DEFAULT CHARSET = utf8mb4;
                    ", transaction: tr);
                }
            }
        }

        /// <summary>
        /// normal
        /// </summary>
        /// <param name="id"></param>
        public static void Run(int id = 56)
        {
            var conn2 = DataManage.Conn();
            string str32 = conn2.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
            Console.WriteLine("{1} select aid:{0}", str32, Thread.CurrentThread.ManagedThreadId);

            using (var trans = new TransactionScope())
            {
                var conn = DataManage.Conn();

                //1:normal
                string str = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id for update", new { Id = id });
                Console.WriteLine("{1} select aid:{0}",str,Thread.CurrentThread.ManagedThreadId);
                
                bool flag=conn.Execute("update OCRRecord set aid=@Aid where id=@Id", new { Aid = DateTime.Now.Second, Id = id })>0;
                Console.WriteLine("{1} update {0}", flag, Thread.CurrentThread.ManagedThreadId);

                string str2 = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str2, Thread.CurrentThread.ManagedThreadId);

            }
        }

        /// <summary>
        /// MySql.Data.MySqlClient.MySqlException:“Deadlock found when trying to get lock; try restarting transaction
        /// </summary>
        /// <param name="id"></param>
        public static void Run2(int id = 56)
        {
            var conn2 = DataManage.Conn();
            string str32 = conn2.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
            Console.WriteLine("{1} select aid:{0}", str32, Thread.CurrentThread.ManagedThreadId);

            using (var trans = new TransactionScope())
            {
                var conn = DataManage.Conn();

                //2: MySql.Data.MySqlClient.MySqlException:“Deadlock found when trying to get lock; try restarting transaction
                string str = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str, Thread.CurrentThread.ManagedThreadId);

                bool flag = conn.Execute("update OCRRecord set aid=@Aid where id=@Id", new { Aid = DateTime.Now.Second, Id = id }) > 0;
                Console.WriteLine("{1} update {0}", flag, Thread.CurrentThread.ManagedThreadId);

                string str2 = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str2, Thread.CurrentThread.ManagedThreadId);


                //Thread.Sleep(100);
            }
        }

        /// <summary>
        /// normal
        /// </summary>
        /// <param name="id"></param>
        public static void Run3(int id = 56)
        {
            var conn2 = DataManage.Conn();
            string str = conn2.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id", new { Id = id });
            Console.WriteLine("{1} select aid:{0}", str, Thread.CurrentThread.ManagedThreadId);

            using (var trans = new TransactionScope())
            {
                var conn = DataManage.Conn();

                bool flag = conn.Execute("update OCRRecord set aid=@Aid where id=@Id", new { Aid = DateTime.Now.Second, Id = id }) > 0;
                Console.WriteLine("{1} update {0}", flag, Thread.CurrentThread.ManagedThreadId);

                string str2 = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str2, Thread.CurrentThread.ManagedThreadId);


                //Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public static void Run4(int id = 56)
        {
            using (var conn = DataManage.Conn())
            {
                string str = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str, Thread.CurrentThread.ManagedThreadId);

                bool flag = conn.Execute("update OCRRecord set aid=aid+@Aid where id=@Id", new { Aid = 1, Id = id }) > 0;
                Console.WriteLine("{1} update {0}", flag, Thread.CurrentThread.ManagedThreadId);

                string str2 = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                Console.WriteLine("{1} select aid:{0}", str2, Thread.CurrentThread.ManagedThreadId);
            }
        }

        public static void Run6(int id = 56)
        {
            using (var conn = DataManage.Conn())
            {
                using (var tr = conn.BeginTransaction())
                {
                    //string str = conn.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id },transaction:tr);
                    string str = tr.Connection.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                    Console.WriteLine("{1} select aid:{0}", str, Thread.CurrentThread.ManagedThreadId);

                    bool flag = tr.Connection.Execute("update OCRRecord set aid=aid+@Aid where id=@Id", new { Aid = 1, Id = id }) > 0;
                    Console.WriteLine("{1} update {0}", flag, Thread.CurrentThread.ManagedThreadId);

                    string str2 = tr.Connection.QueryFirstOrDefault<string>("select aid from OCRRecord where id=@Id;", new { Id = id });
                    Console.WriteLine("{1} select aid:{0}", str2, Thread.CurrentThread.ManagedThreadId);
                }
            }
        }
    }

    public class DataManage
    {
        public static MySqlConnection Conn()
        {
            var conn = new MySqlConnection("DataSource=10.0.17.10;Database=JuketoolActivityRecord_test;uid=juketool;pwd=abc@123");
            conn.Open();
            return conn;
        }
    }
}
