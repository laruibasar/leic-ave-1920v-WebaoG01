using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using WebaoDynamic.TP3Fluent;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    public class WebaoEmitter
    {
        private readonly static char[] separator = new char[] { '.' };

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

    public static void MethodEmitter(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ParameterInfo[] parameterInfos
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

            il.Emit(OpCodes.Ldstr, WebaoOps.GetQuery(typeInfo, metBuilder.Name));

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
                        il.Emit(OpCodes.Callvirt,  typeof(object).GetMethod("ToString", new Type[0]));
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
            il.Emit(OpCodes.Ldtoken, WebaoOps.GetMappingType(typeInfo, metBuilder.Name));
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);

            /* Call part to get return object to end
             * method section for return
             */
            if (WebaoOps.IsContextSet())
            {
                MethodEmitterContextReturn(metBuilder, typeInfo, il);
            }
            else if (WebaoOps.GetMappingWith(typeInfo, metBuilder.Name) != null)
            {
                MethodEmitterReturnWith(metBuilder, typeInfo, il);
            }
            else
            {
                MethodEmitterReturn(metBuilder, typeInfo, il);
            }

            il.Emit(OpCodes.Ret);
        }

        private static void MethodEmitterContextReturn(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ILGenerator il)
        {
            /* We need to load context from the method in execution
             * and them access the right delegate to do the Invoke
             * 
             * The C# code we want to emit is:
             * (List<Artist>)ContextCache.Get(typeof(WebaoArtist))
             *  .info
             *  .GetMethodDelegate("Search")
             *  .DynamicInvoke(results); 
             */

            /* USE THIS FROM TEST: TestLoadAndUseContext */
            //    IL_0000:  ldtoken WebaoTestProject.WebaoArtist
            il.Emit(OpCodes.Ldtoken, typeInfo);
            //    IL_0005:  call[mscorlib] System.Type[mscorlib] System.Type::GetTypeFromHandle([mscorlib] System.RuntimeTypeHandle)
            il.Emit(OpCodes.Call,
                typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
            //    IL_000a:  call class WebaoDynamic.TP3Fluent.Context WebaoDynamic.TP3Fluent.ContextCache::Get([mscorlib] System.Type)
            il.Emit(OpCodes.Call, typeof(ContextCache).GetMethod("Get", new Type[] { typeof(Type) }));
            // Failed generated from here:
//Unhandled Exception:
//System.ArgumentOutOfRangeException: Token 0x0100000b is not valid in the scope of module Dummy.
//Parameter name: metadataToken
//  at IKVM.Reflection.Reader.ModuleReader.ResolveField(System.Int32 metadataToken, IKVM.Reflection.Type[] genericTypeArguments, IKVM.Reflection.Type[] genericMethodArguments)[0x0008c] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.ResolveField(System.Int32 token, IKVM.Reflection.Type[] genericTypeArguments, IKVM.Reflection.Type[] genericMethodArguments)[0x00000] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.WriteIL(Ildasm.LineWriter lw, IKVM.Reflection.MethodBase mb, IKVM.Reflection.MethodBody body, IKVM.Reflection.Type[] genericTypeArguments, IKVM.Reflection.Type[] genericMethodArguments)[0x005be] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.WriteMethod(Ildasm.LineWriter lw, IKVM.Reflection.MethodBase method)[0x007bb] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.WriteType(Ildasm.LineWriter lw, IKVM.Reflection.Type type)[0x004a8] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.WriteTypes(Ildasm.LineWriter lw)[0x00039] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Disassembler.Save(System.IO.TextWriter writer)[0x00082] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//  at Ildasm.Program.Main(System.String[] args)[0x003ce] in < 563ef0f89b9a4a28a403624fc051b175 >:0
//[ERROR] FATAL UNHANDLED EXCEPTION: System.ArgumentOutOfRangeException: Token 0x0100000b is not valid in the scope of module Dummy.

            //    IL_000f:  ldfld class WebaoDynamic.TP3Fluent.Info WebaoDynamic.TP3Fluent.Context::info
            il.Emit(OpCodes.Ldfld, typeof(Context).GetField("info"));
            //  IL_0014:  ldstr      "Search"
            il.Emit(OpCodes.Ldstr, metBuilder.Name);
            //  IL_0019:  callvirt instance [mscorlib] System.Delegate WebaoDynamic.TP3Fluent.Info::GetMethodDelegate(string)
            il.Emit(OpCodes.Callvirt, typeof(Info).GetMethod("GetMethodDelegate", new Type[] { typeof(string) }));
            //  IL_001e:  ldc.i4.1
            il.Emit(OpCodes.Ldc_I4_1);
            //  IL_001f:  newarr[mscorlib] System.Object
            il.Emit(OpCodes.Newarr, typeof(Object));
            //  IL_0024:  dup
            //  IL_0025:  ldc.i4.0
            il.Emit(OpCodes.Ldc_I4_0);
            //  IL_0026:  ldsfld     class WebaoTestProject.Dto.DtoSearch WebaoTestProject.WebaoDynamicTest3B::results
            il.Emit(OpCodes.Ldsfld, WebaoOps.GetMappingType(typeInfo, metBuilder.Name));
            //  IL_002b:  stelem.ref
            il.Emit(OpCodes.Stelem_Ref);
            //  IL_002c:  callvirt instance object[mscorlib] System.Delegate::DynamicInvoke(object[])
            il.Emit(OpCodes.Callvirt, typeof(Delegate).GetMethod("DynamicInvoke"));
        }

        private static void MethodEmitterReturnWith(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ILGenerator il)
        {
            string with = WebaoOps.GetMappingWith(typeInfo, metBuilder.Name);
            with = with.Substring(with.LastIndexOf('.') + 1);

            Type typeDto = WebaoOps.GetMappingType(typeInfo, metBuilder.Name);
            MethodInfo methodToDelegate = typeDto.GetMethod(with, BindingFlags.Public | BindingFlags.Instance);

            //IL_0041: ldftn instance[mscorlib]System.Collections.Generic.List`1 <class WebaoTestProject.Dto.Artist> WebaoTestProject.Dto.DtoSearch::GetArtistsList()
            //IL_0047:  newobj instance void WebaoDynDummy.MyDelegate::.ctor(object, native int)
            //IL_004c:  callvirt instance[mscorlib]System.Collections.Generic.List`1<class WebaoTestProject.Dto.Artist> WebaoDynDummy.MyDelegate::Invoke()
            //IL_0051:  ret
            il.Emit(OpCodes.Ldftn, methodToDelegate);
            ConstructorInfo ctor = typeof(Func<object>).GetConstructor(
                new Type[] { typeof(object), typeof(System.IntPtr) });

            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Callvirt, typeof(Func<object>).GetMethod("Invoke"));
        }

        private static void MethodEmitterReturn(
            MethodBuilder metBuilder,
            TypeInfo typeInfo,
            ILGenerator il)
        {
            string ns = "WebaoTestProject.Dto";
            string domain = WebaoOps.GetMappingType(typeInfo, metBuilder.Name).Name;

            Type returnType = Type.GetType(ns + "." + domain);

            /* treat .Tracks.track kind of domain
             * We have to deal with every strange method, so a switch can be
             * the best option, with fallthrough for reduce clutter
             */
            String mappingPath = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name).Substring(1);
            MethodInfo mii;
            string[] domains = mappingPath.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            switch (metBuilder.Name)
            {
                case "GetInfo":
                case "Search":
                case "GetNationality":
                case "GetList":
                    il.Emit(OpCodes.Castclass, returnType);
                    Type tii = returnType;
                    PropertyInfo prop;

                    foreach (string d in domains)
                    {
                        prop = tii.GetProperty(d);
                        tii = prop.PropertyType;
                        mii = prop.GetGetMethod();
                        il.EmitCall(OpCodes.Callvirt, mii, null);
                    }
                    break;
                case "GeoGetTopTracks":
                    il.Emit(OpCodes.Unbox_Any, returnType);
                    il.Emit(OpCodes.Stloc_1);
                    il.Emit(OpCodes.Ldloca_S, 1);
                    il.Emit(OpCodes.Call, returnType.GetProperty(domains[0]).GetGetMethod());

                    il.Emit(OpCodes.Stloc_2);
                    il.Emit(OpCodes.Ldloca_S, 2);
                    il.Emit(OpCodes.Call, returnType.GetProperty(domains[0]).PropertyType.GetProperty(domains[1]).GetGetMethod());
                    break;
                default:
                    il.Emit(OpCodes.Castclass, returnType);
                    break;
            }
        }
    }
}
