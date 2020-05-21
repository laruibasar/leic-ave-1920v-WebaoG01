using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    public class WebaoEmitter
    {
        private readonly static char[] separator = new char[] { '.' };

        public static void MethodEmitter3With(
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
            il.Emit(OpCodes.Ldtoken, WebaoOps.GetMappingType(typeInfo, metBuilder.Name));
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);



            //com with a partir daqui muda, o domain (path) está no with
            string with = WebaoOps.GetMappingWith(typeInfo, metBuilder.Name);     


            Type typeDto = WebaoOps.GetMappingType(typeInfo, metBuilder.Name);
            MethodInfo methodToDelegate = typeDto.GetMethod(with.Substring(with.LastIndexOf('.') + 1), BindingFlags.Public | BindingFlags.Instance);





            DynamicMethod myDelegate = new DynamicMethod("MyDelegate", typeof(List<Artist>), new Type[] { });
            //Delegate.CreateDelegate(typeof(List<Artist>), methodToDelegate);



            //il.Emit(OpCodes.Callvirt, myDelegate.Invoke());




            //string domain = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name);
            //Type returnType = null; 

            string domain = "." + WebaoOps.GetMappingType(typeInfo, metBuilder.Name).Name;
            Type returnType = Type.GetType("WebaoTestProject.Dto" + domain);



           


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

                    //PropertyInfo pii = returnType.GetProperty(mappingPath);
                    //mii = pii.GetGetMethod();
                    //il.EmitCall(OpCodes.Callvirt, mii, null);    
                    //break;                    

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

            string domain = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name);
            Type returnType = null;

            domain = "." + WebaoOps.GetMappingType(typeInfo, metBuilder.Name).Name;
            returnType = Type.GetType("WebaoTestProject.Dto" + domain);

            
            


            
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

                    //PropertyInfo pii = returnType.GetProperty(mappingPath);
                    //mii = pii.GetGetMethod();
                    //il.EmitCall(OpCodes.Callvirt, mii, null);    
                    //break;                    

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
    }
}
