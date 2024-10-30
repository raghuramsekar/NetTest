using NetTest.File;
using NetTest.File.MMU;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Read mmu = new Read();
            //mmu.MMURead();
            //mmu.MMUReadBuffered();
            //mmu.NormalRead();
            // Ensure the file exists with the correct size
            string filePath = @"D:\and.txt";
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Set the file size to at least one page (4 KB)
                fs.SetLength(1000 * (4096 + 1));
            }
            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.OpenOrCreate, null, 0, MemoryMappedFileAccess.ReadWrite);

            Write mmu = new Write();
            mmu.MMUWrite(1000, 1000,mmf);
            Read readMmu = new Read();
            Task task1 = Task.Run(() => readMmu.MMUReadSpecific(10, 1, mmf));
            Task task2 = Task.Run(() => readMmu.MMUReadSpecific(35, 2, mmf));
            Task task3 = Task.Run(() => readMmu.MMUReadSpecific(190, 2, mmf));
            Task task4 = Task.Run(() => readMmu.MMUReadSpecific(980, 2, mmf));
            Task task5 = Task.Run(() => readMmu.MMUReadSpecific(756, 2, mmf));
            Task task6 = Task.Run(() => readMmu.MMUReadSpecific(845, 2, mmf));
            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            //Thread.Sleep(5000);
            //mmf.Dispose();
            Write mmm = new Write();
            mmm.MMUWriteSpecificExisting(10, 140,mmf);
        }
    }
}
