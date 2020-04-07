using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class TransferModule
    {
        public static string MAIN_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "remote-work");
        public static string SCREENSHOTS_DIR = Path.Combine(MAIN_DIR, "screenshots");
        public static string ACTIVITY_DIR = Path.Combine(MAIN_DIR, "activity");
        public static string CAM_DIR = Path.Combine(MAIN_DIR, "cam");
        public static string API_URL = "http://localhost:5000/api/rwk";

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
            timer.Interval = setting.DATA_TRANSFER_CYCLE * 1000 * 60;
            timer.Elapsed += Timer_Tick;            
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(SCREENSHOTS_DIR));
                foreach (string file in files)
                {
                    if (file.EndsWith(".rwk"))
                    {
                        await Upload("screenshots", file);
                        File.Delete(file);
                        Console.WriteLine("Screen-shot file '" + file + "' sent.");
                    }
                }

                files = new List<string>(Directory.EnumerateFiles(ACTIVITY_DIR));
                foreach (string file in files)
                {
                    if (file.EndsWith(".rwk"))
                    {
                        await Upload("activity", file);
                        File.Delete(file);
                        Console.WriteLine("Activity-log file '" + file + "' sent.");
                    }
                }

                files = new List<string>(Directory.EnumerateFiles(CAM_DIR));
                foreach (string file in files)
                {
                    if (file.EndsWith(".rwk"))
                    {
                        await Upload("cam", file);
                        File.Delete(file);
                        Console.WriteLine("Camera-shot file '" + file + "' sent.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<System.IO.Stream> Upload(string type, string filePath)
        {
            string timestamp = Path.GetFileName(filePath);
            
            using (FileStream paramFileStream = File.OpenRead(filePath))
            {
                HttpContent fileStreamContent = new StreamContent(paramFileStream);
                using (var client = new HttpClient())
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent, "captureFile", "" + setting.USER_ID + "^" + type + "^" + timestamp);
                    var response = await client.PostAsync(API_URL + "/capture", formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    return await response.Content.ReadAsStreamAsync();
                }
            }
        }
    }
}
