using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parserDecimal.Parser
{
    class Derivative
    {
        Operands operands = new Operands();
        Polish polish = new Polish();
        Calculator calculator = new Calculator();
        Derivs derivative = new Derivs();
        Simplify simplify = new Simplify();

        internal string ReturnDerivative(string function)
        {
            //Проверка на валидность
            List<string> splitExpression = operands.returnSplitExpression(function.Replace(" ","").ToLower());//проверка на операнды удаление пробелов и перевод в нижний регистр
            Queue<string> revpExpression = polish.returnPolish(splitExpression);

            string deriv = derivative.GetDerivative(revpExpression.ToList());//нахождение производной функции 

            List<string> splitDerivative = operands.returnSplitExpression(deriv);//проверка на валидность производной (операндов)
            Queue<string> revpDerivative = polish.returnPolish(splitDerivative);//проверка на валидность производной
            deriv = simplify.NoBraces(revpDerivative);//упрощение производнойы
            return deriv;
        }
    }
}
