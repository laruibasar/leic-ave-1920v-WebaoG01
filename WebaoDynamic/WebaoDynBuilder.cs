﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{
    public class WebaoDynBuilder
    {
        public static WebaoDyn Build(Type type, IRequest req)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            string TheName = typeInfo.Name + "Implementation";

            string ASM_NAME = TheName;
            string MOD_NAME = TheName;
            string TYP_NAME = TheName;

            string DLL_NAME = TheName + ".dll";

            // Define assembly
            AssemblyBuilder asmBuilder =
                AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName(ASM_NAME),
                    AssemblyBuilderAccess.RunAndSave
                );

            // Define module in assembly
            ModuleBuilder modBuilder =
                asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME);

            // Define type in module
            TypeBuilder typBuilder = modBuilder.DefineType(TYP_NAME);
            typBuilder.SetParent(typeof(WebaoDyn));

            typBuilder.AddInterfaceImplementation(type);

            ConstructorBuilder constBuilder =
                typBuilder.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.Standard,
                    new Type[] { typeof(IRequest) }
                );
            WebaoEmitter.ConstructorEmitter(constBuilder, typeInfo);
            /*
            MethodInfo[] methods = WebaoOps.GetMethods(type);
            foreach (MethodInfo method in methods)
            {
                MethodBuilder methodBuilder =
                    typBuilder.DefineMethod(
                        method.Name,
                        MethodAttributes.Public |
                        MethodAttributes.Virtual |
                        MethodAttributes.NewSlot
                        // method return,
                        // method parameters new Type[] { typeof()....}
                        );

                WebaoEmitter.MethodEmitter(methodBuilder, typeInfo);
            }
            */
            Type webaoType = typBuilder.CreateTypeInfo().AsType();

            asmBuilder.Save(DLL_NAME);

            WebaoDyn webao = (WebaoDyn)Activator.CreateInstance(webaoType);

            return webao;
        }
    }
}
