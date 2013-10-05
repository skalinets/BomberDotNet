using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bomberman.Api;
using Ninject;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Console.Out.WriteLine("starting runner ...");
            new Bootstrapper().GetGame().Start();

            Console.ReadLine();

        }

        private static void Bootstrap()
        {
//            var kernel = new StandardKernel();
//            kernel.Bind(x => 
//                x.FromThisAssembly)
        }
    }
}
