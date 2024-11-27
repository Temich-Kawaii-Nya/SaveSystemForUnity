using System;
using System.Collections.Generic;

namespace SaveSystem.Models
{
    /// <summary>
    /// Represents the data structure for storing saved information in a save file.
    /// </summary>
    [Serializable]
    public sealed class SavedData
    {
        /// <summary>
        /// A dictionary containing key-value pairs for saved data.
        /// </summary>
        public Dictionary<string, string> savedData;
        public SavedData()
        {
            savedData = new Dictionary<string, string>();
        }
    }
}
