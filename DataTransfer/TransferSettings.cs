using System;
using System.IO;

namespace DataTransfer
{
    public class TransferSettings
    {
        public byte DATA_TRANSFER_CYCLE { get; set; } = 15;     // in minute

        public static TransferSettings readSettings()
        {
            var confFileName = Path.Combine(TransferModule.MAIN_DIR, "tconf.cnf");
            TransferSettings ret = new TransferSettings();

            try
            {
                byte[] fileBytes = File.ReadAllBytes(confFileName);
                
                ret.DATA_TRANSFER_CYCLE = fileBytes[0];
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            return ret;
        }
    }
}
