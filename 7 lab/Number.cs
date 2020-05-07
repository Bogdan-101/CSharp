using System;
using System.Linq;

namespace TestSolutionn
{
    public struct Number :IComparable<Number>
    {
        private int n;
        private int m;

        public Number(string str)
        {
            var isAfter = false;
            var nBuffer = "";
            var mBuffer = "";
            foreach (var t in str)
            {
                if (t != '/')
                {
                    if (isAfter == false)
                        nBuffer += t.ToString();
                    else
                        mBuffer += t.ToString();
                }
                else
                    isAfter = true;
            }

            if (nBuffer.Contains('-'))
            {
                nBuffer = nBuffer.Remove(0, 1);
                n = Convert.ToInt32(nBuffer);
                n *= (-1);
            }
            else
                n = Int32.Parse(nBuffer);
            
            m = Int32.Parse(mBuffer);
        }

        public static bool IsCorrect(string str)
        {
            var index = str.IndexOf('/');
            if (str == null || index == (-1))
                return false;
            foreach (var t in str)
            {
                if (t < 47 || t > 57)
                    return false;
            }

            if (string.IsNullOrEmpty(str.Remove(0, index + 1)))
                return false;
            char[] cArray = str.ToCharArray();
            var reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            str = reverse;
            
            index = str.IndexOf('/');
            if (string.IsNullOrEmpty(str.Remove(0, index + 1)))
                return false;
            
            return true;
        }

        public string Print()
        {
            return Convert.ToString(n) + "/" + Convert.ToString(m);
        }

        public static string operator + (Number a, Number b)
        {
            var c = new Number();

            var buffer = new Number {n=a.n, m=a.m };
            
            a.n *= b.m;
            a.m *= b.m;
            b.n *= buffer.m;
            b.m *= buffer.m;

            c.n = a.n + b.n;
            c.m = a.m;
            return c.Print();
        }
        
        public static string operator - (Number a, Number b)
        {
            var c = new Number();
            var buffer = new Number {n=a.n, m=a.m };
            
            a.n *= b.m;
            a.m *= b.m;
            b.n *= buffer.m;
            b.m *= buffer.m;

            c.n = a.n - b.n;
            c.m = a.m;
            return c.Print();
        }
        
        public static string operator * (Number a, Number b)
        {
            var c = new Number {n = a.n * b.n, m = a.m*b.m};

            return c.Print();
        }
        
        public static string operator / (Number a, Number b)
        {
            var buffer = b.m;
            b.m = b.n;
            b.n = buffer;

            Number c;
            c.n = a.n * b.n;
            c.m = a.m * b.m;
            return c.Print();
        }
        
        public static bool operator != (Number a, Number b)
        {
            return (a.n != b.n) || (a.m != b.m);
        }
        
        public static bool operator == (Number a, Number b)
        {
            return (a.n == b.n) && (a.m == b.m);
        }
        
        public static int operator < (Number a, Number b)
        {
            if (a == b)
            {
                return 0;
            }
            
            var buffer = new Number {n=a.n, m=a.m };
            
            a.n *= b.m;
            a.m *= b.m;
            b.n *= buffer.m;
            b.m *= buffer.m;

            return a.n < b.n?(-1):1;
        }
        
        public static int operator > (Number a, Number b)
        {
            if (a == b)
            {
                return 0;
            }
            
            var buffer = new Number {n=a.n, m=a.m };
            
            a.n *= b.m;
            a.m *= b.m;
            b.n *= buffer.m;
            b.m *= buffer.m;

            return a.n > b.n?(-1):1;
        }
        
        public int CompareTo(Number b)
        {
            var buffer = new Number {n=this.n, m=this.m };
            
            n *= b.m;
            m *= b.m;
            b.n *= buffer.m;
            b.m *= buffer.m;

            return n.CompareTo(b.n);
            
        }
        
    }
}