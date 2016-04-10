using System;

namespace Greenleaf
{
    public class ValueSwitcher : IDisposable
    {
        private readonly Action<bool> _action;

        public ValueSwitcher(Action<bool> action)
        {
            _action = action;

            _action(true);
        }

        public void Dispose()
        {
            _action(false);
        }
    }
}