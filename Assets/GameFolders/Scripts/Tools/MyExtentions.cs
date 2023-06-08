using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFolders.Scripts.Tools
{
    public static class MyExtentions
    {
        public static void UpgradeCount<T>(this IList<List<T>> list, int requirementCount)
        {
            while (list.Count <= requirementCount)
            {
                list.Add(new List<T>());
            }
        }

        public static void UpgradeTheQueueToSuitableCount<T>(ref List<Queue<T>> list, int requirementCount)
        {
            while (list.Count <= requirementCount)
            {
                list.Add(new Queue<T>());
            }
        }

        public static void UpgradeCount<T>(this IList<T> list, int requirementCount) where T : new()
        {
            while (list.Count <= requirementCount)
            {
                list.Add(new T());
            }
        }

        public static List<List<T>> Split<T>(this IList<T> list)
        {
            return list.Select((x, y) => new { Index = y, Value = x })
                .GroupBy(x => x.Index / 4)
                .Select(x => x.Select(v => v.Value).ToList()).ToList();
        }

        public static IEnumerator DelayedForLoop<T>(this IList<T> list, float delay, Action<int> callback)
        {
            for (int i = 0; i < list.Count; i++)
            {
                callback?.Invoke(i);
                yield return new WaitForSeconds(delay);
            }
        }

        public static int GetIndex<Enum>(this Enum tempEnum)
        {
            return (int)(object)tempEnum;
        }

        public static List<Enum> GetOrderTypesEnumList(Type tempEnum)
        {
            return Enum.GetValues(tempEnum.GetType()).Cast<Enum>().ToList();
        }

        public static void RemoveIfItContainsListT<T>(T targetObj, ref List<List<T>> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Contains(targetObj))
                {
                    list[i].Remove(targetObj);
                }
            }
        }
    }
}