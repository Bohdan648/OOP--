using System;
using System.Text;

namespace lab5v5;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Самостійна робота №3 (lab5v5) ===\n");

        BitCollection bits1 = new BitCollection(new bool[] { true, false, true, true });
        BitCollection bits2 = new BitCollection(4);

        bits2[0] = false;
        bits2[1] = true;
        bits2[2] = true;
        bits2[3] = false;

        Console.WriteLine($"bits1: {bits1}");
        Console.WriteLine($"bits2: {bits2}");

        Console.WriteLine($"\nbits1[1] до зміни: {bits1[1]}");
        bits1[1] = true;
        Console.WriteLine($"bits1[1] після зміни: {bits1[1]}");
        Console.WriteLine($"Оновлений bits1: {bits1}");

        BitCollection andResult = bits1 & bits2;
        Console.WriteLine($"\nbits1 & bits2 = {andResult}");

        BitCollection orResult = bits1 | bits2;
        Console.WriteLine($"bits1 | bits2 = {orResult}");

        Console.WriteLine($"\nbits1 == bits2: {bits1 == bits2}");
        Console.WriteLine($"bits1 != bits2: {bits1 != bits2}");
    }
}