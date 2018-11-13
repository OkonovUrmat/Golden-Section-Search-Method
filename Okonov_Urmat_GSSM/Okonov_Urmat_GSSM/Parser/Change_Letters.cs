using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okonov_Urmat_Bisection
{
    public class Change_Letters
    {
       public string Function_Verify(string funcBox)
        {
            bool FirstLetter = false;
            char symbol = ' ';
            funcBox = funcBox.ToLower();
            string function = "";
            List<string> standartOperators = new List<string> {
                "sqrt", "sin", "cos", "tan",
                "atan", "acos", "asin", "acotan",
                "exp", "ln", "log",
                "sinh", "cosh", "tanh", "abs",
                "ceil", "floor", "fac", "sfac", "round", "fpart"
            };


            int length = funcBox.Length;

            for (int i = 0; i < funcBox.Length; i++)
            {
                if (char.IsLetter(funcBox[i]) && i + 1 < funcBox.Length && char.IsLetter(funcBox[i + 1]))
                {
                    function = "";
                    while (i < length && char.IsLetter(funcBox[i]))
                    {
                        function += funcBox[i];
                        i++;
                    }
                    if (standartOperators.Find(x => x == function) == null)
                    {
                        return "";
                    }
                }
                else if (char.IsLetter(funcBox[i]))
                {
                    if (FirstLetter == false)
                    {
                        symbol = funcBox[i];
                        FirstLetter = true;
                    }
                    else if (symbol != funcBox[i])
                    {
                        return "";
                    }
                }
            }
            if (symbol == ' ')
            {
                return "";
            }
            funcBox = funcBox.Replace(symbol, 'x');
            return funcBox;
        }
    }
}
