using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> listServices = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        listServices[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        if (listServices.ContainsKey(typeof(T)))
            return (T)listServices[typeof(T)];
        else
        {
            return default(T);
        } 
    }
}
