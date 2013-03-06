using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO.Ports;
using System.Threading;

namespace XStick_Config
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
            List<string> configuredPortNames = new List<string>();
            while (true)
            {
                foreach (string portName in SerialPort.GetPortNames())
                {
                    if (!configuredPortNames.Contains(portName))
                    {
                        configuredPortNames.Add(portName);
                        Console.Write("Configuring XStick on " + portName + "...");
                        try
                        {
                            SerialPort serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                            //serialPort.Handshake = Handshake.RequestToSend;
                            //serialPort.DtrEnable = true;
                            serialPort.Open();
                            Thread.Sleep(1100);
                            serialPort.Write("+++");    // enter command mode
                            Thread.Sleep(1100);
                            serialPort.Write("ATRE\r");  // restore defaults
                            Thread.Sleep(100);
                            serialPort.Write("ATDLFFFF\r"); // destination address = 0xFFFF (broadcast to all addresses)
                            Thread.Sleep(100);
                            serialPort.Write("ATBD7\r");  // Baud = 115200
                            Thread.Sleep(100);
                            serialPort.Write("ATD55\r");  // D5 = output high
                            Thread.Sleep(100);
                            serialPort.Write("ATGT64\r");  // guard time = 200 ms
                            Thread.Sleep(100);
                            serialPort.Write("ATWR\r");   // save settings
                            Thread.Sleep(100);
                            serialPort.Write("ATFR\r");   // software reset
                            serialPort.Close();
                            Console.WriteLine("Compelte.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Failed (" + e.Message + ")");
                        }
                    }
                }
            }
        }
    }
}
