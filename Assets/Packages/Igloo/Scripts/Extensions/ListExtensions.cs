using System;
using System.Collections.Generic;
using System.Linq;

namespace Packages.Igloo.Scripts.Extensions
{
    public sealed class ListExtensions
    {
        private static Random _rng = new Random();

        public static List<T> GetRandomUniqueElements<T>(List<T> list, int count)
        {
            List<T> finalList = new List<T>();
            List<T> tempList = ShuffleList(list);
            tempList = tempList.Distinct().ToList();
            for (int index = 0; index < count; index++)
            {
                finalList.Add(tempList[index]);
            }
            return finalList;
        }
        public static List<T> GetRandomElements<T>(List<T> list, int count)
        {
            List<T> finalList = new List<T>();
            List<T> tempList = ShuffleList(list);
            for (int index = 0; index < count; index++)
            {
                finalList.Add(tempList[index]);
            }
            return finalList;
        }

        public static List<T> RemoveRandomElements<T>(List<T> list, int count)
        {
            List<T> tempList = ShuffleList(list);
            for (int index = 0; index < count; index++)
            {
                tempList.Remove(tempList[0]);
            }
            List<T> finalList = new List<T>(tempList);
            return finalList;
        }

        public static List<T> ShuffleList<T>(List<T> list, int randomSeed = 0)
        {
            if (randomSeed != 0)
            {
                _rng = new Random(randomSeed);
            }
        
            List<T> finalList = new List<T>(list);
            var n = finalList.Count;
            while (n > 1)
            {
                n--;
                var k = _rng.Next(n + 1);
                var value = finalList[k];
                finalList[k] = finalList[n];
                finalList[n] = value;
            }
            return finalList;
        }

        //primitives only (prolly)
        public static String LogList<T>(List<T> listToPrint)
        {
            string finalOutput = "{";
            foreach (T element in listToPrint)
            {
                finalOutput += "[";
                finalOutput += element.ToString();
                finalOutput += "], ";
            }
            finalOutput += "EndList}";
            return finalOutput;
        }
    }

    public static class ListExtensionHandler
    {
        public static List<T> GetRandomUniqueElements<T>(this List<T> list, int count)
        {
            return ListExtensions.GetRandomUniqueElements(list, count);
        }
        public static List<T> GetRandomElements<T>(this List<T> list, int count)
        {
            return ListExtensions.GetRandomElements(list, count);
        }

        public static List<T> RemoveRandomElements<T>(List<T> list, int count)
        {
            return ListExtensions.RemoveRandomElements(list, count);
        }

        public static List<T> ShuffleList<T>(List<T> list)
        {
            return ListExtensions.ShuffleList(list);
        }

        public static String LogList<T>(this List<T> listToPrint)
        {
            return ListExtensions.LogList(listToPrint);
        }
    }
}