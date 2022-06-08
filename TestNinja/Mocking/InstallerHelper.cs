using System.Net;

namespace TestNinja.Mocking
{
    public interface IFileDownloader
    {
        void DownloadFile(string address, string fileName);
    }

    public class FileDownloader: IFileDownloader
    {
        public void DownloadFile(string address, string fileName)
        {
            var client = new WebClient();
            client.DownloadFile(address, fileName);
        }
    }

    public class InstallerHelper
    {
        private readonly IFileDownloader _fileDownloader;
        const string BaseUrl = "http://example.com/{0}/{1}";
        public string SetupDestinationFile { get; set; }

        public InstallerHelper(IFileDownloader fileDownloader)
        {
            this._fileDownloader = fileDownloader;
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                _fileDownloader.DownloadFile(
                    string.Format(BaseUrl, customerName, installerName),
                    SetupDestinationFile);

                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}