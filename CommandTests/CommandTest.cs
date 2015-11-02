using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;
using Cession.Commands;

namespace CommandTests
{
    class PropertyDummy
    {
        public string Name{ get; set; }
    }

    [TestFixture ()]
    public class Test
    {
        [Test ()]
        public void TestCreateClear ()
        {
            var list = new List<int> (Enumerable.Range(1,10));
            var command = Command.CreateClear (list);

            command.Execute ();
            Assert.AreEqual (list.Count, 0);

            command.Undo ();
            Assert.AreEqual (list.Count, 10);
        }

        [Test ()]
        public void TestDictionaryAdd ()
        {
            var dic = new Dictionary<string,string> ();
            string strValue = "strValue";
            var command = Command.CreateDictionaryAdd (dic, "strKey",strValue);

            command.Execute ();
            Assert.AreEqual (dic.Count, 1);
            Assert.True (dic.ContainsKey ("strKey"));
            Assert.AreEqual (strValue, dic ["strKey"]);


            command.Undo ();
            Assert.AreEqual (dic.Count, 0);
        }

        [Test ()]
        public void TestSetProperty ()
        {
            var dummy = new PropertyDummy (){ Name = "someone" };

            var command = Command.CreateSetProperty (dummy, "anyone", "Name");
            command.Execute ();
            Assert.AreEqual (dummy.Name, "anyone");

            command.Undo ();
            Assert.AreEqual (dummy.Name, "someone");
        }
    }
}

