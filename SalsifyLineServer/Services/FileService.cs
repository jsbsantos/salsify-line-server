using Microsoft.Extensions.Configuration;
using SalsifyLineServer.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SalsifyLineServer.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly List<long> _fileLineIndex;

        public FileService(IConfiguration configuration, List<long> fileLineIndex)
        {
            _configuration = configuration;
            _fileLineIndex = fileLineIndex;
        }

        public async Task<string> GetLine(int index)
        {
            //if requested line is out of bounds return null
            if (index > _fileLineIndex.Count)
            {
                return null;
            }

            var fileName = _configuration.GetValue<string>("fname");

            //open the file for reading
            using (var fs = File.OpenRead(fileName))
            {
                //position file 'pointer'
                fs.Position = _fileLineIndex[index];
                using (var sr = new StreamReader(fs))
                {
                    //read until line break is found
                    return await sr.ReadLineAsync().ConfigureAwait(false);
                }
            }
        }
    }
}