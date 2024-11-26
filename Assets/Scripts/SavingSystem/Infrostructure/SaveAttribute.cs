using System;

namespace SaveSystem
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class SaveAttribute : Attribute
    {
        public string Key { get; private set; }
        public SaveAttribute()
        {

        }
        public SaveAttribute(string key)
        {
            Key = key;
        }
    }
}
