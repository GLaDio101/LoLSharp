using System;
using System.Linq;

namespace LoLSharp.Core
{
  public class CustomScriptBase
  {
    public static CustomScriptBase Instance;

    public string Title;

    public bool Active;
    public void Init()
    {
      Instance = new CustomScriptBase();
      
      var type = this.GetType();
      var attribute = type.GetCustomAttributes(typeof(CustomScriptAttribute), true).FirstOrDefault() as CustomScriptAttribute;
      if (attribute != null)
      {
        Console.WriteLine(attribute.Title + " Loaded.");
        Title = attribute.Title;
        Active = attribute.DefaultActive;
      }
    }
    public virtual void Update()
    {
    }
    public virtual void LateUpdate()
    {
    }
  }
}
