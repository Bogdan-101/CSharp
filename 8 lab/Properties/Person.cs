using System;
using System.Collections;
using System.Security.AccessControl;
using System.Security.Cryptography;



namespace TestSolution.Properties
{
    public enum Sex
    {
        Female,
        Male
    }

    public interface IPerson
    {
        Sex GetSexInfo();
        bool ArmyOrNot();

        string GetYearInfo();
        
        public event EventHandler<GetPrintEventArgs> GetPrint;        //Прототип события
        
        public delegate void ShowMessage(string message);        //Прототип делегата
        public void PrintDelegateMessage(ShowMessage method);  //Прототип функции, которая будет выводить message для десонмтрации работы делегата
        
        
        void PrintInfo();
        string GetFormOfStudyInfo();
        
    }
    public abstract class Person : IPerson, IComparable
    {
        public readonly int _age;
        private int _personId;
        protected double Height;
        public readonly string Name;
        protected readonly string LastName;
        public int CompareTo(object o)
        {
            Person p = o as Person;
            if (p != null)
                return this.LastName.CompareTo(p.LastName);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
        private static int PersonCount;
        protected Sex sex;
        public Sex GetSexInfo()
        {
            return sex;
        }

        protected Person()
        {
        }

        protected Person(int age, double height, string name, string lastName, Sex sex)
        {
            this._age = age;
            this.Height = height;
            this.Name = name;
            this.LastName = lastName;
            _personId = ++PersonCount;

            this.sex = sex;
        }

        public virtual bool ArmyOrNot()
        {
            return sex == Sex.Male && _age < 27 && _age >= 18;
        }

        public string GetYearInfo()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<GetPrintEventArgs> GetPrint;
        public void PrintDelegateMessage(IPerson.ShowMessage method)
        {
            throw new NotImplementedException();
        }

        public void PrintInfo()
        {
            throw new NotImplementedException();
        }

        public string GetFormOfStudyInfo()
        {
            throw new NotImplementedException();
        }

        
    }
}