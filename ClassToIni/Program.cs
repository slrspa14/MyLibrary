using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Program
{
    private static void Main(string[] args)
    {
        var ini = new ClassToIni(new FileInfo(args"test.ini"));
    }
}
