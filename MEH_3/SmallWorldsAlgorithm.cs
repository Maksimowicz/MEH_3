using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEH_3
{
    public class SmallWorldsAlgorithm
    {
        IContinuousDistribution distributionClassClose;
        IContinuousDistribution distributionClassFar;
        Random random;
        Func<List<double>, int, double> testFunction; //List<int> - wartości, int - wymiary, int - wynik
        int dimensions = 1;



        public SmallWorldsAlgorithm(Func<List<double>, int, double> testFunction, int dimensions, IContinuousDistribution distributionClassClose, IContinuousDistribution distributionClassFar)
        {
            this.random = new Random();
            this.distributionClassClose = distributionClassClose;
            this.distributionClassFar = distributionClassFar;
            this.dimensions = dimensions;
            this.testFunction = testFunction;
        }

        //Far operator - rise algorithm
        //Close operator -  nieghbourrhood algorithm

        public Tuple<List<double>, double> RunSmallWorldsAlgorithm()
        {
            int k = 0;

            List<Tuple<List<double>, double>> parametersList = new List<Tuple<List<double>, double>>();

            //List<List<double>> parametersList = new List<List<double>>();
            //List<double> parametersResult = new List<double>();

            //Starting parameters
            for(int i = 0; i < 10; i++)
            {
                List<double> parameters = new List<double>();

                for (int j = 0; j < dimensions; j++)
                {
                    parameters.Add(distributionClassClose.Sample());
                }

                parametersList.Add(Tuple.Create(parameters, testFunction(parameters, parameters.Count)));
            }


            while (k < 1000)
            {
                Tuple<List<double>, double> resultTouple; // = Tuple.Create(parametersList[i], testFunction(parametersList[i], parametersList.Count));
                for(int i = 0; i < 10; i++)
                {
                    Tuple<List<double>, double> resultMutation = this.MutationSearch(parametersList[i].Item1, 10);
                    Tuple<List<double>, double> resultNeighbour = this.NeighbourSearch(parametersList[i].Item1, 5);
          

                    if(resultMutation.Item2 < resultNeighbour.Item2)
                    {
                        resultTouple = resultMutation;
                    }
                    else
                    {
                        resultTouple = resultNeighbour;
                    }

                    parametersList[i] = resultTouple;

                }

                k++;
            }

            int indexOfMin = parametersList.Select((n, i) => new { index = i, value = n })
                                 .OrderBy(item => item.value.Item2)
                                 .First().index;

            return parametersList[indexOfMin];
        }

        protected Tuple<List<double>, double> MutationSearch(List <double> parameters, int newResultsPerTick)
        {
            int dimensions = parameters.Count;
            
            List<double> newParameters = new List<double>(parameters);

            double result = testFunction(parameters, parameters.Count);

            for (int i = 0; i < newResultsPerTick; i++)
            {
                //Gets list of parameters that will be mutated
                List<int> mutationTable = new List<int>();

                if (dimensions == 1)
                {
                    mutationTable.Add(1);
                }
                else
                {
                    mutationTable = Enumerable.Range(0, dimensions)
                                        .Select(r => random.Next(0, 2))
                                        .ToList();
                }

                List<double> permutatedParameters = new List<double>(parameters);

                for (int j = 0; j < mutationTable.Count; j++)
                {
                    if (mutationTable[j] == 1)
                    {
                        permutatedParameters[j] = distributionClassFar.Sample();
                    }

                }

                double resultInner = testFunction(permutatedParameters, permutatedParameters.Count);

                if (result > resultInner)
                {
                    result = resultInner;
                    newParameters = new List<double>(permutatedParameters);
                }

            }

            return Tuple.Create(newParameters, result);
        }


        protected Tuple<List<double>, double> NeighbourSearch(List<double> startingParameters,double neighbourhoodSize)
        {
            double result = 0;
            double neighbourhoodSizeInner = 1;
            double innerResult = 0;

            List<double> bestParametersTick = new List<double>();
            List<double> bestParameters;
            List<double> startParameters = startingParameters;

            bestParameters = startParameters;
            result = testFunction(startParameters, dimensions);

            while (neighbourhoodSizeInner < neighbourhoodSize)
            {
                List<double> permutatedParameters = new List<double>(bestParameters);

                Boolean betterResultFound = false;
                bestParametersTick = new List<double>();
                innerResult = double.PositiveInfinity;

                for (int j = 0; j < 20; j++)
                {
                    

                    for (int i = 0; i < dimensions; i++)
                    {
                        double singleNeighbourSize = distributionClassClose.Sample();

                        while (singleNeighbourSize > neighbourhoodSize || singleNeighbourSize < (-1) * neighbourhoodSize)
                        {
                            singleNeighbourSize = distributionClassClose.Sample();
                        }

                        permutatedParameters[i] = bestParameters[i] + singleNeighbourSize;
                    }

                    innerResult = testFunction(permutatedParameters, dimensions);

                    if (innerResult < result)
                    {
                        bestParametersTick = new List<double>(permutatedParameters);                      
                        result = innerResult;
                        betterResultFound = true;
                    }
                }

                if (betterResultFound)
                {
                    bestParameters = new List<double>(bestParametersTick);
                    neighbourhoodSizeInner = 1;
                }
                else
                {
                    neighbourhoodSizeInner += 0.3;
                }
            }

            return Tuple.Create(bestParameters, result);
        }

    }


    
}
