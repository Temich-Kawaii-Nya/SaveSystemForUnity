using System;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// For MonoBehaviour entities
    /// All classes inherited from this class can be saved
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
