using NetTest.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTest.File.MMU
{
    class Write
    {
        public void MMUWrite(int noOfPage, int noOfCharacters)
        {
            const int pageSize = 4096;

            // Path to the file to map
            string filePath = @"D:\and.txt";

            // Ensure the file exists with the correct size
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Set the file size to at least one page (4 KB)
                fs.SetLength(pageSize*(noOfPage+1));
            }

            // Create a memory-mapped file
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open))
            {
                for(int i = 0; i < noOfPage; i++)
                {
                    string datStr = DataGenerator.GenerateRandomString(noOfCharacters);
                    byte[] data = Encoding.UTF8.GetBytes(datStr);
                    // Create a view accessor for the first page (4 KB)
                    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(i*pageSize+1, pageSize))
                    {
                        //Console.WriteLine(datStr);
                        // Write data into the memory-mapped file page
                        accessor.WriteArray(0, data, 0, noOfCharacters);
                    }
                }
                
            }

            FileInfo fileInfo = new FileInfo(filePath);
            long fileSizeInBytes = fileInfo.Length;
            Console.WriteLine("File size is " + fileSizeInBytes);
        }

        public void ReadSize()
        {

        }
    }
}
