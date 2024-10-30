using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTest.File
{
    class Read
    {
        /**
         * NormalRead => 494millis - 209MB
         * MMURead => 1178millis - 209MB
         * MMUReadBuffered is way slower because there is no buffer
         */

        //No buffer read by byte so very slow.
        public void MMURead()
        {
            string filePath = @"D:\as.txt";

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, null, 0, MemoryMappedFileAccess.ReadWrite))
            {
                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                {
                    long fileSize = accessor.Capacity; // Get file size
                    Console.WriteLine($"Size of the file: {fileSize} bytes");

                    // Read content until end of file
                    for (long i = 0; i < fileSize; i++)
                    {
                        byte b = accessor.ReadByte(i); // Read each byte
                        //Console.Write((char)b); // Print as character
                    }
                    //Console.WriteLine(); // New line after reading all
                }
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        //Read via MMU using buffered by os page size*constant.
        //Used in scenarios of reading a page in an offset.
        public void MMUReadBuffered()
        {
            string filePath = @"D:\DataTable.json";
            int pageSize = Environment.SystemPageSize*100;  // Define page size (typically 4KB)
            
            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"The file '{filePath}' does not exist.");
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                // Open the memory-mapped file
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, null, 0, MemoryMappedFileAccess.ReadWrite))
                {
                    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                    {
                        // Get the file size
                        long fileSize = new FileInfo(filePath).Length;
                        int totalPages = (int)Math.Ceiling((double)fileSize / pageSize);

                        // Iterate over the file, page by page
                        for (int page = 0; page < totalPages; page++)
                        {
                            Console.WriteLine(page);
                            long offset = page * pageSize;

                            // Determine the size of the view to map
                            int size = (int)Math.Min(pageSize, fileSize - offset);  // Adjust for the last page

                            byte[] datt = new byte[size];
                            accessor.ReadArray(offset, datt, 0, size);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        //Read Specific page 
        public void MMUReadSpecific(int pageOffset, int noOfPages, MemoryMappedFile mmf)
        {
            int pageSize = Environment.SystemPageSize;  // Define page size (typically 4KB)

            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor(pageOffset*pageSize,pageSize))
                {
                    byte[] datt = new byte[pageSize];
                    accessor.ReadArray(0, datt, 0, pageSize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex);
            }
            watch.Stop();
            Console.WriteLine("Time taken to read "+watch.ElapsedMilliseconds);
        }

        //Used in scenario where sequential read is needed.
        public void NormalRead()
        {
            string filePath = @"D:\and.txt";

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"The file '{filePath}' does not exist.");
                return;
            }
            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                // Open the file and read it line-by-line
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                Console.WriteLine("An error occurred while reading the file:");
                Console.WriteLine(ex.Message);
            }
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}
