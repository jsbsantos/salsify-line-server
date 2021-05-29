using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalsifyLineServer.Helpers;
using System.Collections.Generic;
using System.IO;

namespace SalsifyLineServerUnitTests
{
    [TestClass]
    public class FileIndexerTests
    {
        private FileIndexer _service;
        private static string lineTemplate = "This is text file line number {0}.";
        private static string fileName = "testFile.txt";

        public FileIndexerTests()
        {
            var lines = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                var text = string.Format(lineTemplate, i);
                lines.Add(text);
            }
            File.WriteAllLines(fileName, lines);

            _service = new FileIndexer(fileName);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void GetIndex_returns_valid_indexes()
        {
            var data = _service.GetIndex();

            Assert.AreEqual(5, data.Count);
        }
    }
}