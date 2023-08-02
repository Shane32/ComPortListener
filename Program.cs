using System.IO.Ports;

Console.WriteLine("Enter COM port (default is COM3): ");
string? comPort = Console.ReadLine();

if (string.IsNullOrEmpty(comPort))
{
    comPort = "COM3";
}

using SerialPort serialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
serialPort.DataReceived += SerialPort_DataReceived;

try
{
    serialPort.Open();
}
catch (Exception ex)
{
    Console.WriteLine("Cannot open COM port: " + ex.Message);
    return;
}
Console.WriteLine("Listening on " + comPort  + " - press Ctrl-C to exit.");

Console.CancelKeyPress += (_, e) =>
{
    Console.WriteLine("Ctrl-C pressed. Closing COM port and exiting...");

    if (serialPort.IsOpen)
    {
        serialPort.Close();
    }

    Environment.Exit(0);
};

while (true)
{
    Thread.Sleep(1000);
}

void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
{
    while (serialPort.BytesToRead > 0)
    {
        var data = serialPort.ReadByte();
        Console.WriteLine($"Received: {data:X2}");
    }
}
