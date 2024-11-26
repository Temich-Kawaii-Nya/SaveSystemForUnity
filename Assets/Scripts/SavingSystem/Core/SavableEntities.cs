using System;
using System.Collections.Generic;

namespace SaveSystem.Core
{
    /// <summary>
    /// Stores list of all savable entities objects
    /// </summary>
    internal class SavableEntities : IDisposable
    {
        public List<object> SavableEntitiesList => _savableObjects;

        private List<object> _savableObjects;
        public SavableEntities()
        {
            _savableObjects = new List<object>();

            MonoSavableObject.OnCreate += Register;
            MonoSavableObject.OnDispose += Unregister;
            SavableObject.OnCreate += Register;
            SavableObject.OnDispose += Unregister;
        }
        private void Register(object obj)
        {
            _savableObjects.Add(obj);
        }
        private void Unregister(object obj)
        {
            _savableObjects.Remove(obj);
        }
        public void Dispose()
        {
            MonoSavableObject.OnCreate -= Register;
            MonoSavableObject.OnDispose -= Unregister;
            SavableObject.OnCreate -= Register;
            SavableObject.OnDispose -= Unregister;
        }
    }
}
