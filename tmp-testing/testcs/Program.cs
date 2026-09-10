using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello from C# (netcoredbg)");
        int sum = 0;
        for (int i = 1; i <= 5; i++)
        {
            sum += i;
        }
        Console.WriteLine($"sum = {sum}");
    }
}
