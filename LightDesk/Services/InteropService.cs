using LightStack.LightDesk.CefRequestHandlers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace LightStack.LightDesk.Services
{
    public class InteropService
    {
        private class MethodInfoHandler : CefV8Handler
        {
            private object context;
            private MethodInfo methodInfo;
            private InteropService service;

            public MethodInfoHandler(MethodInfo info, object context, InteropService service)
            {
                this.methodInfo = info;
                this.context = context;
                this.service = service;
            }

            protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
            {
                exception = null;
                returnValue = CefV8Value.CreateNull();
                if (name == methodInfo.Name)
                {
                    var result = methodInfo.Invoke(context, UnmapArguments(methodInfo.GetParameters(), arguments, obj));
                    returnValue = MapReturnValue(methodInfo.ReturnType, result);
                    return true;
                }
                return false;
            }

            private CefV8Value MapReturnValue(Type returnType, object result)
            {
                return InteropService.MapValue(result);
            }

            private object[] UnmapArguments(ParameterInfo[] parameterInfo, CefV8Value[] arguments, CefV8Value obj)
            {
                var result = new object[arguments.Length];
                for (var i = 0; i < arguments.Length; i++)
                {
                    result[i] = service.UnmapValue(arguments[i], parameterInfo[i].ParameterType);
                }

                return result;
            }
        }

        private Dictionary<Type, CefV8Value> serviceCache = new Dictionary<Type, CefV8Value>();

        private WindowService windowsService;

        public delegate void Callback(object[] arguments);

        public InteropService(WindowService windowsService)
        {
            this.windowsService = windowsService;
        }

        public CefV8Value MapService(object service)
        {
            var type = service.GetType();
            var result = CefV8Value.CreateObject(null);

            foreach (var method in type.GetMethods())
            {
                result.SetValue(method.Name, MapMethod(service, method), CefV8PropertyAttribute.None);
            }

            return result;
        }

        private CefV8Value MapMethod(object service, MethodInfo method)
        {
            return CefV8Value.CreateFunction(method.Name, new MethodInfoHandler(method, service, this));
        }

        private static CefV8Value MapValue(object value)
        {
            if (value == null)
            {
                return CefV8Value.CreateNull();
            }
            var type = value.GetType();
            if (type == typeof(Int32))
            {
                return CefV8Value.CreateInt((int)value);
            }
            else if (type == typeof(String))
            {
                return CefV8Value.CreateString((string)value);
            }
            else if (type == typeof(Double))
            {
                return CefV8Value.CreateDouble((Double)value);
            }
            else if (type == typeof(DateTime))
            {
                return CefV8Value.CreateDate((DateTime)value);
            }
            else if (type == typeof(bool))
            {
                return CefV8Value.CreateBool((bool)value);
            }
            else if (IsGenericDictionary(type))
            {
                dynamic dictionary = value;
                var result = CefV8Value.CreateObject(null);
                foreach (dynamic key in dictionary.Keys)
                {
                    result.SetValue(key.ToString(), MapValue(dictionary[key]), CefV8PropertyAttribute.None);
                }

                return result;
            }
            else if (type.GetInterfaces().Any(x => x.Name.Contains("IEnumerable")))
            {
                var array = ((IEnumerable)value).Cast<object>().ToArray();
                var result = CefV8Value.CreateArray(array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    result.SetValue(i, MapValue(array[i]));
                }

                return result;
            }
            else if (type.IsEnum)
            {
                return CefV8Value.CreateString(value.ToString());
            }
            else if (type == typeof(Int16))
            {
                return CefV8Value.CreateInt((short)value);
            }
            else if (type == typeof(Int64))
            {
                return CefV8Value.CreateInt((int)(long)value);
            }
            else if (type == typeof(UInt32))
            {
                return CefV8Value.CreateUInt((uint)value);
            }
            else if (type == typeof(UInt16))
            {
                return CefV8Value.CreateUInt((ushort)value);
            }
            else if (type == typeof(UInt64))
            {
                return CefV8Value.CreateUInt((uint)(ulong)value);
            }
            else
            {
                var result = CefV8Value.CreateObject(null);
                foreach (var property in type.GetProperties().Where(x => x.CanRead))
                {
                    result.SetValue(property.Name, MapValue(property.GetValue(value)), CefV8PropertyAttribute.None);
                }
                return result;
            }
        }

        private static bool IsGenericDictionary(Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType && x.Name.Contains("IDictionary"));
        }

        private object UnmapValue(CefV8Value value, Type type)
        {
            if (type == typeof(Int32))
            {
                return value.GetIntValue();
            }
            else if (type == typeof(String))
            {
                return value.GetStringValue();
            }
            else if (type == typeof(Double))
            {
                return value.GetDoubleValue();
            }
            else if (type == typeof(DateTime))
            {
                return value.GetDateValue();
            }
            else if (type == typeof(bool))
            {
                return value.GetBoolValue();
            }
            else if (type == typeof(Callback))
            {
                var context = CefV8Context.GetCurrentContext();
                return new Callback(p =>
                    CefRuntime.PostTask(CefThreadId.Renderer,
                         new CefActionTask(() =>
                         {
                             context.Enter();
                             value.ExecuteFunctionWithContext(
                                 context,
                                 context.GetGlobal(),
                                 p.Select(x => MapValue(x)).ToArray());
                             context.Exit();
                         })));


            }
            else if (type == typeof(Int16))
            {
                return value.GetIntValue();
            }
            else if (type == typeof(Int64))
            {
                return value.GetIntValue();
            }
            else if (type == typeof(UInt32))
            {
                return value.GetUIntValue();
            }
            else if (type == typeof(UInt16))
            {
                return value.GetUIntValue();
            }
            else if (type == typeof(UInt64))
            {
                return value.GetUIntValue();
            }

            return null;
        }

    }
}
