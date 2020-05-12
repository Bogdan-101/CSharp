using System;

namespace TestSolution.Properties
{
    public class GetPrintEventArgs : EventArgs            //Класс для передачи параметров
    {
        public GetPrintEventArgs(string info)            //Конструктор
        {
            Info = info;
        }
        
        public string Info { get; private set; }        
    }
}