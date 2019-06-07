using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace System_Moniter
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Greeting
            // Greeting
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Male);
            Talk("Welcome to this system moniter", VoiceGender.Male, 3);
            #endregion
            #region MoniterSet & other data
            // Start System Moniter
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter systemUpTime = new PerformanceCounter("System", "System Up Time");
            PerformanceCounter networkReceved = new PerformanceCounter("Network Interface", "Bytes Received/sec", "802.11n Wireless LAN Card");
            PerformanceCounter networkSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "802.11n Wireless LAN Card");
            PerformanceCounter readData = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            PerformanceCounter writeData = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            #region CPU Overload List
            List<string> CPU_Overload_List = new List<string>();
            CPU_Overload_List.Add("Warning: Your CPU is at a burning hot hundred percent!!!");
            CPU_Overload_List.Add("Warning: Holy crap your cpu is about to catch fire");
            CPU_Overload_List.Add("Warning: Take it easier on your cpu as it is working too hard!!!");
            CPU_Overload_List.Add("Warning: If you are overclocking be careful. If you are not, stop working your poor chip this hard!!!");
            CPU_Overload_List.Add("Warning: It is not healthy for your cpu to be at 100 percent.");
            CPU_Overload_List.Add("Warning: Your processor is gasping and wheezing for breath.");
            Random CPU_Overload_Message = new Random();
            #endregion
            #endregion
            #region data
            int currentCpuPercentage = (int)perfCpuCount.NextValue();
            int avaliableMem = (int)perfMemCount.NextValue();
            float upTime = systemUpTime.NextValue();
            int receveNet = (int)networkReceved.NextValue() / 1028;
            int sendNet = (int)networkSent.NextValue() / 1028;
            int readDisk = (int)readData.NextValue() / 1028;
            int writeDisk = (int)writeData.NextValue() / 1028;
            #endregion
            #region GUI
            // This will print the performance percentage over and over every 1 second.
            Console.WriteLine("CPU Load        : %{0}", currentCpuPercentage);
            Console.WriteLine("Memory avaliable: {0} MB", avaliableMem);
            Console.WriteLine("System Up Time  : {0} secs", upTime);
            Console.WriteLine("Network Receved : {0} MB", receveNet);
            Console.WriteLine("Network Sent    : {0} MB", sendNet);
            Console.WriteLine("Disk Read       : {0} MB", readDisk);
            Console.WriteLine("Disk Written    : {0} MB", writeDisk);
            int talkSpeed = 2;
            TimeSpan upTimeSpan = TimeSpan.FromSeconds(systemUpTime.NextValue());

            string upTimeNotifier = String.Format("Your system has been up for {0} days, {1} hours, {2} minuetes, and {3} seconds.", (int)upTimeSpan.TotalDays, (int)upTimeSpan.TotalHours, (int)upTimeSpan.TotalMinutes, (int)upTimeSpan.TotalSeconds);
            Talk(upTimeNotifier, VoiceGender.Male, 5);
            #endregion
            while (true)
            {

                #region data
                int currentCpuPercentage2 = (int)perfCpuCount.NextValue();
                int avaliableMem2 = (int)perfMemCount.NextValue();
                float upTime2 = systemUpTime.NextValue();
                int receveNet2 = (int)networkReceved.NextValue() / 1028;
                int sendNet2 = (int)networkSent.NextValue() / 1028;
                int readDisk2 = (int)readData.NextValue() / 1028;
                int writeDisk2 = (int)writeData.NextValue() / 1028;
                #endregion
                #region GUI
                // This will print the performance percentage over and over every 1 second.
                Console.WriteLine("CPU Load        : %{0}", currentCpuPercentage2);
                Console.WriteLine("Memory avaliable: {0} MB", avaliableMem2);
                Console.WriteLine("System Up Time  : {0} secs", upTime2);
                Console.WriteLine("Network Receved : {0} MB", receveNet2);
                Console.WriteLine("Network Sent    : {0} MB", sendNet2);
                Console.WriteLine("Disk Read       : {0} MB", readDisk2);
                Console.WriteLine("Disk Written    : {0} MB", writeDisk2);
                #endregion
                #region speech strings
                string cpuVoiceMessage = String.Format("The current cpu load is {0} percent", currentCpuPercentage2);
                string memVoiceMessage = String.Format("The current avaliable memory is {0} megabytes", avaliableMem);
                string cpuHundredMessage = CPU_Overload_List[CPU_Overload_Message.Next(6)];
                #endregion
                #region speech logic & messaging
                if (currentCpuPercentage2 >= 80)
                {
                    if (currentCpuPercentage2 == 100)
                    {
                        Talk(cpuHundredMessage, VoiceGender.Female, talkSpeed);
                    }
                    Talk(cpuVoiceMessage, VoiceGender.Male, 2);
                }
                
                if (avaliableMem <= 512)
                {
                    Talk(memVoiceMessage, VoiceGender.Male, 2);
                }
                #endregion
                Thread.Sleep(1000);
            }
        }
        public static void Talk(string message, VoiceGender voiceGender, int rate)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(voiceGender);
            synth.Rate = (rate);
            synth.Speak(message);
        }
    }
}