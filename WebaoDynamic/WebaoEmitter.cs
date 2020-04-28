using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{
    public class WebaoEmitter
    {
        public static void MethodEmitter(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ParameterInfo[] parameterInfos)
        {
            ILGenerator il = metBuilder.GetILGenerator();

            /* Local variables
             *  - string path
             *  - Type (regarding return)
             */
            LocalBuilder lbPath = il.DeclareLocal(typeof(string));
            lbPath.SetLocalSymInfo("path");
            LocalBuilder lbReturnObj = il.DeclareLocal(typeof(Type));
            lbPath.SetLocalSymInfo("type");

            /* We call methods:
             *  - base.GetRequest
             *  - String.Replace
             *  - Type.GetTypeFromHandle(System.RuntimeTypeHandle) (typeof())
             */
            MethodInfo baseGetRequest = typeof(WebaoDyn).GetMethod(
                "GetRequest",
                new Type[] { typeof(string), typeof(Type) });
            MethodInfo callStringRpl = typeof(String).GetMethod(
                "Replace",
                new Type[] { typeof(string), typeof(string) });
            MethodInfo callTypeOf = typeof(Type).GetMethod(
                "GetTypeFromHandle",
                new Type[] { typeof(RuntimeTypeHandle) });

            il.Emit(OpCodes.Ldstr, WebaoOps.GetQuery(typeInfo, metBuilder.Name));

            if (parameterInfos.Length > 0)
            {
                foreach (ParameterInfo pi in parameterInfos)
                {
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldstr, "{" + pi.Name + "}");
                    il.Emit(OpCodes.Ldarga_S, pi.Position + 1); /* 0 -> this */
                    if (pi.ParameterType == typeof(string))
                    {
                        il.Emit(OpCodes.Callvirt, pi.ParameterType.GetMethod("ToString", new Type[0]));
                    }
                    else
                    {
                        il.Emit(OpCodes.Call, pi.ParameterType.GetMethod("ToString", new Type[0]));
                    }
                    il.EmitCall(OpCodes.Callvirt, callStringRpl, null);
                }
            }
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldtoken, WebaoOps.GetMappingType(typeInfo, metBuilder.Name));
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);

            string domain = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name);
            if (domain.Equals("."))
            {
                domain = "." + WebaoOps.GetMappingType(typeInfo, metBuilder.Name).Name;
            }
            Type returnType = Type.GetType("WebaoTestProject.Dto" + domain);
            il.Emit(OpCodes.Castclass, returnType);

            il.Emit(OpCodes.Ret);
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
