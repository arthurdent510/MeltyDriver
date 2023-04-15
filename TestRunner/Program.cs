using SharpDX.XInput;

Controller con = new Controller(UserIndex.One);
int deadZone = 20;


//var x = 89;
//var y = 50;

//var c = Math.Sqrt(y* y + x * x);
//double foo = Math.Asin(x / c);

////Math.PI / 180 * deg

//double deg = 180 / Math.PI * foo;

var currentHeading = 0;
var heading = 0;
var magnitude = 90;

var modTest = 143 % 360;
Console.WriteLine(modTest);

//while (true)
//{
//    currentHeading += 5;

//    if (Math.Abs(heading + 90 - currentHeading) < magnitude)
//    {
//        Console.WriteLine($"Current:{currentHeading}, heading:{heading}");
//    }
//    else
//    {
//        Console.WriteLine("bar");
//    }

//    Thread.Sleep(500);
//}

//Console.WriteLine(deg);


//while (true)
//{
//    var state = con.GetState();

//    int x = (int)GetStickInput(state.Gamepad.RightThumbX);
//    int y = (int)GetStickInput(state.Gamepad.RightThumbY);
//    var spinny = Convert.ToInt32(state.Gamepad.LeftTrigger / 2.55);

//    // let's calculate the angle that it's going to... probably some right triangle math?

//    var c = Math.Sqrt(y * y + x * x);
//    double foo = Math.Asin(x / c);
//    double deg = 180 / Math.PI * foo;
//    double outdeg = 0;

//    var magnatiude = Math.Max(Math.Abs(x), Math.Abs(y));

//    if (!double.IsNaN(deg))
//    { 
//    // figure out what quadrant we're in so we get the correct angle
//        if (x >= 0 && y >= 0)
//            outdeg = deg;
//        else if (x >= 0 && y < 0)
//            outdeg = 180 - Math.Abs(deg);
//        else if (x < 0 && y < 0)
//            outdeg = 180 + Math.Abs(deg);
//        else
//            outdeg = 360 - Math.Abs(deg);
//    }

//    //Console.WriteLine($"x:{x}, y:{y}, out deg: {outdeg:0}, magnitude: {magnatiude}");
//    Console.WriteLine($"spinney: {spinny}");
//}

int GetStickInput(short value)
{
    // 183.333 is 33000/180, 33000 is the max value from the thumb stick
    // while +/-180 is the max pwm value we give in the arduino code
    //var calculated = Convert.ToInt32(value / 183.333);


    var calculated = Convert.ToInt32(value / 366.666); // we need the value to be from 0 to 180.  This gives us -90 to 90, we'll translate that later

    if (Math.Abs(calculated) < deadZone)
        return 0;
    else
        return calculated;
}