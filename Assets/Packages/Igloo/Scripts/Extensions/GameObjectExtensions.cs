using System;
using UnityEngine;

namespace Packages.Igloo.Scripts.Extensions
{
    public sealed class GameObjectExtensions
    {
        public static Component GetOrAddComponent<T>(GameObject gameObj) where T : Component
        {
            Component componentToReturn;
            if (gameObj.TryGetComponent(typeof(T), out componentToReturn))
            {
                return componentToReturn;
            }
        
            gameObj.AddComponent<T>();
        
            if (gameObj.TryGetComponent(typeof(T), out componentToReturn))
            {
                return componentToReturn;
            }

            throw new ComponentMissingException();
        }
    }

    public class ComponentMissingException : Exception
    {
        public ComponentMissingException()
        {
            Debug.Log("Component not found and couldn't be applied.");
        }
    }

    public static class GameObjectExtensionsHandler
    {
        public static Component GetOrAddComponent<T>(this GameObject gameObj) where T : Component
        {
            return GameObjectExtensions.GetOrAddComponent<T>(gameObj);
        }
    }
}