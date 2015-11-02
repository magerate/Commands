using System;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    class TestDummay0
    {
        public int Number = 0;
        public void Inc()
        {
            Number += 1;
        }

        public void Dec()
        {
            Number -= 1;
        }
    }

    [TestFixture ()]
    public class Command0Test
    {
        [Test ()]
        public void TestCtr ()
        {
            var dummy = new TestDummay0 ();
            Action exeAction = dummy.Inc;
            Action undoAction = dummy.Dec;

            Assert.Throws<ArgumentNullException> (() => Command.Create (null, null));
            Assert.Throws<ArgumentNullException> (() => Command.Create (exeAction, null));
            Assert.Throws<ArgumentNullException> (() => Command.Create (null, undoAction));
        }

        [Test ()]
        public void TestExecute ()
        {
            var dummy = new TestDummay0 ();
            Action exeAction = dummy.Inc;
            Action undoAction = dummy.Dec;

            var command = Command.Create (exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (dummy.Number, 1);

            command.Execute ();
            Assert.AreEqual (dummy.Number, 2);

            command.Undo ();
            Assert.AreEqual (dummy.Number, 1);

            command.Redo ();
            Assert.AreEqual (dummy.Number, 2);
        }
    }
}

