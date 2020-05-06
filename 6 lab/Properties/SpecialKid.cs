using System;

namespace TestSolution.Properties
{
    public class SpecialKid: Student, IPerson
    {
        private readonly string _fac;

        public SpecialKid(int age, double height, string name, string lastName, string university, int year,
            string fac, bool formOfStudy, Sex sex
            ) : base(age, height, name, lastName, university, year, formOfStudy,
            sex)
        {
            this._fac = fac;
        }
        
        protected override string GetYearInfo()
        {
            return base.GetYearInfo() + ",  " + _fac;
        }

        
        public override void PrintInfo()
        {
            Console.WriteLine($"Студент {LastName} {Name} " + GetYearInfo() + $". Ростом вышел {Height}. \nПол: " +
                              GetSexInfo() + "\nФорма обучения: " + GetFormOfStudyInfo());
        }
    }
}