/*
This is under the MIT License
Copyright 2019 Jeremiah Haven

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files 
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Libraries:
*/
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
            
            // Generate the Speech Synthesis
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Male);
            
            // Welcome the User
            Talk("Welcome to this system moniter", VoiceGender.Male, 3);
            #endregion
            #region MoniterSet & other data
            
            // Create the moniters and put them in variables so that we can easily use them later
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter systemUpTime = new PerformanceCounter("System", "System Up Time");
            PerformanceCounter networkReceved = new PerformanceCounter("Network Interface", "Bytes Received/sec", "802.11n Wireless LAN Card");
            PerformanceCounter networkSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "802.11n Wireless LAN Card");
            PerformanceCounter readData = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            PerformanceCounter writeData = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            #region CPU Overload List
            
            // Create a list of messages for the program to use on maxed out cpu
            List<string> CPU_Overload_List = new List<string>();
            CPU_Overload_List.Add("Warning: Holy crap your cpu is about to catch fire");
            CPU_Overload_List.Add("Warning: Your cpu is maxed out");
            
            // Call a random listing
            Random CPU_Overload_Message = new Random();
            #endregion
            #endregion
            #region data
            
            // Retrieve info from the moniters and put them into variables
            int currentCpuPercentage = (int)perfCpuCount.NextValue();
            int avaliableMem = (int)perfMemCount.NextValue();
            float upTime = systemUpTime.NextValue();
            int receveNet = (int)networkReceved.NextValue() / 1028;
            int sendNet = (int)networkSent.NextValue() / 1028;
            int readDisk = (int)readData.NextValue() / 1028;
            int writeDisk = (int)writeData.NextValue() / 1028;
            #endregion
            #region Interface
            
            // This will print the performance percentage over and over every 1 second.
            Console.WriteLine("CPU Load        : %{0}", currentCpuPercentage);
            Console.WriteLine("Memory avaliable: {0} MB", avaliableMem);
            Console.WriteLine("System Up Time  : {0} secs", upTime);
            Console.WriteLine("Network Receved : {0} MB", receveNet);
            Console.WriteLine("Network Sent    : {0} MB", sendNet);
            Console.WriteLine("Disk Read       : {0} MB", readDisk);
            Console.WriteLine("Disk Written    : {0} MB", writeDisk);
            
            // Set the speed
            int talkSpeed = 2;
            TimeSpan upTimeSpan = TimeSpan.FromSeconds(systemUpTime.NextValue());
            
            // Tell the user the cpu uptime
            string upTimeNotifier = String.Format("Your system has been up for {0} days, {1} hours, {2} minuetes, and {3} seconds.", (int)upTimeSpan.TotalDays, (int)upTimeSpan.TotalHours, (int)upTimeSpan.TotalMinutes, (int)upTimeSpan.TotalSeconds);
            Talk(upTimeNotifier, VoiceGender.Male, 5);
            #endregion
            
            // Do the above in a loop
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
                #region Interface
                Console.WriteLine("CPU Load        : %{0}", currentCpuPercentage2);
                Console.WriteLine("Memory avaliable: {0} MB", avaliableMem2);
                Console.WriteLine("System Up Time  : {0} secs", upTime2);
                Console.WriteLine("Network Receved : {0} MB", receveNet2);
                Console.WriteLine("Network Sent    : {0} MB", sendNet2);
                Console.WriteLine("Disk Read       : {0} MB", readDisk2);
                Console.WriteLine("Disk Written    : {0} MB", writeDisk2);
                #endregion
                #region speech strings
                    
                // Create strings for the speech synthesis
                string cpuVoiceMessage = String.Format("The current cpu load is {0} percent", currentCpuPercentage2);
                string cpuHundredMessage = CPU_Overload_List[CPU_Overload_Message.Next(6)];
                #endregion
                #region speech logic & messaging
                
                // If the CPU Load is over or equal to 80%, alert the user
                if (currentCpuPercentage2 >= 80)
                {
                    
                    // If the CPU Load is 100%, alert the user
                    if (currentCpuPercentage2 == 100)
                    {
                        Talk(cpuHundredMessage, VoiceGender.Female, talkSpeed);
                    }
                    Talk(cpuVoiceMessage, VoiceGender.Male, 2);
                }
                #endregion
                    
                // Update Every Second
                Thread.Sleep(1000);
            }
        }
        
        // Talk void so that we don't have to copy multiple lines of code for use in multiple cases.
        public static void Talk(string message, VoiceGender voiceGender, int rate)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(voiceGender);
            synth.Rate = (rate);
            synth.Speak(message);
        }
    }
}
