using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsharpRetrohwexception
{
    class Program
    {
        /// <summary>
        /// Net 4.5 C# using ExceptionDispatchInfo.Capture to rethrow exceptions
		/// copiado de la web > https://thecsharper.com/?p=277
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var exceptions = new BlockingCollection<ExceptionDispatchInfo>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    ThrowOne();
                }
                catch (Exception ex)
                {
                    var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);

                    exceptions.Add(exceptionDispatchInfo);
                }
                exceptions.CompleteAdding();
            });

            foreach (var exceptionDispatchInfo in exceptions.GetConsumingEnumerable())
            {
                try
                {
                    exceptionDispatchInfo.Throw();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Throws Not Supported Exception.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        private static void ThrowOne()
        {
            Console.WriteLine("Throw Not Supported Exception");

            ThrowTwo();

            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws Not Implemented Exception.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void ThrowTwo()
        {
            Console.WriteLine("Throw Not Implemented Exception");

            ThrowThree();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws Argument Null Exception.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        private static void ThrowThree()
        {
            Console.WriteLine("Throw Argument Null Exception");

            throw new ArgumentNullException();
        }
    }
}
