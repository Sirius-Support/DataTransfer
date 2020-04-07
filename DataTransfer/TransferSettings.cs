using System;
using System.IO;

namespace DataTransfer
{
    public class TransferSettings
    {
        public int USER_ID { get; set; } = 0;
        public byte DATA_TRANSFER_CYCLE { get; set; } = 15;     // in minute

        public static TransferSettings readSettings()
        {
            var confFileName = "config.dat";
            TransferSettings ret = new TransferSettings();

            try
            {
                byte[] fileBytes = File.ReadAllBytes(confFileName);
                
                ret.USER_ID = (fileBytes[0] << 24) | (fileBytes[1] << 16) | (fileBytes[2] << 8) | fileBytes[3];
                ret.DATA_TRANSFER_CYCLE = fileBytes[14];
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            return ret;
        }
    }
}
