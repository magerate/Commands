using System;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    class TestDummay4
    {
        public int Number = 0;
        public void Inc(int a,int b,int c,int d)
        {
            Number = Number + a+b+c+d;
        }

        public void Dec(int a,int b,int c,int d)
        {
            Number = Number - a-b-c-d;
        }
    }

    [TestFixture ()]
    public class Command4Test
    {
        [Test ()]
        public void TestExecute ()
        {
            var dummy = new TestDummay4 ();
            Action<int,int,int,int> exeAction = dummy.Inc;
            Action<int,int,int,int> undoAction = dummy.Dec;

            int a = 1;
            int b = 2;
            int c = 3;
            int d = 4;
                

            var command = new Command<int,int,int,int>(a,b,c,d,exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (dummy.Number,10);

            command.Undo ();
            Assert.AreEqual (dummy.Number, 0);

        }
    }
}

