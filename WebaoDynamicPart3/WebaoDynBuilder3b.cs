using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{
    public class InfoMethod
    {
        public string method { get; set; }
        public string query { get; set; }
        public Type type { get; set; }
        public Delegate Del { get; set; }
    }

    public class Context
    {
        Dictionary<string, string> dicParameters = new Dictionary<string, string>();
        Dictionary<string, InfoMethod> dicMethods = new Dictionary<string, InfoMethod>();

        InfoMethod infoMethod;

        public Context AddParameter(string key, string value)
        {
            dicParameters.Add(key, value);
            return this;
        }

        public Context On(string method)
        {
            infoMethod = new InfoMethod();
            infoMethod.method = method;
            return this;
        }

        public Context GetFrom(string query)
        {
            infoMethod.query = query;
            return this;
        }

        public Context Mapping<T>(Func<T, object> func)
        {
            infoMethod.Del = func;
            dicMethods.Add(infoMethod.method, infoMethod);
            return this;
        }       
    }

    public class WebaoDynBuilder3b
    {
        public static Context For<T>(String baseUrl)
        {
            Context context = new Context();
            return context;
        }

        public static object Build3b(IRequest req)
        {           
            TypeInfo typeInfo = type.GetTypeInfo();
            string TheName = "Emit" + typeInfo.Name;

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
            TypeBuilder typBuilder =
                modBuilder.DefineType(TYP_NAME, TypeAttributes.Public);
            typBuilder.SetParent(typeof(WebaoDyn));

            typBuilder.AddInterfaceImplementation(type);

            ConstructorBuilder constBuilder =
                typBuilder.DefineConstructor(
                    MethodAttributes.Public,
                    CallingConventions.Standard,
                    new Type[1] { typeof(IRequest) }
                );

            WebaoEmitter.ConstructorEmitter(constBuilder, typeInfo);
            
            MethodInfo[] methods = WebaoOps.GetMethods(type);
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parametersInfo = method.GetParameters();
                Type[] methodParameters = new Type[parametersInfo.Length];
                int idx = 0;
                foreach (ParameterInfo parameterInfo in parametersInfo)
                {
                    methodParameters[idx++] = parameterInfo.ParameterType;
                }

                MethodBuilder methodBuilder =
                    typBuilder.DefineMethod(
                        method.Name,
                        MethodAttributes.Public |
                        MethodAttributes.Virtual |
                        MethodAttributes.NewSlot |
                        MethodAttributes.Final,
                        method.ReturnType,
                        methodParameters
                        );

                WebaoEmitter.MethodEmitter3With(methodBuilder, typeInfo, parametersInfo);
            }
            Type webaoType = typBuilder.CreateTypeInfo().AsType();

            asmBuilder.Save(DLL_NAME);

            return (object)Activator.CreateInstance(webaoType, req);
        }
    }
}
