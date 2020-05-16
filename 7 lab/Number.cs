using System;
using System.Linq;

namespace TestSolutionn
{
    public struct Number :IComparable<Number>, IEquatable<Number>
    {
        readonly int n;
        readonly int m;
        private int nBuffer;
        private int mBuffer;

        public Number(int n, int m)
        {
            this.n = n;
            this.nBuffer = n;
            this.m = m;
            this.mBuffer = m;
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
            return Convert.ToString(nBuffer) + "/" + Convert.ToString(mBuffer);
        }


        public string Print()        //Ну для удобства я оставил Print(), для наглядности, но всё же переопределил стандартный ToString()
        {
            return ToString();
        }

        public static string operator + (Number a, Number b)
        {
            var c = new Number();

            var buffer = new Number {nBuffer=a.n, mBuffer=a.m };
            
            a.nBuffer *= b.mBuffer;
            a.mBuffer *= b.mBuffer;
            b.nBuffer *= buffer.mBuffer;
            b.mBuffer *= buffer.mBuffer;

            c.nBuffer = a.nBuffer + b.nBuffer;
            c.mBuffer = a.mBuffer;
            return c.Print();
        }
        
        public static string operator - (Number a, Number b)
        {
            var c = new Number();
            var buffer = new Number {nBuffer=a.n, mBuffer=a.m };
            
            a.nBuffer *= b.mBuffer;
            a.mBuffer *= b.mBuffer;
            b.nBuffer *= buffer.mBuffer;
            b.mBuffer *= buffer.mBuffer;

            c.nBuffer = a.nBuffer - b.nBuffer;
            c.mBuffer = a.mBuffer;
            return c.Print();
        }
        
        public static string operator * (Number a, Number b)
        {
            var c = new Number {nBuffer = a.n * b.n, mBuffer = a.m*b.m};

            return c.Print();
        }
        
        public static string operator / (Number a, Number b)
        {
            var buffer = b.m;
            b.mBuffer = b.n;
            b.nBuffer = buffer;

            var c = new Number {nBuffer = 1, mBuffer = 1};
            c.nBuffer = a.nBuffer * b.nBuffer;
            c.mBuffer = a.mBuffer * b.mBuffer;
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
            
            var buffer = new Number {nBuffer=a.n, mBuffer=a.m };
            
            a.nBuffer *= b.m;
            a.mBuffer *= b.m;
            b.nBuffer *= buffer.mBuffer;
            b.mBuffer *= buffer.mBuffer;

            return a.nBuffer < b.nBuffer?(-1):1;
        }
        
        public static int operator > (Number a, Number b)
        {
            if (a == b)
            {
                return 0;
            }
            
            var buffer = new Number {nBuffer=a.n, mBuffer=a.m };
            
            a.nBuffer *= b.m;
            a.mBuffer *= b.m;
            b.nBuffer *= buffer.mBuffer;
            b.mBuffer *= buffer.mBuffer;

            return a.nBuffer > b.nBuffer?(-1):1;
        }
        
        public int CompareTo(Number b)
        {
            var buffer = new Number {nBuffer=this.n, mBuffer=this.m };
            
            nBuffer *= b.m;
            mBuffer *= b.m;
            b.nBuffer *= buffer.m;
            b.mBuffer *= buffer.m;

            return n.CompareTo(b.n);
            
        }
        
    }
}
