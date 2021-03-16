using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace NLogDemoOne
{
    class NLogTestOne
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Error("This Is An Error Message!");
        }
    }
}
