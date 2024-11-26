using System;
using System.Collections.Generic;
using System.Reflection;

namespace SaveSystem.Core
{
    /// <summary>
    /// Saves all fields marked with attribute "Save"
    /// </summary>
    internal class SaveFiledBinder
    {
        public Dictionary<Type, FieldInfo[]> Classes { get; private set; }
        public SaveFiledBinder()
        {
            Classes = new Dictionary<Type, FieldInfo[]>();
            FindClasses();
        }
        public void FindClasses()
        {
            foreach (var type in Assembly.GetCallingAssembly().GetTypes())
            {
                if (type.GetCustomAttribute<SaveAttribute>() == null)
                    continue;

                var fields = type.GetFields();

                var saveFields = new List<FieldInfo>();
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<SaveAttribute>() == null)
                        continue;
                    saveFields.Add(field);
                }
                Classes.Add(type, saveFields.ToArray());
            }
        }
    }
}
