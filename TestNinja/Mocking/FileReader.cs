using System.IO;

namespace TestNinja.Mocking
{
    public interface IFileReader
    {
        string Read(string path);
    }

    public class FileReader : IFileReader
    {
        public string Read(string path) => File.ReadAllText(path);

    }
}