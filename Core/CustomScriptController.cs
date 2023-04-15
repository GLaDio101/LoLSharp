using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoLSharp.Core
{
  public static class CustomScriptController
  {
    private static Dictionary<string, CustomScriptBase> _map;
    public static void Init()
    {
      _map = new Dictionary<string, CustomScriptBase>();
      
      Assembly assembly = Assembly.GetExecutingAssembly();

      var typesWithCustomAttribute = from type in assembly.GetTypes()
                                     let attributes = type.GetCustomAttributes(typeof(CustomScriptAttribute), false)
                                     where attributes != null && attributes.Length > 0
                                     select new { Type = type, Attributes = attributes.Cast<CustomScriptAttribute>() };

      foreach (var typeWithAttribute in typesWithCustomAttribute)
      {
        Type type = typeWithAttribute.Type;
        object instance = Activator.CreateInstance(type);
        var customScriptBase = (CustomScriptBase)instance;
        customScriptBase.Init();
        _map.Add(customScriptBase.Title, customScriptBase);
        // Console.WriteLine("Type: {0}", type.Name);
        // foreach (var attribute in typeWithAttribute.Attributes)
        // {
        //   Console.WriteLine("Description: {0}", attribute.Title);
        // }
      }
    }


    public static CustomScriptBase[] GetActiveScripts()
    {
      return _map.Values.Where(script => script.Active).ToArray();
    }
    
    public static CustomScriptBase[] GetScripts()
    {
      return _map.Values.ToArray();
    }

    public static CustomScriptBase GetByKey(string key)
    {
      return _map[key];
    }
  }
}
