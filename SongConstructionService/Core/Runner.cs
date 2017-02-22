using Logging;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace SongConstructionService
{
    static class Runner
    {
        /// <summary> /// The main entry point for the application.  /// </summary>
        static void Main()
        {
            try
            {

            // We wan to run only our service
            SongConstructionService serviceObj = new SongConstructionService();
            serviceObj.ServiceName = "SongConstructionService";
            ServiceBase[] servicesToRun = new ServiceBase[] { serviceObj };

            // I use this to make debugging easier. Debugging a windows service usually requires
            // installing the .exe, starting it using the service manager, then attaching the debugger.
            // This way, if this project is run through visual studio, we will call the OnStart and OnStop
            // methods using reflection allowing for quick and easy debugging. (A breakpoint must be used)
            if (!Environment.UserInteractive)
            {
                // Startup as a service
                Logger.Log("Starting service as a service.");
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                // Startup as an application
                Logger.Log("Starting service as interactive.");
                RunInteractive(servicesToRun);
            }

            }catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        // This function is calls onStart and onStop of the services using reflection for easier debugging
        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("Started");
            }

            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }

            Console.WriteLine("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }
    }
}
