using System;
using HZH_Controls;

namespace AirdPro.Algorithms.Maths;

public class GaussianRandomGenerator
{
    private Random random;
    private double mean;
    private double standardDeviation;

    public GaussianRandomGenerator(double mean, double standardDeviation)
    {
        this.random = new Random();
        this.mean = mean;
        this.standardDeviation = standardDeviation;
    }

    private double GenerateStandardGaussian()
    {
        double u, v, s;
        do
        {
            u = random.NextDouble();
            v = 2 * random.NextDouble() - 1;
            s = u * u + v * v;
        }
        while (s >= 1 || s == 0);

        return v * Math.Sqrt(-2 * Math.Log(s) / s);
    }

    public int GenerateTruncatedGaussian(int minValue, int maxValue)
    {
        double result;
        do
        {
            result = mean + GenerateStandardGaussian() * standardDeviation;
        }
        while (result < minValue || result > maxValue);

        return result.ToInt();
    }

    public int[] GenerateRandomNumbers(int count, int minValue, int maxValue)
    {
        int[] numbers = new int[count];
        for (int i = 0; i < count; i++)
        {
            numbers[i] = GenerateTruncatedGaussian(minValue, maxValue);
        }
        return numbers;
    }
}