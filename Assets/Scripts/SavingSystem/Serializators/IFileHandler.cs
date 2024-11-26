using SaveSystem.Models;
using System.Collections;

namespace SaveSystem.Core
{
    internal interface IFileHandler
    {
        SaveMeta CreateSave(string name, string player = "", float progress = 0f, string description = "");
        IEnumerator LoadSave(SaveMeta meta);
        IEnumerator Save(SaveMeta meta);
        IEnumerator LoadSaveMetasFile();
        IEnumerator SaveMetasFile();
        IEnumerator DeleteSave(SaveMeta meta);
        IEnumerator DeleteAllSaves();
    }
}