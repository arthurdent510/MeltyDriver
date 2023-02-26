using System;
using System.IO.Ports;

namespace MeltyDriver.Service
{
    public static class SerialPortService
    {
        static SerialPort serialPort = new SerialPort { };

        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public static bool ConnectToPort(string portName)
        {
            try
            {
                serialPort.Dispose();

                serialPort = new SerialPort()
                {
                    PortName = portName,
                    BaudRate = 115200,
                    DtrEnable = false
                };

                serialPort.Open();

                serialPort.DataReceived += SerialPort_DataReceived;                
            }
            catch(Exception e)
            {
                // do something
                return false;
            }

            return true;
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            Console.Write(indata);
        }
    }
}
