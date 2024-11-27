using System;
using System.Collections.Generic;
using System.Reflection;

namespace SaveSystem.Core
{
    /// <summary>
    /// Binds all fields marked with the <see cref="SaveAttribute"/> for classes in the assembly.
    /// </summary>
    internal sealed class SaveFiledsBinder
    {
        /// <summary>
        /// A dictionary mapping types to arrays of their fields marked with the <see cref="SaveAttribute"/>.
        /// </summary>
        public Dictionary<Type, FieldInfo[]> Classes { get; private set; }
        public SaveFiledsBinder()
        {
            Classes = new Dictionary<Type, FieldInfo[]>();
            FindClasses();
        }
        /// <summary>
        /// Finds and binds all classes and their fields marked with the <see cref="SaveAttribute"/> in the calling assembly.
        /// </summary>
        private void FindClasses()
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
