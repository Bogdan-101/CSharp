using System;
using System.Linq;

namespace TestSolutionn
{
    public struct Number :IComparable<Number>, IEquatable<Number>
    {
        readonly int n;
        readonly int m;

        public Number(int n, int m)
        {
            this.n = n;
            this.m = m;
        }
        
        
        public static Number Parse(string str)
        {
            int n, m = 0;
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

            Number num = new Number(n, m);
            
            return num;
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

        public bool Equals(Number other)
        {
            return n == other.n && m == other.m;
        }

        public override string ToString()
        {
            return Convert.ToString(n) + "/" + Convert.ToString(m);
        }
        
        public static string operator + (Number a, Number b)
        {
            var c = new Number(a.n*b.m + b.n*a.m, b.m*a.m);
            
            return c.ToString();
        }
        
        public static string operator - (Number a, Number b)
        {
            var c = new Number(a.n*b.m - b.n*a.m, b.m*a.m);
            
            return c.ToString();
        }
        
        public static string operator * (Number a, Number b)
        {
            var c = new Number (a.n * b.n, a.m*b.m);

            return c.ToString();
        }
        
        public static string operator / (Number a, Number b)
        {
            var c = new Number(a.n*b.m, a.m*b.n);
            
            return c.ToString();
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
            
            var c = new Number(a.n * b.m, b.n * a.m);
                return c.n < c.m?(-1):1;
        }
        
        public static int operator > (Number a, Number b)
        {
            if (a == b)
            {
                return 0;
            }
            
            var c = new Number(a.n * b.m, b.n * a.m);
            return c.n > c.m?(-1):1;
        }
        
        public int CompareTo(Number b)
        {
            var c = new Number(n * b.m, b.n * m);

            return c.m.CompareTo(c.n);
        }
        
    }
}