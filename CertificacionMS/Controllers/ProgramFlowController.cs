using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

/*
 * Como construir Aplicaciones Multithreaded 
 * */
namespace CertificacionMS.Controllers
{
    [Route("api/[controller]")]
    public class ProgramFlowController : Controller
    {
        public static List<string> output = new List<string>();
        // GET: api/values
        /// <summary>
        /// Each application runs in its own process, 
        ///     A thread, is something like a virtualized CPU
        ///     If an application Crashes or hits an infinitive loop,
        ///     Only the application's process is affected.
        /// Parallelism = Application can execute multiple threads on different CPU in parallel
        /// Multiple CPU's Similar~ multiple Cores
        /// </summary>
        /// <returns>The get.</returns>
        [HttpGet]
        [Route("Threads")]
        public IEnumerable<string> Get()
        {
            //Using Thread Class
            /*
             * Using System.Threading namespace
             * 
             * Usually isn't something that you should use un your application, 
             *      except when you have special needs.
             * 
             * It's like a low level class, 
             *  You can control all configuration options
             *  Ej; Specify the priority of your thread 
             *          For long running  
             * */

            /*
             * Run an method on another thread
             *  Syncronization = Mechanism of ensuring that two threads don't 
             *                  execute a specific portion of your program at the 
             *                  same time.
             * 
             *                  If one thread is working with the output stream,
             *                  other threads will have to wait before it's finished
             * */
            output = new List<string>();
            output.Add("....");
            //Thread t = new Thread(new ThreadStart(ThreadMethod)); 
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod));//Execute method with parameters
            t.IsBackground = false; //
            //Foreground  threads can be used to keep an application alive 
            //t.Start();
            t.Start(5); //Passing parameters to Thread Methods

            for (int i = 0; i < 10; i++)
            {
                output.Add("Main thread: Do some work.");
                Console.WriteLine("Main thread: Do some work.");
                /* This is used to signal to Windows that the thread is finished
                      Instead of waiting for the whole time-slice of the thread to finish 
                      , it will immediately switch to another thread.*/
                Thread.Sleep(0);
            }
            //This method is called on the main thread to let it wait until the 
            //  other thread finishes
            t.Join();
            return output;
        }

        public static void ThreadMethod(){
            for (int i = 0; i < 10; i++){
                output.Add($"ThreadProc: {i}");
                Console.WriteLine($"ThreadProc: {i}");
                Thread.Sleep(0);
            }
        }

        public static void ThreadMethod(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                output.Add($"ThreadProc: {i}");
                Console.WriteLine($"ThreadProc: {i}");
                Thread.Sleep(0);
            }
        }

        public static void MainThreadStop(){

            bool stopped = false;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while(!stopped){
                    Console.WriteLine("Running ...");
                    Thread.Sleep(1000);
                }
            }));

            thread.Start();

            Console.WriteLine("Press Any Key to exit");
            Console.ReadKey();

            stopped = true;
            thread.Join();
        }
    }
}
