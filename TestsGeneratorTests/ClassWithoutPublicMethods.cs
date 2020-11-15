using System;
using System.Collections.Generic;
using System.Text;

namespace TestsGeneratorTests
{
    class ClassWithoutPublicMethods
    {
        private void FirstMethod() { }

        protected void SecondMethod() { }

        internal void ThirdMethod() { }
    }
}
