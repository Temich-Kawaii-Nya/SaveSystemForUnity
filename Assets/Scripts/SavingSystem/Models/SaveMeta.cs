using System;

namespace SaveSystem.Models
{
    /// <summary>
    /// Contains information about Save
    /// </summary>
    [Serializable]
    public sealed class SaveMeta
    {
        public Guid id;
        public string player;
        public string name;

        public string filePath;

        public float progress;
        public float volume;
        public string descrioption;

        public DateTime lastLoadTime;
        public DateTime lastUpdate;
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
