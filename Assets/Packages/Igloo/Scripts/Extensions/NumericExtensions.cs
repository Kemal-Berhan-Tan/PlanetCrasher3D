using UnityEngine;

namespace Packages.Igloo.Scripts.Extensions
{
    public sealed class NumericExtensions
    {
        public static T[] Swap<T>(T[] numbers)
        {
            if (numbers.Length != 2)
            {
                //todo: exception
            }
        
            T[] swappedNumbers = numbers;

            T tempNumeric = swappedNumbers[0];
            swappedNumbers[0] = swappedNumbers[1];
            swappedNumbers[1] = tempNumeric;
        
            return swappedNumbers;
        }

        public static bool WeightedRandom(int weight)
        {
            int value = Random.Range(1, 101);
            return value <= weight;
        }

        public static float IntToFloat(int value)
        {
            return value;
        } 
    
        public static double IntToDouble(int value)
        {
            return value;
        }
    
        public static int FloatToInt(float value)
        {
            return (int) value;
        }
    
        public static double FloatToDouble(float value)
        {
            return value;
        }
    
        public static int DoubleToInt(double value)
        {
            return (int) value;
        }
    
        public static float DoubleToFloat(double value)
        {
            return (float) value;
        }
    }

    public static class NumericExtensionsHandler
    {
        public static T[] Swap<T>(T[] numbers)
        {
            return NumericExtensions.Swap(numbers);
        }

        public static bool WeightedRandom(int weight)
        {
            return NumericExtensions.WeightedRandom(weight);
        }

        public static float IntToFloat(int value)
        {
            return NumericExtensions.IntToFloat(value);
        } 
    
        public static double IntToDouble(int value)
        {
            return NumericExtensions.IntToDouble(value);
        }
    
        public static int FloatToInt(float value)
        {
            return NumericExtensions.FloatToInt(value);
        }
    
        public static double FloatToDouble(float value)
        {
            return NumericExtensions.FloatToDouble(value);
        }
    
        public static int DoubleToInt(double value)
        {
            return NumericExtensions.DoubleToInt(value);
        }
    
        public static float DoubleToFloat(double value)
        {
            return NumericExtensions.DoubleToFloat(value);
        }

        public static float Normalize(this float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static float Denormalize(this float value, float min, float max)
        {
            return value * (max - min) + min;
        }
    }
}