using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace TestSolutionn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a:");
            var a = Console.ReadLine();

            while (!Number.IsCorrect(a))
            {
                Console.WriteLine("Error, try again");
                a = Console.ReadLine();
            }
            
            Console.WriteLine("Now enter b:");
            var b = Console.ReadLine();
            while (!Number.IsCorrect(b))
            {
                Console.WriteLine("Error, try again");
                b = Console.ReadLine();
            }
            var A = new Number();
            var B = new Number();
            A = Number.Parse(a);
            B = Number.Parse(b);
            Console.WriteLine($"a + b = {A + B}");
            Console.WriteLine($"a - b = {A - B}");
            Console.WriteLine($"a * b = {A * B}");
            Console.WriteLine($"a / b = {A / B}");
            var op1 = A == B ? "==" : "!=";
            Console.WriteLine($"a{op1}b");
            var op2 = (A > B)==(-1) ? ">" : (A>B)==(1)?"<":"==";
            Console.WriteLine($"a{op2}b");
            
            //Сейчас сделаем массив из чисел, чтобы продемонстрировать работу IComparable:

            
            Number C=new Number(3, 8);
            Number D=new Number(6, 8);
            Number[] objectList = new Number[] {A, B, C, D};

            Array.Sort(objectList);
            foreach (var t in objectList)
            {
                Console.WriteLine(t.ToString());
            }
            
        }
    }
}
