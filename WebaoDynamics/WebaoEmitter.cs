﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using Webao.Dto;

namespace WebaoDynamics
{
    public class WebaoEmitter
    {
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
                LocalBuilder lbGeoGetToTracks = il.DeclareLocal(typeof(DtoGeoTopTracks));
                lbGeoGetToTracks.SetLocalSymInfo("retTopTracks");
                LocalBuilder lbTracks = il.DeclareLocal(typeof(DtoTracks));
                lbTracks.SetLocalSymInfo("retTracks");
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
                        il.EmitCall(OpCodes.Callvirt, typeof(object).GetMethod("ToString", new Type[0]), null);
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

            Type callReturnType = WebaoOps.GetMappingType(typeInfo, metBuilder.Name);
            il.Emit(OpCodes.Ldtoken, callReturnType);
            il.EmitCall(OpCodes.Call, callTypeOf, null);
            il.EmitCall(OpCodes.Call, baseGetRequest, null);

            string[] domains = WebaoOps.GetMappingDomain(typeInfo, metBuilder.Name).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo property = null;
            Type oldType = null;

            switch (metBuilder.Name)
            {
                case "GetInfo":
                case "Search":
                case "GetList":
                case "GetNationality":
                    il.Emit(OpCodes.Castclass, callReturnType);
                    oldType = callReturnType;
                    foreach (string domain in domains)
                    {
                        property = oldType.GetProperty(domain);
                        il.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                        oldType = property.PropertyType;
                    }
                    break;
                case "GeoGetTopTracks":
                    // unbox
                    il.Emit(OpCodes.Unbox_Any, callReturnType);

                    il.Emit(OpCodes.Stloc_1);
                    il.Emit(OpCodes.Ldloca_S, 1);
                    oldType = callReturnType;
                    property = oldType.GetProperty(domains[0]);
                    il.EmitCall(OpCodes.Call, property.GetGetMethod(), null);

                    il.Emit(OpCodes.Stloc_2);
                    il.Emit(OpCodes.Ldloca_S, 2);
                    oldType = property.PropertyType; ;
                    property = oldType.GetProperty(domains[1]);
                    il.EmitCall(OpCodes.Call, property.GetGetMethod(), null);
                    break;
                default:
                    il.Emit(OpCodes.Castclass, callReturnType);
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
