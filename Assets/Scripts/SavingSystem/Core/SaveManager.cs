using SaveSystem.Models;
using System;
using System.Collections.Generic;
/// <summary>
/// Store saves meta files and save data from current save
/// </summary>
internal class SaveManager
{
    public Dictionary<Guid, SaveMeta> SaveMetas { get; set; }
    public SavedData SaveData { get; set; }
    public SaveManager()
    {
        SaveMetas = new();
        SaveData = new SavedData();
    }

}
