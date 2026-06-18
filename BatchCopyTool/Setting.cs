using BatchCopyTool.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BatchCopyTool
{
    internal class Setting
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string[] TargetMachines { get; set; }
        public MachineAccount DefaultAccount { get; set; }

        const string SETTING_FILE = "setting.json";

        public static Setting Load()
        {
            Setting setting = null;
            try
            {
                var path = Path.Combine(
                    Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                    SETTING_FILE);
                string json = File.ReadAllText(path);
                setting = JsonSerializer.Deserialize<Setting>(json);
            }
            catch { }
            if (setting == null)
            {
                setting = new()
                {
                    //  initial setting parameters for testing, you can modify them as needed
                    SourcePath = @"D:\Source",
                    DestinationPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs",
                    TargetMachines = new string[] {
                        "192.168.10.1, 192.168.20.1~10, 192.168.30.1, 3, 5",
                        "192.168.40.10",
                        "192.168.50.10",
                        "192.168.60.10~20"
                    },
                    DefaultAccount = new MachineAccount
                    {
                        UserName = "user",
                        Password = "password",
                        DomainName = ""
                    }
                };
                setting.Save();
            }

            //  Decrypt password.
            if (!string.IsNullOrEmpty(setting.DefaultAccount.Password))
            {
                setting.DefaultAccount.RawPassword = EncryptionUtil.AesDecrypt(setting.DefaultAccount.Password);
            }
            return setting;
        }

        public void Save()
        {
            //  Encrypt password.
            if (!string.IsNullOrEmpty(this.DefaultAccount.RawPassword))
            {
                this.DefaultAccount.Password = EncryptionUtil.AesEncrypt(this.DefaultAccount.RawPassword);
            }
            try
            {
                var path = Path.Combine(
                    Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                    SETTING_FILE);
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch { }
        }
    }
}
