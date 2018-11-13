using System;
using System.IO;
using System.Collections.Generic;
using info.lundin.math;
using parserDecimal.Parser;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Okonov_Urmat_Bisection;

namespace Okonov_Urmat_GSSM
{
    public partial class GSSM : Form
    {
        decimal intervalBegin = 0;
        decimal intervalEnd = 0;
        decimal precision = 0;
        Stopwatch stopWatch = new Stopwatch();
        Stopwatch time = new Stopwatch();
        int init = 0;
        Computer computer = new Computer();
        Change_Letters c = new Change_Letters();
        string func = "";
        int k_max = 0;
        decimal max_time = 0;
        decimal f1, f2,n = 0.0M;

        ExpressionParser parser = new ExpressionParser();
        public GSSM()
        {
            InitializeComponent();
            SolutionOfTaskBox.Text = "";
            ValueOfFunctionBox.Text = "";
            BapsedTimeBox.Text = "";
            AmountOfIteratinosBox.Text = "";
            AbsBox.Text = "";
            label11.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            funcBox.Text = c.Function_Verify(funcBox.Text);
            if (funcBox.Text != "")
            {
                if (LeftEndPointBox.Text == "" || RightEndPointBox.Text == "" || ToleranceBox.Text == "" || timeBox.Text == "" || K_maxBox.Text == "")
            {
                MessageBox.Show("Input textboxes are empty! Enter the data");
                label11.Text = "Input textboxes are empty! Enter the data";
                return;
            }
            func = funcBox.Text.Trim().Replace(".", ",");
         
            func = func.ToLower();
            label11.Text = "";
            try
            {
                k_max = Int32.Parse(K_maxBox.Text.Trim());
            }
            catch (Exception h)
            {
                MessageBox.Show("Character values are not allowed.Check k_max.");
                label11.Text = "Character values are not allowed.Check k_max.";
                return;
            }
            if (k_max <= 0)
            {
                MessageBox.Show("k_max can`t be less or equal than zero");
                label11.Text = "k_max can`t be less or equal than zero";
                return;
            }
            try
            {
                intervalBegin = Decimal.Parse(LeftEndPointBox.Text.Trim().Replace(".", ","));
            }
            catch (Exception h)
            {
                MessageBox.Show("Character values are not allowed.Check left bound value.");
                label11.Text = "Character values are not allowed.Check left bound value.";
                return;
            }
            try
            {
                intervalEnd = Decimal.Parse(RightEndPointBox.Text.Trim().Replace(".", ","));
            }
            catch (Exception h)
            {
                MessageBox.Show("Character values are not allowed.Check right bound value.");
                label11.Text = "Character values are not allowed.Check right bound value.";
                return;
            }
            try
            {
                precision = Decimal.Parse(ToleranceBox.Text.Trim().Replace(".", ","), System.Globalization.NumberStyles.Float);
            }
            catch (Exception h)
            {
                MessageBox.Show("Character values are not allowed.Check the Tolerance textbox.");
                label11.Text = "Character values are not allowed.Check the Tolerance textbox.";
                return;
            }
            try
            {
                max_time = decimal.Parse(timeBox.Text.Trim());
            }
            catch (Exception h)
            {
                MessageBox.Show("Character values are not allowed.Check the max_time textbox.");
                label11.Text = "Character values are not allowed.Check the max_time textbox.";
                return;
            }
            string str = "-";

            if (str == ToleranceBox.Text[0].ToString() || ToleranceBox.Text.ToString() == "0")
            {
                MessageBox.Show("Tolerance can`t be less or equal than zero");
                label11.Text = "Tolerance can`t be less or equal than zero";
                return;
            }
            if (max_time <= 0)
            {
                MessageBox.Show("Time limit can`t be less or equal than zero");
                label11.Text = "Time limit can`t be less or equal than zero";
                return;
            }
            if (funcBox.Text == "$" || funcBox.Text == "#" || funcBox.Text == "@" || funcBox.Text == "&" || funcBox.Text == "$" || funcBox.Text == "`" || funcBox.Text == "?" || funcBox.Text == ";" || funcBox.Text == ":")
            {
                MessageBox.Show("Function incorrect.You are probably added wrong values or incorrect signs like #,@,$,& and etc.Please check the textbox of functions.");
                return;
            }
          
                try
                {
                    func = funcBox.Text.Trim().Replace("b", "x");

                
                funcBox.Text = func;
             
            }
                catch (Exception h)
                {
                    MessageBox.Show("Function incorrect,programm can`t read different values.Please check the textbox of functions");
                    label11.Text = "Function incorrect";
                    return;
                }
           
            try
            {
              
                GetFunction(func,intervalBegin);
            }
            catch (Exception h)
            {
                MessageBox.Show("Function incorrect, it doesn`t match to mathematical representation of the function.Please check the textbox of functions");
                label11.Text = "Function incorrect";
                return;
            }
          

              
          


            try
            {
                 GetFunction(func, intervalEnd);
            }
            catch (Exception h)
            {
                MessageBox.Show("Function incorrect.Please check the textbox of functions");
                label11.Text = "Function incorrect";
                return;
            }

                progressBar.Value = 0;
         
                GSSM_Method_Min min = new GSSM_Method_Min();
                GSSM_Method_Max max = new GSSM_Method_Max();
                var R = (Convert.ToDecimal(Math.Sqrt(5)) - 1M) / 2M;
                var x1 = intervalBegin + (1M - R) * (intervalEnd) - intervalBegin;
                var x2 = intervalBegin + R * (intervalEnd) - intervalBegin;
                f1 = GetFunction(func, x1);
                n = precision;
                f2 = GetFunction(func, x2);
            if (radioButton1.Checked)
                {
                max.increment_max += UpdateIteration;
                max.time_max += UpdateTime;
                max.start(intervalBegin, intervalEnd, precision, k_max, init, max_time, func, progressBar);
                ResultPrint(max.intervalBegin1, max.f3, max.elapsedTime1, max.k_max_begin, max.time_max_begin, max.init1, max.abs, max.b3);
                stopWatch.Reset();
                init = 0;
                }
                else if (radioButton2.Checked)
                {
                min.increment_max += UpdateIteration;
                min.time_max += UpdateTime;
                min.start(intervalBegin, intervalEnd, precision, k_max, init, max_time, func, progressBar);
                    ResultPrint(min.x1, min.f2, min.elapsedTime1, min.k_max_begin, min.time_max_begin, min.init1, min.abs, min.b3);
                    stopWatch.Reset();
                    init = 0;
                }
                else
                {
                    MessageBox.Show("Choose max or min");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Function incorrect.Please check the textbox of functions");
                label11.Text = "Function incorrect";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SolutionOfTaskBox.Text = "";
            ValueOfFunctionBox.Text = "";
            BapsedTimeBox.Text = "";
            AmountOfIteratinosBox.Text = "";
            AbsBox.Text = "";
            label11.Text = "";
        }

        private void GoToExcelButton_Click(object sender, EventArgs e)
        {     
            string mySheet =Path.Combine(System.Windows.Forms.Application.StartupPath, @"LookingForOneOptPoint.xlsm") ;//Выбор экселевского файла без обязательного задания путя
            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            Excel.Workbooks books = excelApp.Workbooks;
            Excel.Workbook sheet = books.Open(mySheet);
        }

        public void ResultPrint(decimal x1, decimal f1, string relError, int k_max_begin, decimal time_max_begin, int iter_value, decimal rel,string b1)
        {
            SolutionOfTaskBox.Text = x1.ToString();
            ValueOfFunctionBox.Text = f1.ToString();
            AmountOfIteratinosBox.Text = iter_value.ToString();
            BapsedTimeBox.Text = relError.ToString();
            AbsBox.Text = rel.ToString("0e0");
            K_maxBox.Text = k_max_begin.ToString();
            timeBox.Text = time_max_begin.ToString();
            label11.Text = b1.ToString();
        }
        private decimal GetFunction(string function, decimal x1)
        {
            Computer comp = new Computer();
            return comp.Compute(function, x1);
        }
        private void UpdateIteration(int iteration)
        {
            System.Action action = () =>
            {
                K_maxBox.Text = iteration.ToString();
            };
            Invoke(action);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTime(int iteration)
        {
            System.Action action = () =>
            {
                timeBox.Text = iteration.ToString();
            };
            Invoke(action);
        }
        public class ComparisonComparer<T> : IComparer<T>
        {
            private readonly Comparison<T> _comparison;


            public ComparisonComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }


            public int Compare(T x, T y)
            {
                return _comparison(x, y);
            }
        }
    }
}
