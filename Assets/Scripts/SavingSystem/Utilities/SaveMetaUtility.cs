using SaveSystem.Models;
using System;

namespace SaveSystem.Utilities
{
    internal class SaveMetaUtility
    {
        public static DateTime CalculatePlayTime(SaveMeta saveMeta)
        {
            var timeZoneDiff = DateTime.Now - DateTime.UtcNow;    

            var lastLoadTimeUtc = saveMeta.lastLoadTime - timeZoneDiff;
            var diff = DateTime.UtcNow - lastLoadTimeUtc;
            var newTime = saveMeta.time + diff;

            return newTime;
        }
        public static string GetFileName(SaveMeta meta, string extension)
        {
            return $"save_{meta.name}_{meta.id}.{extension}";
        }
    }
}
