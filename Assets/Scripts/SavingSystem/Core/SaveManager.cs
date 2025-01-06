using SaveSystem.Models;
using System;
using System.Collections.Generic;
/// <summary>
/// Manage saves metadata from files and save data from current save
/// </summary>
internal sealed class SaveManager
{
    /// <summary>
    /// Dictionary storing loaded metadata for all saves.
    /// </summary>
    public Dictionary<Guid, SaveMeta> SaveMetas { get; set; }
    
    /// <summary>
    /// Stores the current save data, representing the state of all savable entities.
    /// </summary>
    public SavedData SaveData { get; set; }
    public SaveManager()
    {
        SaveMetas = new();
        SaveData = new SavedData();
    }

}
