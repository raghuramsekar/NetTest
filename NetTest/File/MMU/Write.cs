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
        public void MMUWrite(int noOfPage, int noOfCharacters,MemoryMappedFile mmf)
        {
            int pageSize = Environment.SystemPageSize;

            // Create a memory-mapped file
            for(int i = 0; i < noOfPage; i++)
            {
                string datStr = DataGenerator.GenerateRandomString(noOfCharacters);
                byte[] data = Encoding.UTF8.GetBytes(datStr);
                // Create a view accessor for the first page (4 KB)
                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(i*pageSize+1, pageSize))
                {
                    accessor.WriteArray(0, data, 0, noOfCharacters);
                }
            }
        }

        public void MMUWriteSpecificExisting(int page,int noOfCharacters, MemoryMappedFile mmf)
        {
            int pageSize = Environment.SystemPageSize;

            string datStr = DataGenerator.GenerateRandomString(noOfCharacters);
            byte[] data = Encoding.UTF8.GetBytes(datStr);
                
            using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(page * pageSize, pageSize))
            {
                // Write data into the memory-mapped file page
                accessor.WriteArray(0, data, 0, noOfCharacters);
            }
        }
    }
}
