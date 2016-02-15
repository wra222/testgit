using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Entity.Infrastructure.Framework;
//using IMES.Entity.Repository.Interface;
using IMES.Entity.Repository.Meta;
using System.Threading;
//using IMES.Entity.Repository.Implementation;
using IMES.Entity.Infrastructure.Interface;
using log4net;
using System.Reflection;

namespace Repository
{
    class Program
    {
        protected static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start Main thread:{0}", Thread.CurrentThread.ManagedThreadId);
                ThreadPool.QueueUserWorkItem(work1);
                ThreadPool.QueueUserWorkItem(work1);
                ThreadPool.QueueUserWorkItem(work1);
                ThreadPool.QueueUserWorkItem(work1);
                ThreadPool.QueueUserWorkItem(work1);
                ThreadPool.QueueUserWorkItem(work1);
                //Thread td1 = new Thread(work1);
                //Thread td2 = new Thread(work2);
                //Thread td3 = new Thread(work1);
                //Thread td4 = new Thread(work1);
                //td1.Start();
                //td2.Start();
                //td3.Start();
                //td4.Start();
                //td1.Join();
                //td2.Join();
                //td3.Join();
                //td4.Join();
                Console.WriteLine("Waiting Main thread:{0}", Thread.CurrentThread.ManagedThreadId);
                //Thread.CurrentThread.Join();
                Console.WriteLine("End Main thread:{0}", Thread.CurrentThread.ManagedThreadId);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }


        }
        static void work1(object t)
        {
            Console.WriteLine("Start thread:{0}", Thread.CurrentThread.ManagedThreadId);
            //UnitOfWork.Begin("DBServer");
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //IProductRepository prodRep = new ProductRepository("DBServer");
            //Console.WriteLine("ProductID : {0} ", prodRep.First(x => x.Model == "PCBE15YA837Y"  && string.IsNullOrEmpty(x.CUSTSN)).ProductID);
            //Product p = prodRep.First(x => string.IsNullOrEmpty(x.CUSTSN));
            //p.CUSTSN = "test";
            //UnitOfWork uow = new UnitOfWork();
            //using (uow )
            //{
                IRepository<Product> prodRep = new Repository<Product>("HPIMES");
                IRepository<ProductStatus> prodStatusRep = new Repository<ProductStatus>("HPIMES");
                var E = (from p in prodRep.Query()
                         join s in prodStatusRep.Query() on p.ProductID equals s.ProductID
                         where p.ProductID == "A35500006 "
                         select new { prod = p }).ToList();

                Console.WriteLine("HPIMES productID: {0} ", E[0].prod.ProductID);

               // Product p = prodRep.Table
                var K = (from p in prodRep.Query()
                        //join s in prodStatusRep.Query() on p.ProductID equals s.ProductID
                        where p.ProductID == "A35500006 "
                        select p).ToList();
                
                

             
                Console.WriteLine("HPIMES productID: {0} ", K[0].ProductID);
                IRepository<ProductStatus> tsbprodStatusRep = new Repository<ProductStatus>("TSBIMES");
                var D = (from s in tsbprodStatusRep.Query()
                         where s.ProductID == "BE9000007"
                         select s).ToList();

                Console.WriteLine(" TSBIMES productID: {0} ", D[0].ProductID);

                K[0].CUSTSN = Thread.CurrentThread.ManagedThreadId.ToString();
                D[0].Station = Thread.CurrentThread.ManagedThreadId.ToString();

                UnitOfWork.ThreadCommit();
                //Product p1 = new Product
                //{
                //    ProductID ="A355000067",
                //    CUSTSN = "test1111",
                //    Model = "PCBE15YA837Y",
                //    PCBID = "",
                //    Udt = DateTime.Now.AddDays(-1)
                //};
                ////prodRep.Delete(p);
                //prodRep.Update(p1);


                ////foreach (Product item in prodRep.GetProductByModel("PCBE15YA837Y"))
                ////{
                ////    if (string.IsNullOrEmpty(item.CUSTSN))
                ////    {
                ////        item.CUSTSN = "";
                ////    }
                ////    Thread.Sleep(20);
                ////    Console.WriteLine("thread:{0} ProductID:{1} CustomSN:{2}", Thread.CurrentThread.ManagedThreadId,  item.ProductID, item.CUSTSN);
                ////}

                ////prodRep.Save();
                //uow.Commit();
            //}
            Console.WriteLine("End thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }


        static void work2()
        {
            Console.WriteLine("Start thread:{0}", Thread.CurrentThread.ManagedThreadId);
            //UnitOfWork uow = new UnitOfWork();
            //UnitOfWork.Begin("DBServer");
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //IProductRepository prodRep = new ProductRepository("DBServer");
            //Console.WriteLine("ProductID : {0} ", prodRep.Single(x => string.IsNullOrEmpty( x.CUSTSN)).ProductID);
            //using (UnitOfWork uow = new UnitOfWork())
            //{
             IRepository<Product> prodRep = new Repository<Product>("HPIMES");
                //foreach (Product item in prodRep.Find(x => x.Model == "PCBE15YA837Y"))
                //{
                //    if (string.IsNullOrEmpty(item.CUSTSN))
                //    {
                //        item.CUSTSN = "";
                //    }
                //    Thread.Sleep(20);
                //    Console.WriteLine("thread:{0} ProductID:{1} CustomSN:{2}", Thread.CurrentThread.ManagedThreadId, item.ProductID, item.CUSTSN);
                //}

                Product p1 = new Product
                {
                    ProductID = "A35500006 ",
                    CUSTSN = "test1111",
                    Model = "PCBE15YA837Y",
                    PCBID = "",
                    Udt = DateTime.Now.AddDays(-1)
                };
                //prodRep.Delete(p);
                prodRep.Update(p1);
                
                //prodRep.Save();
                UnitOfWork.ThreadCommit();
                Console.WriteLine("Update!!");
            //}
            Console.WriteLine("End thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
