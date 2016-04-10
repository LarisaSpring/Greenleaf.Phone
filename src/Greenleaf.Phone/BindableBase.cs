using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Greenleaf.Phone
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        private const string ErrorMsg = "{0} is not a public property of {1}";
        private static readonly Dictionary<string, PropertyChangedEventArgs> _cache = new Dictionary<string, PropertyChangedEventArgs>();
        private readonly Dictionary<string, object> _changedProperties = new Dictionary<string, object>();
        private string[] _properties;
        private bool _allowPropertyChangedNotifications = true;

        protected bool AllowPropertyChangedNotifications
        {
            get { return _allowPropertyChangedNotifications; }
            set { _allowPropertyChangedNotifications = value; }
        }

        protected List<string> GetChangedProperties()
        {
            return _changedProperties.Keys.ToList();
        }
        
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> path)
        {
            var me = path.Body as MemberExpression;
            if (me != null)
            {
                OnPropertyChanged(me.Member.Name);
            }
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (!_allowPropertyChangedNotifications)
            {
                return;
            }
            
            if (propertyName == null)
            {
#if DEBUG
                throw new ArgumentNullException("propertyName"); 
#else
                return;
#endif
            }

#if DEBUG
            VerifyProperty(propertyName);
#endif

            if (_changedProperties != null)
            {
                _changedProperties[propertyName] = null;
            }
            
            var handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChangedEventArgs value;
                
                lock (_cache)
                {
                    if (!_cache.TryGetValue(propertyName, out value))
                    {
                        value = new PropertyChangedEventArgs(propertyName);
                        _cache.Add(propertyName, value);
                    }
                }

                handler(this, value);
            }
        }

        /// <summary>
        /// Clear changes history of any properties.
        /// </summary>
        protected void Reset()
        {
            _changedProperties.Clear();
        }

        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            if (_properties == null)
            {
                _properties = GetType().GetRuntimeProperties().Select(x => x.Name).ToArray();
            }

            if (!_properties.Contains(propertyName))
            {
                // The property could not be found,
                // so alert the developer of the problem.

                var msg = string.Format(
                    ErrorMsg,
                    propertyName,
                    GetType().Name);

#if DEBUG
                Debug.Assert(false, msg);
#endif
            }
        }
    }
}
