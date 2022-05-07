using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEH_2
{
    public class AlgorithmsClass
    {

        public static double quadraticFunction(List<double> parameters, int dimensions)
        {
            double returnValue = 0;

            if (dimensions != parameters.Count)
            {
                throw new Exception("Wrong parameters count.");
            }
            //min f(x) = sum((x(i) -1)^2)

            foreach (double parameter in parameters)
            {
                returnValue += Math.Pow(parameter - 1, 2);
            }


            return returnValue;
        }

        public static double ackleyFunction(List<double> parameters, int dimensions)
        {
            double returnValue = 0;
            double firstSum = 0;
            double secondSum = 0;
          

            //min f(x) = exp(1)+20-20*exp(-0.2*sqrt((1/n)*sum(x(i)^2)))-exp((1/n)*sum(cos(2*Pi*x(i))))
            if (dimensions != parameters.Count)
            {
                throw new Exception("Wrong parameters count.");
            }

            foreach(double parameter in parameters)
            {
                firstSum += Math.Pow(parameter, 2);
                secondSum += Math.Cos(2 * Math.PI * parameter);
            }
            double OneDvidieDim = (double)1 / (double)dimensions;

            returnValue = Math.Exp(1) + 20 - 20 * Math.Exp(-0.2 * Math.Sqrt(OneDvidieDim * firstSum)) - Math.Exp(OneDvidieDim * secondSum);

            return returnValue;
        }


        public static double rastringFunction(List<double> parameters, int dimensions)
        {
            double returnValue = 0;
            double firstSum = 0;

            //min f(x) = 10.0*n + sum(x(i)^2 - 10.0*cos(2*Pi*x(i)))
   
            foreach (double parameter in parameters)
            {
                firstSum += Math.Pow(parameter, 2) - 10 * Math.Cos(2 * Math.PI * parameter);
            }

            returnValue = 10 * dimensions + firstSum;

            return returnValue;
        }
    }
}
