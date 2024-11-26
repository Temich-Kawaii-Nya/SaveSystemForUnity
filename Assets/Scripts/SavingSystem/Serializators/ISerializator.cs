namespace SaveSystem.Core
{
    internal interface ISerializator
    {
        T DeserializeFile<T>(byte[] data);
        byte[] SerializeData<T>(T data);
    }
}