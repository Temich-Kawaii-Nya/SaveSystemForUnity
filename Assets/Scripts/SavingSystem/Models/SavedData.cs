using System;
using System.Collections.Generic;

namespace SaveSystem.Models
{
    /// <summary>
    /// Stores data in save
    /// </summary>
    [Serializable]
    public sealed class SavedData
    {
        public Dictionary<string, string> savedData;
        public SavedData()
        {
            savedData = new Dictionary<string, string>();
        }
    }
}
