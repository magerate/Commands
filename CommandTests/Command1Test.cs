using System;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    [TestFixture ()]
    public class Command1Test
    {
        [Test ()]
        public void TestCtr ()
        {
            var list = new List<int> ();
            Action<int> exeAction = list.Add;
            Action<int> undoAction = list.RemoveAt;

            Assert.Throws<ArgumentNullException> (() => new Command<int> (1,null, null));
            Assert.Throws<ArgumentNullException> (() => new Command<int> (2,exeAction, null));
            Assert.Throws<ArgumentNullException> (() => new Command<int> (3,null, undoAction));
        }

        [Test ()]
        public void TestExecute ()
        {
            var list = new List<int> ();
            Action<int> exeAction = list.Add;
            Action<int> undoAction = (v) => list.Remove(v);

            var command = new Command<int>(2,exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (list.Count,1);
            Assert.AreEqual (list [0], 2);

            command.Undo ();
            Assert.AreEqual (list.Count,0);
        }
    }
}

