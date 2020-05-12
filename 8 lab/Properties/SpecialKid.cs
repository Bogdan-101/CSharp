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
            return base.GetYearInfo() + ", " + _fac;
        }

        public event EventHandler<GetPrintEventArgs> GetPrint;        //Прототип события
        
        public override void PrintInfo()            //Специально взял метол принт, потому что он единственный что-ли может красиво подойти под событие, т.к. все остальные методы откуда-то наследуются, короче бред, а тут удобно
        {
            string buffer;        
            buffer = $"Студент {LastName} {Name} " + GetYearInfo() + $". Ростом вышел {Height}. \nПол: " +
                              GetSexInfo() + "\nФорма обучения: " + GetFormOfStudyInfo();

            if (GetPrint != null)            //Проверка на то, подписались ли мы на событие 
                GetPrint(this, new GetPrintEventArgs(buffer)); //Вызываем событие GetPrint
        }

        public delegate void ShowMessage(string message);        //Прототип делегата, его же и использую для анонимного метода

        public void PrintDelegateMessage(ShowMessage method)        //Сам делегат. ShowMessage - Делегат, method - Какой-то метод, который на самом деле является указателем на другой метод
        {
            ShowMessage AnOnYmUs2007 = delegate(string message)        //Анонимный метод
            {
                Console.WriteLine(message);
            };

            AnOnYmUs2007("Анонимный метод check - YES");        //Что мы посылаем в анонимный метод

            ShowMessage lyambda = (message) => Console.WriteLine(message);        //Лямбда-выражение
            
            method("Delegate check - YES");        //ну такой простой делегат, просто чтобы вставить тут находится. Показать работу
        }

    }
}