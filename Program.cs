using System;
using System.Collections.Generic;
using System.Threading;

namespace LazySample
{
    class Program
    {
        private List<Lazy<ILoggerClass, LoggerMetadata>> LazyLoggerClasses { get; set; }
        
        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            Console.WriteLine("-- Create some logger objects with metadata --");
            this.CreateLogger();

            Console.WriteLine("-- Read the metadata --");
            foreach (var lazyDataClass in LazyLoggerClasses)
                Console.WriteLine(lazyDataClass.Metadata.Destination);

            Console.WriteLine("-- Invoke the method of LoggerClass --");
            Write("Debug", "message for debug");
            Write("Trace", "message for trace");
            Write("Release", "message for release");
        }

        private void CreateLogger()
        {
            LazyLoggerClasses = new List<Lazy<ILoggerClass, LoggerMetadata>>();

            LazyLoggerClasses.Add(new Lazy<ILoggerClass, LoggerMetadata>(() => { return new ShortFormatLogger(); },
                                                                         new LoggerMetadata("Trace")));

            LazyLoggerClasses.Add(new Lazy<ILoggerClass, LoggerMetadata>(() => { return new ShortFormatLogger(); },
                                                                         new LoggerMetadata("Debug")));

            LazyLoggerClasses.Add(new Lazy<ILoggerClass, LoggerMetadata>(() => { return new LongFormatLogger(); },
                                                                         new LoggerMetadata("Debugger")));
        }

        private void Write(string destination, string message)
        {
            var outputs = LazyLoggerClasses.FindAll(a => a.Metadata.Destination.Contains(destination));
            foreach (var lazyDataClass in outputs)
                lazyDataClass.Value.WriteMessage(message);
        }
    }

    public interface ILoggerClass
    {
        void WriteMessage(string message);
    }

    public class LongFormatLogger : ILoggerClass
    {
        public LongFormatLogger()
        {
            Console.WriteLine("LongFormatLogger constructor");
        }
        public void WriteMessage(string message)
        {
            Console.WriteLine("Message (" + DateTime.Now.ToLongDateString() + "): " + message);
        }
    }

    public class ShortFormatLogger : ILoggerClass
    {
        public ShortFormatLogger()
        {
            Console.WriteLine("ShortFormatLogger constructor");
        }
        public void WriteMessage(string message)
        {
            Console.WriteLine("Message (" + DateTime.Now.ToShortDateString() + "): " + message);
        }
    }

    public class LoggerMetadata
    {
        public string Destination { get; private set; }
        public LoggerMetadata(string destination)
        {
            this.Destination = destination;
        }
    }
}

