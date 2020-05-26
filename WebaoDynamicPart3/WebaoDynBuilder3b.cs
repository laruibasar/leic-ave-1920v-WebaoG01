using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using Webao.Attributes;
using Webao.Base;

namespace WebaoDynamic
{

    public class WebaoDynBuilder3b
    {
        public static Context context;
        public static Type type;

        public static Context For<T>(string baseUrl)
        {
            type = typeof(T);
            TypeInformation typeInfo = TypeInfoCache.Get(typeof(T));

            BaseUrlAttribute baseUrlAttribute = new BaseUrlAttribute(baseUrl);
            Attribute[] attributes = { baseUrlAttribute };
            typeInfo.setAttributes(baseUrlAttribute.GetType().FullName, attributes);

            return new Context(typeInfo, type);
        }
    }

    public class InfoMethod
    {
        public string method { get; set; }
        public string query { get; set; }
        public Type type { get; set; }
        public Delegate Del { get; set; }
    }

    public static class Context
    {
        Type t;
        public Dictionary<string, InfoMethod> dicMethods;
        public Dictionary<string, string> dicParameters;

        InfoMethod infoMethod;
        TypeInformation typeInfo;
        Type type;

        public Context(TypeInformation typeInfo, Type type) {
            dicMethods = new Dictionary<string, InfoMethod>();
            this.typeInfo = typeInfo;
            this.type = type;
        }

        public Context AddParameter(string name, string value)
        {
            AddParameterAttribute addParameter = new AddParameterAttribute(name, value);
            Attribute[] attributes = { addParameter };
            typeInfo.setAttributes(addParameter.GetType().FullName, attributes); 
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
            GetAttribute getAttribute = new GetAttribute(query);
            Attribute[] attributes = { getAttribute };
            typeInfo.setAttributes(infoMethod.method+getAttribute.GetType().FullName, attributes);
            return this;
        } 

        public Context Mapping<T>(Func<T, object> func)
        {  
            infoMethod.Del = func;
            dicMethods.Add(infoMethod.method, infoMethod);
            return this; 
        } 

        public object Build3b(IRequest req)
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
