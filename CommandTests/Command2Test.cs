using System;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    class TestDummay2
    {
        public int Number = 0;
        public void Inc(int a,int b)
        {
            Number = Number + a + b;
        }

        public void Dec(int a,int b)
        {
            Number = Number - a - b;
        }
    }

    [TestFixture ()]
    public class Command2Test
    {
        [Test ()]
        public void TestCtr ()
        {
            var dummy = new TestDummay2 ();
            Action<int,int> exeAction = dummy.Inc;
            Action<int,int> undoAction = dummy.Dec;

            Assert.Throws<ArgumentNullException> (() => new Command<int,int> (1,2,null, null));
            Assert.Throws<ArgumentNullException> (() => new Command<int,int> (2,3,exeAction, null));
            Assert.Throws<ArgumentNullException> (() => new Command<int,int> (3,4,null, undoAction));
        }

        [Test ()]
        public void TestExecute ()
        {
            var list = new List<int> ();
            Action<int,int> exeAction = (i, v) => list.Insert (i, v);
            Action<int,int> undoAction = (i, v) => list.RemoveAt (i);

            var command = new Command<int,int>(0,4,exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (1, list.Count);
            Assert.AreEqual (list[0],4);

            command.Undo ();
            Assert.AreEqual (0, list.Count);
        }
    }
}

