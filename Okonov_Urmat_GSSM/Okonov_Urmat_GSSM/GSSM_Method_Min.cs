using System;
using parserDecimal.Parser;
using System.Diagnostics;
using System.Windows.Forms;


namespace Okonov_Urmat_GSSM
{
    public class GSSM_Method_Min
    {
        decimal eps = 0,Abs=0;
        public decimal timepl = 0;
        public decimal intervalBegin1 = 0;
        public decimal intervalEnd1 = 0;
        public decimal f3 = 0;
        public decimal cond = 0, cond1 = 0;
        public decimal precision = 0;
        public Stopwatch stopWatch = new Stopwatch();
        public Stopwatch time = new Stopwatch();
        public int init1 = 0;
        public TimeSpan tim = new TimeSpan();
        public string elapsedTime1 = "";
        Computer computer = new Computer();
        public string func = "", b1 = "", b2 = "", b3 = "";
        public int k_max_begin = 0;
        public decimal time_max_begin = 0,abs=0;
        GSSM b = new GSSM();
        public decimal x1, f1, x2, f2;
        public uint k = 0;
        public decimal kon;
        public bool flag = false;
        public event Action<int> increment_max;
        public event Action<int> time_max;
        public void start(decimal intervalBegin, decimal intervalEnd, decimal precision, int K_maxBox1, int init, decimal max_time, string func, System.Windows.Forms.ProgressBar progressBar)
        {
            decimal R = Convert.ToDecimal((Math.Sqrt(5) - 1) / 2);
            x1 = intervalBegin + (1 - R) * (intervalEnd - intervalBegin);
            f1 = GetFunction(func, x1);
            x2 = intervalBegin + R * (intervalEnd - intervalBegin);
            f2 = GetFunction(func, x2);
            if (intervalBegin == intervalEnd)
            {
                MessageBox.Show("There isn`t difference between a and b. Please check a graph of function!");
                return;
            }

            do
            {
                stopWatch.Start();
                init++;

                if (f1 > f2)
                {
                    intervalBegin = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = intervalBegin + R * (intervalEnd - intervalBegin);
                    f2 = GetFunction(func, x2);
                    Abs = Math.Abs(intervalEnd - intervalBegin);
                }
                else
                {
                    intervalEnd = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = intervalBegin + (1 - R) * (intervalEnd - intervalBegin);
                    f1 = GetFunction(func, x1);
                    Abs = Math.Abs(intervalEnd - intervalBegin);
                }


                progressBar.Visible = true;
                progressBar.Maximum = Convert.ToInt32(init + 0.0001);
                progressBar.Value = init;

                stopWatch.Stop();
                tim = stopWatch.Elapsed;
                TimeSpan ts1 = stopWatch.Elapsed;
                elapsedTime1 = String.Format("{00}",
                ts1.Milliseconds);
                eps = Convert.ToDecimal(elapsedTime1);
                if (eps < 0.01M)
                {
                    timepl = 0.01M - eps;
                }
                if (eps >= max_time)
                {
                    var mb = MessageBox.Show("K_max " + K_maxBox1 + "\n" + "Time " + max_time + "\n" + "X*= " + x1 + "\n" + "Time is up.It isn't possible to find a solution with the given time.Would you like to continue? ", "Warning", MessageBoxButtons.YesNo);
                    if (mb == DialogResult.Yes)
                    {
                        max_time += max_time;
                        time_max?.Invoke(Convert.ToInt32(max_time));

                    }
                    else if (mb == DialogResult.No)
                    {
                        b1 = "Solution hasn`t found.Time reached the limit.";
                        break;
                    }

                }


                if (init >= K_maxBox1)
                {
                    var mb = MessageBox.Show("K_max " + K_maxBox1 + "\n" + "Time " + max_time + "\n" + "X*= " + x1 + "\n" + "iterations reached the limit.It isn't possible to find a solution with the given k_max.Would you like to continue? ", "Warning", MessageBoxButtons.YesNo);
                    if (mb == DialogResult.Yes)
                    {
                        K_maxBox1 += K_maxBox1;
                        increment_max?.Invoke(Convert.ToInt32(K_maxBox1));

                    }
                    else if (mb == DialogResult.No)
                    {
                        b1 = "Solution hasn`t found.iterations reached the limit.";
                        break;
                    }

                }
                b1 = "Solution has found";

 

            } while (Math.Abs(intervalBegin - intervalEnd) > precision && init < K_maxBox1);

            stopWatch.Reset();
            progressBar.Visible = false;
            intervalBegin1 = x1;
            intervalEnd1 = x2;
            b3 = b1;
            f3 = f2;
            init1 = init;
            k_max_begin = Convert.ToInt32(K_maxBox1);
            time_max_begin = max_time;
            b3 = b1;
            abs = Math.Abs(intervalBegin - intervalEnd);
        }


        private decimal GetFunction(string function, decimal x1)
        {
            Computer comp = new Computer();
            return comp.Compute(function, x1);
        }
    }
}

