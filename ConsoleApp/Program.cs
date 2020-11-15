using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using TestsGenerator;

namespace ConsoleApp
{
    class Program
    {
        private const int MAX_DEGREE_OF_PARALLELISM_READ = 4;
        private const int MAX_DEGREE_OF_PARALLELISM_GENERATE = 4;
        private const int MAX_DEGREE_OF_PARALLELISM_WRITE = 4;
        private const string TESTS_DESTINATION = "D:\\University\\GitRepos\\mpp-5sem-4\\Tests\\";
        private static readonly string[] FILES_DESTINATIONS = 
        {
            "D:\\University\\GitRepos\\mpp-5sem-3\\AssemblyParserLib\\AssemblyParser.cs",
            "D:\\University\\GitRepos\\mpp-5sem-2\\Faker\\Faker.cs",
            "D:\\University\\GitRepos\\mpp-5sem-2\\Faker\\GeneratorsManager.cs",
            "D:\\University\\GitRepos\\mpp-5sem-4\\TestsGenerator\\TestsGenerator.cs"
        };

        static void Main(string[] args)
        {
            var generator = new TestsGenerator.TestsGenerator();

            var loadSourceFile = new TransformBlock<string, string>(async path => 
            {
                Console.WriteLine($"Loading file ({path})...");

                using (var reader = new StreamReader(path))
                {
                    return await reader.ReadToEndAsync();
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MAX_DEGREE_OF_PARALLELISM_READ});

            var generateTests = new TransformManyBlock<string, TestClass>(sourceCode => 
            {
                Console.WriteLine("Generating test classes...");

                return generator.Generate(sourceCode);
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MAX_DEGREE_OF_PARALLELISM_GENERATE});

            var saveTestFile = new ActionBlock<TestClass>(async testClass => 
            {
                Console.WriteLine($"Saving {testClass.FileName} to {TESTS_DESTINATION}...");

                using (StreamWriter writer = File.CreateText(TESTS_DESTINATION + testClass.FileName))
                {
                    await writer.WriteAsync(testClass.SourceCode);
                }

                Console.WriteLine($"{testClass.FileName} was successfully saved.");
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MAX_DEGREE_OF_PARALLELISM_WRITE});

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            loadSourceFile.LinkTo(generateTests, linkOptions);
            generateTests.LinkTo(saveTestFile, linkOptions);

            FILES_DESTINATIONS.ToList().ForEach(dest => loadSourceFile.Post(dest));

            loadSourceFile.Complete();

            saveTestFile.Completion.Wait();
        }
    }
}
