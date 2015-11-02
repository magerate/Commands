using System;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    class TestDummay5
    {
        public int Number = 0;
        public void Inc(int a,int b,int c,int d,int e)
        {
            Number = Number + a+b+c+d+e;
        }

        public void Dec(int a,int b,int c,int d,int e)
        {
            Number = Number - a-b-c-d-e;
        }
    }

    [TestFixture ()]
    public class Command5Test
    {
        [Test ()]
        public void TestExecute ()
        {
            var dummy = new TestDummay5 ();
            Action<int,int,int,int,int> exeAction = dummy.Inc;
            Action<int,int,int,int,int> undoAction = dummy.Dec;

            int a = 1;
            int b = 2;
            int c = 3;
            int d = 4;
            int e = 5;

            var command = new Command<int,int,int,int,int>(a,b,c,d,e,exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (dummy.Number,15);

            command.Undo ();
            Assert.AreEqual (dummy.Number, 0);

        }
    }
}

