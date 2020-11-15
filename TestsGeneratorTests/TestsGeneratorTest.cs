using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestsGenerator.Tests
{
    [TestClass]
    public class TestsGeneratorTest
    {
        private TestsGenerator generator = new TestsGenerator();

        private const string REAL_CLASS_PATH = "D:\\University\\GitRepos\\mpp-5sem-2\\Faker\\GeneratorsManager.cs";
        private const string EMPTY_CLASS_PATH = "D:\\University\\GitRepos\\mpp-5sem-4\\TestsGeneratorTests\\EmptyClass.cs";
        private const string NON_PUBLIC_METHODS_CLASS_PATH = "D:\\University\\GitRepos\\mpp-5sem-4\\TestsGeneratorTests\\ClassWithoutPublicMethods.cs";
        private const string TWO_CLASSES_FILE_PATH = "D:\\University\\GitRepos\\mpp-5sem-4\\TestsGeneratorTests\\TwoClasses.cs";

        [TestMethod]
        public void Generate_EmptyClass_ReturnsNoTestClasses()
        {
            var sourceCode = File.ReadAllText(EMPTY_CLASS_PATH);

            var tests = generator.Generate(sourceCode);

            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void Generate_ClassWithoutPublicMethods_ReturnsNoTestClasses()
        {
            var sourceCode = File.ReadAllText(NON_PUBLIC_METHODS_CLASS_PATH);

            var tests = generator.Generate(sourceCode);

            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void Generate_NormalClass_ReturnsTestClass()
        {
            var sourceCode = File.ReadAllText(REAL_CLASS_PATH);

            var tests = generator.Generate(sourceCode);

            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual("GeneratorsManagerTests.cs", tests[0].FileName);
        }

        [TestMethod]
        public void Generate_FileWithTwoClasses_ReturnsTwoTestClasses()
        {
            var sourceCode = File.ReadAllText(TWO_CLASSES_FILE_PATH);

            var tests = generator.Generate(sourceCode);

            Assert.AreEqual(2, tests.Count);
        }
    }
}
