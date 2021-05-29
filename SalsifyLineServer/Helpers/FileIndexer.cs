using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SalsifyLineServer.Helpers
{
    public class FileIndexer
    {
        private readonly string _path;

        public FileIndexer(string path)
        {
            _path = path;
        }

        public List<long> GetIndex()
        {
            Console.WriteLine("Starting indexing... ");

            var time = new Stopwatch();
            time.Start();

            var list = new List<long>();
            using (var file = File.OpenRead(_path))
            {
                //add starting position '0' to line index
                list.Add(file.Position);
                int currentByte = 0;
                var fileLength = file.Length;

                //loop through until EOF
                while (currentByte != -1)
                {
                    //read until a line break is found and make sure not to index the last one (will be an empty line)
                    currentByte = file.ReadByte();
                    var position = file.Position;

                    if (currentByte == '\n' && position < fileLength)
                    {
                        //add the position to the index as the start for line N
                        list.Add(position);
                    }
                }
            }

            time.Stop();
            Console.WriteLine($"Indexing completed in {time.Elapsed}.");

            return list;
        }
    }
}