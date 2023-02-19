using SharpDX.XInput;
using System.IO.Ports;
using System.Text.Json;
using System.Text.Json.Serialization;

Controller con = new Controller(UserIndex.One);
SerialPort serialPort = new SerialPort { };

serialPort.PortName = "COM6";
serialPort.BaudRate = 115200;
serialPort.DtrEnable = false;
serialPort.Open();

//serialPort.DataReceived += SerialPort_DataReceived;



double maxTurn = .75;
double maxForward = 1.0;
int deadZone = 20;

while (true)
{
    var state = con.GetState();

    int x = (int)(GetStickInput(state.Gamepad.RightThumbX) * maxTurn); 
    int y = (int)(GetStickInput(state.Gamepad.LeftThumbY) * maxForward); 
    var spinny = Convert.ToInt32(state.Gamepad.LeftTrigger / 1.416);

    // if we're not spinning, we need to mix x and y
    if(spinny == 0 && (Math.Abs(x) > 0 || Math.Abs(y) > 0))
    {
        // First hypotenuse
        var z = Math.Sqrt(x * x + y * y);
        // angle in radians
        var rad = Math.Acos(Math.Abs(x) / z);
        // and in degrees
        var angle = rad * 180 / Math.PI;

        // Now angle indicates the measure of turn
        // Along a straight line, with an angle o, the turn co-efficient is same
        // this applies for angles between 0-90, with angle 0 the co-eff is -1
        // with angle 45, the co-efficient is 0 and with angle 90, it is 1
        var tcoeff = -1 + (angle / 90) * 2;
        var turn = tcoeff * Math.Abs(Math.Abs(y) - Math.Abs(x));
        turn = Math.Round(turn * 100) / 100;

        // And max of y or x is the movement
        var move = Math.Max(Math.Abs(y), Math.Abs(x));

        double left;
        double right;

        // First and third quadrant
        if ((x >= 0 && y >= 0) || (x < 0 && y < 0))
        {
            left = move;
            right = turn;
        }
        else
        {
            right = move;
            left = turn;
        }

        // Reverse polarity
        if (y < 0)
        {
            left = 0 - left;
            right = 0 - right;
        }

        // modify x and y values so they can be sent on the wire
        x = (int)left;
        y = (int)right;
    }

    var command = new Command
    {
        Spinney = spinny,
        X = (int)x, 
        Y = (int)y 
    };

    serialPort.Write(JsonSerializer.Serialize(command));

    Console.WriteLine(JsonSerializer.Serialize(command));
}

void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
{
    SerialPort sp = (SerialPort)sender;
    string indata = sp.ReadExisting();

    Console.WriteLine(indata);
}

int GetStickInput(short value)
{
    // 183.333 is 33000/180, 33000 is the max value from the thumb stick
    // while +/-180 is the max pwm value we give in the arduino code
    var calculated = Convert.ToInt32(value / 183.333);

    if (Math.Abs(calculated) < deadZone)
        return 0;
    else
        return calculated;
}


public class Command
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Spinney { get; set; }
}