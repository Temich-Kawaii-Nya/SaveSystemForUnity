using System;

namespace SaveSystem
{
    /// <summary>
    /// Base class for savable objects. 
    /// Classes inheriting from this class are automatically registered for saving and can be disposed.
    /// </summary>
    public class SavableObject : IDisposable
    {
        internal static event Action<object> OnCreate;
        internal static event Action<object> OnDispose;
        public SavableObject() 
        {
            OnCreate?.Invoke(this);
        }
        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }
    }
}
