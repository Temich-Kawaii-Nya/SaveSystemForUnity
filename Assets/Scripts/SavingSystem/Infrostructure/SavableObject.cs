using System;

namespace SaveSystem
{
    /// <summary>
    /// All classes inherited from this class can be saved
    /// </summary>
    public class SavableObject : IDisposable
    {
        internal static event Action<object> OnCreate;
        internal static event Action<object> OnDispose;
        SavableObject() 
        {
            OnCreate?.Invoke(this);
        }
        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }
    }
}
