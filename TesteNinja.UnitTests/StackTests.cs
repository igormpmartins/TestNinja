using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        private Stack<string> stack;

        [SetUp]
        public void SetUp()
        {
            stack = new Stack<string>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(3)]
        public void Count_TestSize_ReturnSize(int size)
        {
            for (var i = 0; i < size; i++)
                stack.Push("a");

            Assert.That(stack.Count, Is.EqualTo(size));
        }

        [Test]
        public void Push_AddElement_ReturnLastElement()
        {
            stack.Push("a");
            stack.Push("b");

            Assert.That(stack.Peek(), Is.EqualTo("b"));
        }

        [Test]
        public void Pop_AddElement_ReturnLastElement()
        {
            stack.Push("a");
            stack.Push("b");
            var result = stack.Pop();

            Assert.That(result, Is.EqualTo("b"));
        }        
        
        [Test]
        public void Peek_AddElement_ReturnLastElement()
        {
            stack.Push("a");
            stack.Push("b");
            stack.Pop();

            Assert.That(stack.Peek, Is.EqualTo("a"));
        }        
        
        [Test]
        public void Peek_AddElements_DoesNotRemoveObjectOnTopStack()
        {
            stack.Push("a");
            stack.Push("b");
            stack.Push("c");
            stack.Peek();

            Assert.That(stack.Count, Is.EqualTo(3));
        }

        [Test]
        public void Peek_AddNullElement_ThrowException()
        {
            Assert.That(() => stack.Peek(), Throws.InvalidOperationException);
        }        
        
        [Test]
        public void Pop_EmptyList_ThrowException()
        {
            Assert.That(() => stack.Pop(), Throws.InvalidOperationException);
        }     
        
        [Test]
        public void Push_NullObject_ThrowException()
        {
            Assert.That(() => stack.Push(null), Throws.ArgumentNullException);
        }

    }
}
