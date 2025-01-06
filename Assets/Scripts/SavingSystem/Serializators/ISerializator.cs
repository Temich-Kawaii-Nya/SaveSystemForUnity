namespace SaveSystem.Core
{
    /// <summary>
    /// Interface for handling data serialization and deserialization.
    /// </summary>
    internal interface ISerializator
    {
        /// <summary>
        /// Deserializes data from a byte array into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the data into.</typeparam>
        /// <param name="data">The byte array containing serialized data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>a
        T DeserializeFile<T>(byte[] data);

        /// <summary>
        /// Serializes an object of type <typeparamref name="T"/> into a byte array.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="data">The object to serialize.</param>
        /// <returns>A byte array representing the serialized data.</returns>
        byte[] SerializeData<T>(T data);
    }
}