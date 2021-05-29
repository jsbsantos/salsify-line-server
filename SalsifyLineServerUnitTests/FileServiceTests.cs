using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalsifyLineServer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SalsifyLineServerUnitTests
{
    [TestClass]
    public class FileServiceTests
    {
        private FileService _service;
        private static string lineTemplate = "This is text file line number {0}.";
        private static string fileName = "testFile.txt";

        public FileServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"fname", fileName}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var lines = new List<string>();
            var index = new List<long>();

            var encoder = new ASCIIEncoding();
            for (var i = 0; i < 5; i++)
            {
                var text = string.Format(lineTemplate, i);
                lines.Add(text);
                index.Add(encoder.GetBytes(text + Environment.NewLine).Length * i);
            }
            File.WriteAllLines(fileName, lines);

            _service = new FileService(configuration, index);
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
        public async Task GetLine_returns_valid_line()
        {
            var index = 2;
            var data = await _service.GetLine(index).ConfigureAwait(false);

            Assert.AreEqual(string.Format(lineTemplate, index), data);
        }

        [TestMethod]
        public async Task GetLine_returns_null_for_out_of_bounds()
        {
            var data = await _service.GetLine(1111).ConfigureAwait(false);

            Assert.IsNull(data);
        }
    }
}