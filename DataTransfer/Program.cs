using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class Program
    {
        static void Main(string[] args)
        {
            TransferSettings settings = TransferSettings.readSettings();
            TransferModule transferModule = new TransferModule(settings);
            transferModule.startTransfer();
            System.Console.ReadKey();
        }
    }
}
