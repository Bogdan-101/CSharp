using System;
using System.Net.Mime;
using System.Text;

namespace TestSolution.Properties
{
    public class Student : Person, IPerson
    {
        private readonly StudentInfo _info;

        protected Student(int age, double height, string name, string lastName, string university, int year,
            bool formOfStudy, Sex sex) : base(
            age, height, name, lastName, sex)
        {
            _info.University = university;
            _info.Year = year;
            _info.FormOfStudy = formOfStudy;
        }

        private struct StudentInfo
        {
            public string University;
            public int Year;
            public bool FormOfStudy;
        }

        public override bool ArmyOrNot()
        {
            if (base.ArmyOrNot())
            {
                if (_info.Year != 0)
                    return false;
                return true;
            }

            return false;
        }

        protected virtual string GetYearInfo()
        {
            if (_info.Year == 0)
            {
                return " вовсе не студент.";
            }

            //return $"учится на {Info.year} курсе в {Info.university}";

            return _info.Year == 0 ? " вовсе не студент" :  $"учится на {_info.Year} курсе в {_info.University}"; //- Почему не работает?
        }

        protected string GetFormOfStudyInfo()
        {
            if (_info.FormOfStudy)
                return " Платная.";
            else
                return " Бюджетная.";
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Студент {LastName} {Name} " + GetYearInfo() + $" ростом {Height}. \nПол: {sex}"+ "\nФорма обучения: " + GetFormOfStudyInfo());
        }
    }
}