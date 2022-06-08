using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IFileDownloader> _mockClient;
        private InstallerHelper _installer;

        [SetUp]
        public void SetUp()
        {
            _mockClient = new Mock<IFileDownloader>();
            _installer = new InstallerHelper(_mockClient.Object);
        }

        [Test]
        public void DownloadInstaller_InvalidUrl_ThrowsException()
        {
            _installer = new InstallerHelper(new FileDownloader());

            Assert.That(() => _installer.DownloadInstaller("@", @":\\http"), Throws.ArgumentNullException);
        }        
        
        [Test]
        public void DownloadInstaller_DownloadFails_ReturnFalse()
        {
            _mockClient
                .Setup(fd => fd.DownloadFile(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<WebException>();
            
            Assert.That(_installer.DownloadInstaller("customer", "installer"), Is.False);
        }        
        
        [Test]
        public void DownloadInstaller_DownloadCompletes_ReturnTrue()
        {
            //_installer.BaseUrl = String.Empty;
            //_mockClient.Setup(fd => fd.DownloadFile(It.IsAny<string>(), It.IsAny<string>()));
            
            Assert.That(_installer.DownloadInstaller("customer", "installer"), Is.True);
        }
    }
}
