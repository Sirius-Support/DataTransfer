using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;

namespace DataTransfer
{
    public class TransferModule
    {
        public static string MAIN_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "remote-work");

        Timer timer;
        TransferSettings setting;

        public TransferModule(TransferSettings setting)
        {
            this.setting = setting;
            initializeTimer();
        }

        public void startTransfer()
        {
            timer.Start();
        }
        public void stopTransfer()
        {
            timer.Stop();
        }
        private void initializeTimer()
        {
            timer = new Timer();
            timer.Interval = setting.DATA_TRANSFER_CYCLE * 1000;/* ///// * 60; */
            timer.Elapsed += Timer_Tick;            
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(MAIN_DIR));
                foreach (string file in files)
                {
                    if (file.EndsWith(".rwk"))
                    {
                        Console.WriteLine(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
