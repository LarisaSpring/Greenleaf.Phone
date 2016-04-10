using System;
using System.Diagnostics;

namespace Greenleaf
{
    public class StopWatcher : IDisposable
    {
        private readonly string _moduleName;
        private readonly Action<string> _logger;
        private readonly Stopwatch _stopwatch;

        public static IDisposable StartNew(string moduleName, Action<string> logger)
        {
            return new StopWatcher(moduleName, logger);
        }
        
        public StopWatcher(string moduleName) :
            this(moduleName, null)
        {
        }

        public TimeSpan Elapsed { get; private set; }
        
        public StopWatcher(string moduleName, Action<string> logger)
        {
            _moduleName = moduleName;
            _logger = logger;

            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            Elapsed = _stopwatch.Elapsed;

            if (_logger != null)
            {
                var message = "{0} :: {1}".FormatWith(_moduleName, _stopwatch.Elapsed);
                _logger(message);
            }
        }
    }
}