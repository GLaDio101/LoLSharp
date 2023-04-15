using System;

namespace LoLSharp.Core
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
  public class CustomScriptAttribute : Attribute
  {

    public CustomScriptAttribute(string title, bool defaultActive = false)
    {
      this.Title = title;
      DefaultActive = defaultActive;
    }

    public string Title { get; }
    public bool DefaultActive { get; }
  }
}
