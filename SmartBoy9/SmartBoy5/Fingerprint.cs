﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SmartBoy
{
    public class Fingerprint
    {
        StringUtil tools = new StringUtil();
        string output;

        public void CreateFingerprintv2()
        {
            Console.WriteLine("Fingerprint | CreateFingerprint | Initializing...");
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "fpcalc.exe";
            p.StartInfo.Arguments = " \"" + CurrentSongData.filePath + "\"";
            p.Start();

            // Read the output stream first and then wait.
            output = p.StandardOutput.ReadToEnd();

            CurrentSongData.duration = tools.getBetween(output, "DURATION=", "FINGERPRINT=");
            CurrentSongData.fingerprint = tools.getEnd(output, "FINGERPRINT=");

            Console.WriteLine("Fingerprint | CreateFingerprint | Finalizing...");

            // Data Logs.
            Console.WriteLine("Fingerprint | CreateFingerprint | Fingerprint: " + CurrentSongData.fingerprint);
            Console.WriteLine("Fingerprint | CreateFingerprint | Duration: " + CurrentSongData.duration);

            p.WaitForExit();
        }        
    }
}
