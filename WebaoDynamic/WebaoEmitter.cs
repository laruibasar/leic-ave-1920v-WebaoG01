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
            /*
            .locals init (string V_0)
            IL_0000:  ldstr      "activity\?key={key}"
            IL_0005:  stloc.0
            IL_0006:  ldloc.0
            IL_0007:  ldstr      "{key}"
            IL_000c:  ldarga.s   key
            IL_000e:  call       instance string [mscorlib]System.Int32::ToString()
            IL_0013:  callvirt   instance string [mscorlib]System.String::Replace(string,
                                                                                  string)
            IL_0018:  stloc.0
            IL_0019:  ldarg.0
            IL_001a:  ldloc.0
            IL_001b:  ldtoken    WebaoTestProject.Dto.Boredom
            IL_0020:  call       [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle([mscorlib]System.RuntimeTypeHandle)
            IL_0025:  call       instance object WebaoDynamic.WebaoDyn::GetRequest(string,
                                                                                   [mscorlib]System.Type)
            IL_002a:  castclass  WebaoTestProject.Dto.Boredom
            IL_002f:  ret
             */

            il.Emit(OpCodes.Ldstr, WebaoOps.GetQuery(typeInfo, metBuilder.Name));

            //replace args if exists
            if (parameterInfos.Length > 0)
            {
                foreach (ParameterInfo pi in parameterInfos)
                {
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldstr, "{" + pi.Name + "}");
                    il.Emit(OpCodes.Ldarga_S, pi.Position);
                    il.Emit(OpCodes.Call, pi.ParameterType.GetMethod("ToString", new Type[0]));
                    il.EmitCall(OpCodes.Callvirt, callStringRpl, null);
                }
            }
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldtoken, WebaoOps.GetMappingType(typeInfo, metBuilder.Name));
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);

            string domain = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name);
            //il.Emit(OpCodes.Castclass, );

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
