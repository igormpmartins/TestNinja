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
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeStorage> mock;
        private EmployeeController controller;

        [SetUp]
        public void SetUp()
        {
            mock = new Mock<IEmployeeStorage>();
            controller = new EmployeeController(mock.Object);
        }

        [Test]
        public void DeleteEmployee_GivenId_RemoveEmployeeById()
        {
            var id = 1;
            controller.DeleteEmployee(id);

            mock.Verify(s => s.DeleteEmployee(id));
        }

        [Test]
        public void DeleteEmployee_Redirect_ReturnRedirectResult()
        {
            var result = controller.DeleteEmployee(1);

            Assert.That(result, Is.InstanceOf<RedirectResult>());
        }
    }
}
