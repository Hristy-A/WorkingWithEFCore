using System;
using System.Diagnostics;

namespace Store.Infrastructure.Loggers
{
    public class DebugWindowLogger : ILogger
    {
        public void Log(string msg) => Debug.WriteLine(msg);
    }
}
