
using System;

namespace NumericMethodsEquMSV
{
    public static class Method
    {
        public static IDataProvider DataProvider { get; set; } = new DataProvider();

        public static double SomeMethodThatThrowsException()
        {
            double data = DataProvider.GetData();
            if (data < 0)
            {
                throw new InvalidOperationException("Data is less than zero.");
            }
            return data * 2;
        }

        public static double epsilon = 10e-5;
        public static int ApostRelax = 0, ApostNewton = 0;

        public static double SomeMethod()
        {
            double data = DataProvider.GetData();
            return data;
        }
        public static double Fu(double x)
        {
            return (Math.Pow(x, 2) + 5 * Math.Sin(x) - 1); ;
        }
        public static double Fu_App(double x)
        {
            return (2 * x + 5 * Math.Cos(x));
        }
        public static double Fu_App2(double x)
        {
            return (2 - 5 * Math.Cos(x));
        }
        public static double FiFu(double x)
        {
            if (x >= 3)
            {
                throw new ArgumentOutOfRangeException();
            }
            double fi = Math.Asin((-Math.Pow(x, 2) + 1) / 5);

           


            return fi;
        }
        public static double FiFuApp(double x)
        {
            double fi_der = -(5 * Math.Cos(x) / 2 * Math.Sqrt(-5 * Math.Sin(x) + 1));
            return 0;
        }
        public static double Relaxation(double tau, double x0)
        {
            double x = x0;

            do
            {
                x0 = x;
                x = x0 + tau * Fu(x0);
                ApostRelax++;

            }

            while (Math.Abs(x - x0) > epsilon);
            return Math.Round(x, 3);
        }
        public static double MNewton(double a, double b)
        {
            double curr, next, x0;
            if (Fu(a) * Fu_App2(a) > 0) x0 = a;
            else x0 = b;
            next = x0;
            double funcDer = Fu_App(x0);
            do
            {
                curr = next;
                next = curr - (Fu(curr) / funcDer);
                ApostNewton++;
            } while (Math.Abs(next - curr) > epsilon);
            return next;
        }
        public static int ApriorRelax(double q, double z)
        {   
            if (q == 0)
            {
                throw new DivideByZeroException("You can't divide by Zero!");
            }
            int x = (int)(Math.Log(Math.Abs(z) / epsilon) / Math.Log(1 / q)) + 1;

            return x;
        }
    }
}