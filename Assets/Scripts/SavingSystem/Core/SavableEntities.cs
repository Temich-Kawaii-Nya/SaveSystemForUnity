using System;
using System.Collections.Generic;

namespace SaveSystem.Core
{
    /// <summary>
    /// Manages a collection of all savable entities in the system.
    /// Automatically registers and unregisters savable entities during their lifecycle.
    /// </summary>
    internal sealed class SavableEntities : IDisposable
    {
        /// <summary>
        /// A list of all registered savable entity objects.
        /// </summary>
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
        public void Dispose()
        {
            MonoSavableObject.OnCreate -= Register;
            MonoSavableObject.OnDispose -= Unregister;
            SavableObject.OnCreate -= Register;
            SavableObject.OnDispose -= Unregister;
        }
        /// <summary>
        /// Registers a new savable entity to the collection.
        /// </summary>
        /// <param name="obj">The savable entity object to register.</param>
        private void Register(object obj)
        {
            _savableObjects.Add(obj);
        }
        /// <summary>
        /// Unregisters a savable entity from the collection.
        /// </summary>
        /// <param name="obj">The savable entity object to unregister.</param>
        private void Unregister(object obj)
        {
            _savableObjects.Remove(obj);
        }
    }
}
