﻿using NetTest.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MMU mmu = new MMU();
            mmu.MMURead();
            mmu.MMUReadBuffered();
            mmu.NormalRead();
        }
    }
}
