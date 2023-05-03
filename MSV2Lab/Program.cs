using System;

namespace NumericMethodsEquMSV
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ModN method: x = " + Method.MNewton(-2.5, 0).ToString("0.000000"));
            Console.WriteLine("Aposteriori: n = " + Method.ApostNewton);
            Console.WriteLine("Relax method: x = " + Method.Relaxation(0.13, -2.1));//(-2.5; -2.0)
            Console.WriteLine("Aposteriori assessment: n = " + Method.ApostRelax);
            Console.WriteLine("Apriori assessment: n = " + Method.ApriorRelax(0.2, -2.1));
            Console.ReadKey();
        }
    }
}
