using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using LoLSharp.Modules;
using Microsoft.CSharp;
using SharpDX;
using SharpDX.Mathematics.Interop;
namespace LoLSharp.Managers
{
    public class ExecutableMethod
    {

        public MethodInfo MethodInfo;

        public object Instance;
    }

    public class ScriptManager
    {
        private static List<ExecutableMethod> _updateMethods;
        private static List<ExecutableMethod> _lateUpdateMethods;
        private static List<ExecutableMethod> _drawMethods;

        public static void LoadScripts()
        {
            LogService.Log("Scripts Loading...");

            _updateMethods = new List<ExecutableMethod>();
            _drawMethods = new List<ExecutableMethod>();
            _lateUpdateMethods = new List<ExecutableMethod>();

            var enumerateFiles = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory + "/Scripts", "*.cs");
            foreach (string file in enumerateFiles)
            {
                var filename = Path.GetFileName(file).Replace(".cs", "");

                string source = File.ReadAllText(file);

                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Linq.dll");
                cp.ReferencedAssemblies.Add("System.Core.dll");
                cp.ReferencedAssemblies.Add("SharpDX.Mathematics.dll");
                var assemblyLocation = typeof(ScriptManager).Assembly.Location;
                cp.ReferencedAssemblies.Add(assemblyLocation);
                cp.ReferencedAssemblies.Add(typeof(Vector3).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(RawVector3).Assembly.Location);
                cp.ReferencedAssemblies.Add(typeof(Exception).Assembly.Location);
                cp.CompilerOptions = "/t:library";
                cp.GenerateInMemory = true;

                CSharpCodeProvider provider = new CSharpCodeProvider();

                var results = provider.CompileAssemblyFromSource(cp, source);

                if (results.Errors.Count != 0)
                {
                    foreach (CompilerError error in results.Errors)
                    {
                        Console.WriteLine(error.ErrorText);
                    }
                    throw new Exception("Mission failed!");

                }

                object o = results.CompiledAssembly.CreateInstance("Script." + filename);
                if (o != null)
                {
                    MethodInfo mi = o.GetType().GetMethod("OnUpdate");
                    _updateMethods.Add(new ExecutableMethod()
                    {
                        MethodInfo = mi,
                        Instance = o
                    });

                    MethodInfo draw = o.GetType().GetMethod("OnDraw");
                    _drawMethods.Add(new ExecutableMethod()
                    {
                        MethodInfo = draw,
                        Instance = o
                    });

                    MethodInfo lateUpdate = o.GetType().GetMethod("OnLateUpdate");
                    _lateUpdateMethods.Add(new ExecutableMethod()
                    {
                        MethodInfo = lateUpdate,
                        Instance = o
                    });
                    LogService.Log(filename + " Loaded.");
                }
                else
                {
                    LogService.Log("Wrong Class Name");

                }
            }
        }

        public static void ExecuteUpdates()
        {
            foreach (var executableMethod in _updateMethods)
            {
                executableMethod.MethodInfo.Invoke(executableMethod.Instance, null);
            }
        }

        public static void ExecuteDraws()
        {
            foreach (var executableMethod in _drawMethods)
            {
                executableMethod.MethodInfo.Invoke(executableMethod.Instance, null);
            }
        }
        public static void ExecuteLateUpdates()
        {
            foreach (var executableMethod in _lateUpdateMethods)
            {
                executableMethod.MethodInfo.Invoke(executableMethod.Instance, null);
            }
        }
    }
}
