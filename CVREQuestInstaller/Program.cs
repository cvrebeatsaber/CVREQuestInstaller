using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CVREQuestInstaller
{
    internal class Program
    {
        private const string Url = "https://github.com/cvrebeatsaber/QuestQualifications/releases/download/v0.1.0/QuestCVREPlugin_v0.1.0.zip";

        private static void Unzip(string path, string folder) => ZipFile.ExtractToDirectory(path, folder);

        private static async Task Main(string[] args)
        {
            try
            {
                string? adbExe = null;
                if (args.Length > 1)
                {
                    adbExe = args[1];
                }
                Console.WriteLine("Downloading...");
                var client = new WebClient();
                await client.DownloadFileTaskAsync(Url, "QuestMod.zip");
                Console.WriteLine("Unzipping...");
                Unzip("QuestMod.zip", "QuestMod");
                Console.WriteLine("Finding adb...");
                if (adbExe is null)
                {
                    adbExe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SideQuest", "platform-tools", "adb.exe");
                }
                while (!File.Exists(adbExe))
                {
                    // Ask user for adb path
                    Console.WriteLine("Drag and drop your adb.exe (typically under: %appdata%/SideQuest/platform-tools) onto this window and press enter:");
                    adbExe = Console.ReadLine();
                }
                if (adbExe is null)
                    throw new ArgumentException("Must provide valid adb file!");
                Console.WriteLine("Found adb: " + adbExe);
                Console.WriteLine("Pushing files...");
                Process.Start(adbExe, "push QuestMod/libbeatsaber-hook_0_7_1.so /sdcard/Android/data/com.beatgames.beatsaber/files/libs/libbeatsaber-hook_0_7_1.so");
                Process.Start(adbExe, "push QuestMod/libcodegen_0_2_6.so /sdcard/Android/data/com.beatgames.beatsaber/files/libs/libcodegen_0_2_6.so");
                Process.Start(adbExe, "push QuestMod/libcustom-types_0_2_5.so /sdcard/Android/data/com.beatgames.beatsaber/files/libs/libcustom-types_0_2_5.so");
                Process.Start(adbExe, "push QuestMod/libquestui.so /sdcard/Android/data/com.beatgames.beatsaber/files/libs/libquestui.so");
                Process.Start(adbExe, "push QuestMod/libCVREPlugin_0_1_0.so /sdcard/Android/data/com.beatgames.beatsaber/files/mods/libCVREPlugin_0_1_0.so");
                Console.WriteLine("Complete! Press any key to finish");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}