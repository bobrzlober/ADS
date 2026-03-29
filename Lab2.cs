using System;

class Node
{
    public double Value;
    public Node Next;
    public Node Prev;
    
}
class Program
{
    static void Main()
    {
        Console.WriteLine("waiting for n:");
        int n = int.Parse(Console.ReadLine());
        int ListSize = n*2;
        Console.WriteLine($"List will be {ListSize} ellements\n1-position number: ");

        Node head = new Node();
        head.Value = double.Parse(Console.ReadLine());
        head.Next = null;
        head.Prev = null;

        Node tail = head;

        for (int i = 2; i <= ListSize; i++)
        {
            Console.WriteLine($"{i}-position number: ");
            Node newNode = new Node();
            newNode.Value = double.Parse(Console.ReadLine());
            newNode.Next = null;
            newNode.Prev = tail;
            
            tail.Next = newNode;
            tail = newNode;
        }
        Node left = head;
        Node right = tail;
        double result = 1;
        while (left != right && left.Prev != right)
        {
            result *= (left.Value - right.Value);
            left = left.Next.Next;
            right = right.Prev.Prev;
        }
        Console.WriteLine($"result: {result}");
        Node current = head;
        while(current != null)
        {
            Node next = current.Next;
            current.Next = null;
            current.Prev = null;
            current = next;
        }
        head = null;
        tail = null;
    }
}