using System;

namespace SaveSystem.Models
{
    /// <summary>
    /// Represents metadata information about a save, including details about the player, progress, and timestamps.
    /// </summary>
    [Serializable]
    public sealed class SaveMeta
    {
        /// <summary>
        /// Unique identifier for the save file.
        /// </summary>
        public Guid id;

        /// <summary>
        /// Name of the player associated with this save.
        /// </summary>
        public string player;

        /// <summary>
        /// Name of the save.
        /// </summary>
        public string name;

        /// <summary>
        /// File path where the save is stored.
        /// </summary>
        public string filePath;

        /// <summary>
        /// Progress percentage associated with this save.
        /// </summary>
        public float progress;

        /// <summary>
        /// Volume of the save.
        /// </summary>
        public float volume;

        /// <summary>
        /// Description of the save, providing additional details.
        /// </summary>
        public string descrioption;

        /// <summary>
        /// The last time this save file was loaded.
        /// </summary>
        public DateTime lastLoadTime;

        /// <summary>
        /// The last time this save file was updated.
        /// </summary>
        public DateTime lastUpdate;

        /// <summary>
        /// Played time. Adds the time between load and save.
        /// </summary>
        public DateTime time;
        public SaveMeta(Guid id, string name, string player, float progress = 0f, string description = "")
        {
            this.id = id;
            this.name = name;
            this.progress = progress;
            this.player = player;
            this.descrioption = description;
        }
    }
}
