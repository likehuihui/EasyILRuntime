using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class DelegateDemo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(DelegateDemo);

            field = type.GetField("TestMethodDelegate", flag);
            app.RegisterCLRFieldGetter(field, get_TestMethodDelegate_0);
            app.RegisterCLRFieldSetter(field, set_TestMethodDelegate_0);
            field = type.GetField("TestFunctionDelegate", flag);
            app.RegisterCLRFieldGetter(field, get_TestFunctionDelegate_1);
            app.RegisterCLRFieldSetter(field, set_TestFunctionDelegate_1);
            field = type.GetField("TestActionDelegate", flag);
            app.RegisterCLRFieldGetter(field, get_TestActionDelegate_2);
            app.RegisterCLRFieldSetter(field, set_TestActionDelegate_2);


        }



        static object get_TestMethodDelegate_0(ref object o)
        {
            return DelegateDemo.TestMethodDelegate;
        }
        static void set_TestMethodDelegate_0(ref object o, object v)
        {
            DelegateDemo.TestMethodDelegate = (TestDelegateMethod)v;
        }
        static object get_TestFunctionDelegate_1(ref object o)
        {
            return DelegateDemo.TestFunctionDelegate;
        }
        static void set_TestFunctionDelegate_1(ref object o, object v)
        {
            DelegateDemo.TestFunctionDelegate = (TestDelegateFunction)v;
        }
        static object get_TestActionDelegate_2(ref object o)
        {
            return DelegateDemo.TestActionDelegate;
        }
        static void set_TestActionDelegate_2(ref object o, object v)
        {
            DelegateDemo.TestActionDelegate = (System.Action<System.String>)v;
        }


    }
}
