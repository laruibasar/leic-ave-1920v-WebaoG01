using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{
    public class WebaoEmitter
    {
        public static void MethodEmitter(MethodBuilder metBuilder, TypeInfo typeInfo)
        {
            ILGenerator il = metBuilder.GetILGenerator();

            /* We call methods:
             *  - base.GetRequest
             *  - String.Replace
             *  - (sometimes) xx ToString()
             *  - Type.GetTypeFromHandle(System.RuntimeTypeHandle) (typeof())
             */
        }

        public static void ConstructorEmitter(ConstructorBuilder constBuilder, TypeInfo typeInfo)
        {
            ILGenerator il = constBuilder.GetILGenerator();

            /* We call base constructor with req
             * And methods in the construtor:
             *  - base.SetUrl(string)
             *  - base.SetParameter(string , string)
             */
            ConstructorInfo baseCtor = typeof(WebaoDyn).GetConstructor(
                BindingFlags.Public |
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance,
                null,
                new Type[] { typeof(IRequest) },
                null);

            MethodInfo baseSetUrl = typeof(WebaoDyn).GetMethod(
                "SetUrl",
                new Type[] { typeof(string) });

            MethodInfo baseSetParameter = typeof(WebaoDyn).GetMethod(
                "SetParameter",
                new Type[] { typeof(string), typeof(string) });

            /* example from WebaoBoredomDummy
                IL_0000: ldarg.0
                IL_0001: ldarg.1
                IL_0002: call instance void WebaoDynamic.WebaoDyn::.ctor([Webao]Webao.IRequest)
                IL_0007: ldarg.0
                IL_0008: ldstr      "https://www.boredapi.com/api/"
                IL_000d: call instance void WebaoDynamic.WebaoDyn::SetUrl(string)
                IL_0012: ldarg.0
                IL_0013: ldstr      "format"
                IL_0018: ldstr      "json"
                IL_001d: call instance void WebaoDynamic.WebaoDyn::SetParameter(string,
                                                                                string)
            */
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call, baseCtor);

            // Call to SetUrl
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, WebaoOps.GetUrl(typeInfo));
            il.EmitCall(OpCodes.Call, baseSetUrl, null);

            // Call to SetParameter n times
            Dictionary<string, string> parameters =
                WebaoOps.GetParameters(typeInfo);
            foreach (KeyValuePair<string, string> p in parameters)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, p.Key);
                il.Emit(OpCodes.Ldstr, p.Value);
                il.EmitCall(OpCodes.Call, baseSetParameter, null);
            }

            il.Emit(OpCodes.Ret);
        }
    }
}
