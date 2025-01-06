using SaveSystem.Models;
using System;

namespace SaveSystem.Utilities
{
    /// <summary>
    /// Provides utility methods for handling SaveMeta objects.
    /// </summary>
    internal sealed class SaveMetaUtility
    {
        /// <summary>
        /// Calculates the total playtime based on the provided SaveMeta.
        /// Calculate save time depends on timezone.
        /// </summary>
        /// <param name="saveMeta">The SaveMeta.</param>
        /// <returns>The updated cumulative playtime as a DateTime.</returns>
        public static DateTime CalculatePlayTime(SaveMeta saveMeta)
        {
            var timeZoneDiff = DateTime.Now - DateTime.UtcNow;    

            var lastLoadTimeUtc = saveMeta.lastLoadTime - timeZoneDiff;
            var diff = DateTime.UtcNow - lastLoadTimeUtc;
            var newTime = saveMeta.time + diff;

            return newTime;
        }
        /// <summary>
        /// Generates a save file name based on SaveMeta properties.
        /// </summary>
        /// <param name="meta">The SaveMeta.</param>
        /// <param name="extension">The file extension to append to the save file name.</param>
        /// <returns>A formatted string representing the save file name.</returns>
        public static string GetFileName(SaveMeta meta, string extension)
        {
            return $"save_{meta.name}_{meta.id}.{extension}";
        }
    }
}
