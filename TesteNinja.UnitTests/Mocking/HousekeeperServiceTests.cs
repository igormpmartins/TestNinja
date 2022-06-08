using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HousekeeperServiceTests
    {
        private Mock<IEmailSender> _email;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IXtraMessageBox> _messageBox;
        private HousekeeperService _service;
        private DateTime _statementDateTime = new DateTime(2022, 4, 26);
        private Housekeeper _houseKeeper;
        private string _fileName;

        [SetUp]
        public void SetUp()
        {
            _houseKeeper = new Housekeeper
            {
                Oid = 1,
                FullName = "FullName",
                Email = "Test"
            };

            _fileName = "fileName";

            _email = new Mock<IEmailSender>();
            
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(r => r.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDateTime))
                .Returns(() => _fileName);

            _unitOfWork = new Mock<IUnitOfWork>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperService(_unitOfWork.Object, _email.Object, 
                _statementGenerator.Object, _messageBox.Object);

            _unitOfWork.Setup(r => r.Query<Housekeeper>())
                .Returns(new List<Housekeeper> { _houseKeeper }
                .AsQueryable());
        }

        [Test]
        public void SendStatementEmails_WhenCalled_RunQueryForHousekeeper()
        {
            _service.SendStatementEmails(_statementDateTime);
            _unitOfWork.Verify(u => u.Query<Housekeeper>());
        }
        
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_WhenHousekeeperEmail_ShouldNotGenerateStatements(string email)
        {
            _houseKeeper.Email = email;

            _service.SendStatementEmails(_statementDateTime);

            _statementGenerator
                .Verify(r => r.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDateTime), Times.Never);
           
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            _service.SendStatementEmails(_statementDateTime);

            _statementGenerator
                .Verify(r => r.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDateTime));
        }

        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _service.SendStatementEmails(_statementDateTime);

            _email
                .Verify(e => 
                    e.EmailFile(_houseKeeper.Email, _houseKeeper.StatementEmailBody, _fileName, It.IsAny<string>()));
        }        
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_StatementFilenameEmpty_ShouldNotEmailTheStatement(string filename)
        {
            _fileName = filename;

            _service.SendStatementEmails(_statementDateTime);

            _email
                .Verify(e => 
                    e.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                    Times.Never);
        }

        [Test]
        public void SendStatementEmails_EmailSendingFails_ShouldDisplayMessageBox()
        {
            _email
                .Setup(es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();
            
            _service.SendStatementEmails(_statementDateTime);

            _messageBox
                .Verify(b => b.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }


    }
}
