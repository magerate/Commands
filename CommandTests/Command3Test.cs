using System;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    [TestFixture ()]
    public class Command3Test
    {
        [Test ()]
        public void TestExecute ()
        {
            var map = new Dictionary<int,int> ();
            Action<Dictionary<int,int>,int,int> exeAction = (m,k,v) => m.Add(k,v);
            Action<Dictionary<int,int>,int,int> undoAction = (m,k,v) => m.Remove(k);

            int key = 3;
            int value = 4;

            var command = new Command<Dictionary<int,int>,int,int>(map,key,value,exeAction, undoAction);
            command.Execute ();
            Assert.AreEqual (map.Count,1);
            Assert.True(map.ContainsKey(key));
            Assert.AreEqual (map [key], value);

            command.Undo ();
            Assert.AreEqual (map.Count, 0);

        }
    }
}

