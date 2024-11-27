using System;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Base class for savable MonoBehaviour entities.
    /// Any class inheriting from <see cref="MonoSavableObject"/> is automatically tracked by the save system.
    /// </summary>
    public class MonoSavableObject : MonoBehaviour, IDisposable
    {
        internal static event Action<object> OnCreate;
        internal static event Action<object> OnDispose;

        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }

        private void Start()
        {
            OnCreate?.Invoke(this);
        }
    }
}
