using System;

namespace SaveSystem
{
    /// <summary>
    /// Attribute to mark classes, fields, or properties for inclusion in the save system.
    /// All fields or properties, marked with this attribute will be saved and loaded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class SaveAttribute : Attribute
    {
        public SaveAttribute()
        {

        }
    }
}
