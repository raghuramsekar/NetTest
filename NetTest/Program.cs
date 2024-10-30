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
            Write mmu = new Write();

            mmu.MMUWrite(1000, 1000);
            Read readMmu = new Read();
            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(@"D:\and.txt", FileMode.OpenOrCreate, null, 0, MemoryMappedFileAccess.ReadWrite);
            Task task1 = Task.Run(() => readMmu.MMUReadSpecific(10, 1, mmf));
            Task task2 = Task.Run(() => readMmu.MMUReadSpecific(35, 2, mmf));
            Task task3 = Task.Run(() => readMmu.MMUReadSpecific(190, 2, mmf));
            Task task4 = Task.Run(() => readMmu.MMUReadSpecific(980, 2, mmf));
            Task task5 = Task.Run(() => readMmu.MMUReadSpecific(756, 2, mmf));
            Task task6 = Task.Run(() => readMmu.MMUReadSpecific(845, 2, mmf));
            await Task.WhenAll(task1, task2, task3, task4, task5, task6);
            Thread.Sleep(5000);
        }
    }
}
