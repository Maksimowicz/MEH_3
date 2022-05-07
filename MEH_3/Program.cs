// See https://aka.ms/new-console-template for more information
using MathNet.Numerics.Distributions;
using MEH_2;
using MEH_3;

Cauchy cauchy = new Cauchy(0, 2);
Normal normal = new Normal();
Normal normal5 = new Normal(0, 5.0);
ContinuousUniform continuousUniform = new ContinuousUniform(-100,100);

List<int> testList = new List<int>();

testList.Add(0);
testList.Add(1);
testList.Add(2);
testList.Add(3);
testList.Add(4);

var d = testList[0];


//List<Tuple<Func<List<double>, int, double>, string>> functions = new List<Tuple<Func<List<double>, int, double>, string>>();

List<Func<List<double>, int, double>> functions = new List<Func<List<double>, int, double>>();

functions.Add(AlgorithmsClass.ackleyFunction);
functions.Add(AlgorithmsClass.rastringFunction);
functions.Add(AlgorithmsClass.quadraticFunction);

List<string> functionNames = new List<string>();
functionNames.Add("Ackley");
functionNames.Add("Rastring");
functionNames.Add("Quadratic");

List<IContinuousDistribution> distributions = new List<IContinuousDistribution>();
distributions.Add(cauchy);
distributions.Add(normal);
distributions.Add(normal5);
distributions.Add(continuousUniform);

List<string> distributionNames = new List<string>();
distributionNames.Add("Cauchy");
distributionNames.Add("Normal");
distributionNames.Add("Normal5");
distributionNames.Add("Uniform");

SmallWorldsAlgorithm sma = null;


//Get values based on 10 runs 
//Of each function
//With each distribution as close and far operator
//For dimensions 2-5
int functionCounter = 0;
foreach (var function in functions)
{
    int closeCounter = 0;
    foreach (var distributionClose in distributions)
    {
        int farCounter = 0;
        foreach (var distributionFar in distributions)
        {            
            for (int dimension = 2; dimension < 3; dimension++)
            {
                sma = new SmallWorldsAlgorithm(function, dimension, distributionClose, distributionFar);
                List<double> resultDimensionsSum = new List<double>();
                double resultSum = 0;

                for (int averageRuns = 0; averageRuns < 10; averageRuns++)
                {
                    Tuple<List<double>, double> resultTuple = sma.RunSmallWorldsAlgorithm();

                    //Add parameters to sum - divide at the end
                    if (resultDimensionsSum.Count == 0)
                    {
                        resultDimensionsSum = resultTuple.Item1;
                    }
                    else
                    {
                        for(int resultDimension = 0; resultDimension < dimension; resultDimension++)
                        {
                            resultDimensionsSum[resultDimension] += resultTuple.Item1[resultDimension];
                        }
                    }

                    resultSum += resultTuple.Item2;
                }

                for(int resultDimension = 0; resultDimension < dimension; resultDimension++)
                {
                    resultDimensionsSum[resultDimension] /= 10.00;
                }

                resultSum /= 10;

                //Write down the result:
                Console.WriteLine(String.Format("Funcion: {0} - Close: {1} - Far: {2} - Dimensions: {3}",
                                                functionNames[functionCounter],
                                                distributionNames[closeCounter],
                                                distributionNames[farCounter],
                                                dimension
                                                ));

                Console.WriteLine("Result: " + resultSum);
                foreach(double value in resultDimensionsSum)
                {
                    Console.WriteLine(value + " | ");
                }

                Console.WriteLine(Environment.NewLine);
            }

            farCounter++;
        }

        closeCounter++;
    }

    functionCounter++;
}





