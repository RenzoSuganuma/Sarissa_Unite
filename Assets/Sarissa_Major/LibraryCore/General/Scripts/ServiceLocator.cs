using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Sarissa.CodingFramework
{
    public static class ServiceLocator<T> where T : class
    {
        public static T Instance { get; private set; }

        public static bool IsValid => Instance is not null;

        public static void Add(T instance)
        {
            Instance = instance;
        }

        public static void Remove(T instance)
        {
            if (Instance == instance)
            {
                Clear();
            }
        }

        public static void Clear()
        {
            Instance = null;
        }
    }
}