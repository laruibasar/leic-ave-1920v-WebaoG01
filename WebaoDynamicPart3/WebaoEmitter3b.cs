using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using WebaoDynamic;
using WebaoDynamicPart3;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamicPart3
{
    public class WebaoEmitter3b
    {
        private readonly static char[] separator = new char[] { '.' };

        public static void MethodEmitter3b(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ParameterInfo[] parameterInfos,
            InfoMethod infoMethod
            )
        {
            ILGenerator il = metBuilder.GetILGenerator();

            /* Local variables
             *  - string path
             *  - Type (regarding return)
             */
            LocalBuilder lbPath = il.DeclareLocal(typeof(string));
            lbPath.SetLocalSymInfo("path");

            if (metBuilder.Name.Equals("GeoGetTopTracks"))
            {
                LocalBuilder vt1 = il.DeclareLocal(typeof(DtoGeoTopTracks));
                lbPath.SetLocalSymInfo("vt1");

                LocalBuilder vt2 = il.DeclareLocal(typeof(DtoTracks));
                lbPath.SetLocalSymInfo("vt2");
            }


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

            il.Emit(OpCodes.Ldstr, infoMethod.query);

            if (parameterInfos.Length > 0)
            {
                foreach (ParameterInfo pi in parameterInfos)
                {
                    il.Emit(OpCodes.Stloc_0);
                    il.Emit(OpCodes.Ldloc_0);
                    il.Emit(OpCodes.Ldstr, "{" + pi.Name + "}");

                    if (pi.ParameterType == typeof(string))
                    {
                        il.Emit(OpCodes.Ldarg, pi.Position + 1); /* 0 -> this */
                        il.Emit(OpCodes.Callvirt, typeof(object).GetMethod("ToString", new Type[0]));
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarga_S, pi.Position + 1); /* 0 -> this */
                        il.Emit(OpCodes.Call, pi.ParameterType.GetMethod("ToString", new Type[0]));
                    }
                    il.EmitCall(OpCodes.Callvirt, callStringRpl, null);
                }
            }
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldtoken, infoMethod.methodReturnType);
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);


        //    IL_007d: stloc.0
        //    IL_007e: ldarg.0
        //    IL_007f: call instance[mscorlib]System.Delegate WebaoDynDummy.WebaoArtistDummy3b::get_Del()
        //    IL_0084: ldc.i4.1
        //    IL_0085: newarr[mscorlib]System.Object
        //    IL_008a:  dup    
        //    IL_008b:  ldc.i4.0
        //    IL_008c: ldloc.0
        //    IL_008d: stelem.ref
        //    IL_008e:  callvirt instance object[mscorlib] System.Delegate::DynamicInvoke(object[])
        //    IL_0093: castclass[mscorlib]System.Collections.Generic.List`1 <[WebaoTestProject]WebaoTestProject.Dto.Artist >
        //    IL_0098:  ret


//    IL_0061: ldc.i4.1
//    IL_0062: newarr[mscorlib]System.Object
//    IL_0067:  dup
//    IL_0068:  ldc.i4.0
//    IL_0069: ldloc.1
//    IL_006a: stelem.ref
//    IL_006b:  callvirt instance object[mscorlib] System.Delegate::DynamicInvoke(object[])
//    IL_0070: castclass[mscorlib]System.Collections.Generic.List`1 <[WebaoTestProject]WebaoTestProject.Dto.Artist >
//    IL_0075:  ret





            il.Emit(OpCodes.Ldftn, infoMethod.Del.GetMethodInfo());
            ConstructorInfo ctor = typeof(Func<object>).GetConstructor(new Type[] { typeof(object), typeof(System.IntPtr) });

            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Callvirt, typeof(Func<object>).GetMethod("DynamicInvoke"));
            il.Emit(OpCodes.Ret);
        }

        

        public static void ConstructorEmitter(ConstructorBuilder constBuilder, Info info)
        {
            ILGenerator il = constBuilder.GetILGenerator();

            /* We call base constructor with req
             * And methods in the construtor:
             *  - base.SetUrl(string)
             *  - base.SetParameter(string , string)
             */
            ConstructorInfo baseCtor = typeof(WebaoDyn).GetConstructor(
                BindingFlags.Public |
                BindingFlags.Instance,
                null,
                CallingConventions.Any,
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
            il.Emit(OpCodes.Ldstr, info.url);
            il.EmitCall(OpCodes.Call, baseSetUrl, null);

            // Call to SetParameter n times
            foreach (KeyValuePair<string, string> p in info.parameters)
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
