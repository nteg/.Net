// ***********************************************************************
// Copyright (c) 2008 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using NUnit.TestUtilities;

namespace NUnit.Framework.Assertions
{
	[TestFixture]
	public class AssertThrowsTests
	{
		[Test]
		public void CorrectExceptionThrown()
		{
            Assert.Throws(typeof(ArgumentException), TestDelegates.ThrowsArgumentException);
            Assert.Throws(typeof(ArgumentException),
                delegate { throw new ArgumentException(); });

            Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException(); });
            Assert.Throws<ArgumentException>(TestDelegates.ThrowsArgumentException);

            // Without cast, delegate is ambiguous before C# 3.0.
            Assert.That((TestDelegate)delegate { throw new ArgumentException(); },
                    Throws.Exception.TypeOf<ArgumentException>() );
            //Assert.Throws( Is.TypeOf(typeof(ArgumentException)),
            //        delegate { throw new ArgumentException(); } );
        }

		[Test]
		public void CorrectExceptionIsReturnedToMethod()
		{
			ArgumentException ex = Assert.Throws(typeof(ArgumentException),
                new TestDelegate(TestDelegates.ThrowsArgumentException)) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

            ex = Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException("myMessage", "myParam"); }) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

			ex = Assert.Throws(typeof(ArgumentException), 
                delegate { throw new ArgumentException("myMessage", "myParam"); } ) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

            ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsArgumentException) as ArgumentException;

            Assert.IsNotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif
		}

		[Test, ExpectedException(typeof(AssertionException))]
		public void NoExceptionThrown()
		{
            ArgumentException ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsNothing);
            Assert.That(ex.Message, Is.EqualTo(
				"  Expected: <System.ArgumentException>" + Env.NewLine +
				"  But was:  null" + Env.NewLine));
		}

        [Test, ExpectedException(typeof(AssertionException))]
        public void UnrelatedExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsNullReferenceException);
            Assert.That(ex.Message, Is.StringStarting(
                "  Expected: <System.ArgumentException>" + Env.NewLine +
                "  But was:  <System.NullReferenceException> (my message)" + Env.NewLine));
            Assert.That(ex.Message, Contains.Substring("  at "));
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void BaseExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsSystemException);
            Assert.That(ex.Message, Is.StringStarting(
                "  Expected: <System.ArgumentException>" + Env.NewLine +
                "  But was:  <System.Exception> (my message)" + Env.NewLine));
            Assert.That(ex.Message, Contains.Substring("  at "));
        }

        [Test,ExpectedException(typeof(AssertionException))]
        public void DerivedExceptionThrown()
        {
            Exception ex = Assert.Throws<Exception>(TestDelegates.ThrowsArgumentException);
            Assert.That(ex.Message, Is.StringStarting(
                "  Expected: <System.Exception>" + Env.NewLine +
                "  But was:  <System.ArgumentException> (myMessage" + Env.NewLine +
                "Parameter name: myParam)" + Env.NewLine));
            Assert.That(ex.Message, Contains.Substring("  at "));
        }

        [Test]
        public void DoesNotThrowSuceeds()
        {
            Assert.DoesNotThrow(TestDelegates.ThrowsNothing);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void DoesNotThrowFails()
        {
            Assert.DoesNotThrow(TestDelegates.ThrowsArgumentException);
        }
    }
}
