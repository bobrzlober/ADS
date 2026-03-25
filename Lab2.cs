using System;

class Node
{
    public double Value;
    public Node Next;
    
}
class Program
{
    static void Main()
    {
        Console.WriteLine("waiting for n:");
        int n = int.Parse(Console.ReadLine());
        int ListSize = n*2;
        Console.WriteLine($"List will be {ListSize} ellements");
    }
}