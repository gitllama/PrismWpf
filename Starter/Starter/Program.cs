using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BTCV
{

    class Program
    {
        static void Main(string[] args)
        {
            /*Splash*/
            //SplashForm.ShowSplash();



            /*動作環境チェック*/
            CheckEnv hoge = new CheckEnv();
            if(!hoge.ShowMessageBox("4.6",true)) return;

            //System.Diagnostics.Process.Start(@"", @"");

        }


    }

    class CheckEnv
    {
        int releaseKey = 0;
        bool is64 = false;
        Dictionary<string, int> ReleaseKeyList = new Dictionary<string, int>()
        {
            ["4.6.1"] = 394254,
            ["4.6"] = 393295,
            ["4.5.2"] = 379893,
            ["4.5.1"] = 378675,
            ["4.5"] = 378389
        };

        public CheckEnv()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    releaseKey = (int)ndpKey.GetValue("Release");
                }
            }
            is64 = System.Environment.Is64BitOperatingSystem;
        }

        public bool ShowMessageBox(string ver,bool bit)
        {
            string ret = "";
            if (ReleaseKeyList[ver] > releaseKey) ret += $".NET Framework Version {ver} 以降をインストールしてください\r\n";
            if (!is64) ret += $"64bit OSをご使用ください\r\n";

            if (ret != "")
            {
                MessageBox.Show(ret, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;

        }


    }
}
