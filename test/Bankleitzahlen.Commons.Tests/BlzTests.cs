using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Commons.Tests
{
    [TestFixture]
    public class BlzTests
    {
        [Test]
        public void Models_BLZ_Conversion()
        {
            BLZ blz = 12345;
            Assert.That((int)blz, Is.EqualTo(12345));
        }
    }
}
