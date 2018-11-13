using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace parserDecimal.Parser
{
    class Computer
    {
        Operands operands = new Operands();//создание объекта класса операндс 
        Polish polish = new Polish();//создание объекта класса полишь
        Calculator calculator = new Calculator();//создание объекта класса калькулятор

        internal decimal Compute(string function, decimal value)
        {
            Thread.Sleep(1);
            //Проверка на валидность функции
            List<string> splitExpression = operands.returnSplitExpression(function.Replace(" ", ""));
            Queue<string> revpExpression = polish.returnPolish(splitExpression);

            decimal answer = calculator.calculate(revpExpression, value);//вычисление функции
            return answer;
        }
    }
}
