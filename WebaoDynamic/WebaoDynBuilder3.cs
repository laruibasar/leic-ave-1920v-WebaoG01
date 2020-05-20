using System;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{   

    public class WebaoDynBuilder3
    {
        //private delegate string WidthInvoker(string width);

        //private static WidthInvoker retWidthProperty { get; set; }

        public static object Build(Type type, IRequest req)
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


            //MethodInfo mInfo = typeInfo.GetMethod("MethodAZZZZ",
            //    BindingFlags.Public | BindingFlags.Instance,
            //    null,
            //    CallingConventions.Any, 
            //    new Type[] {  },
            //    null);

            //Delegate d = Delegate.CreateDelegate(typeInfo, mInfo);



            //retWidthProperty = d;
            //WidthInvoker wi = (WidthInvoker)constBuilder.CreateDelegate(typeof(WidthInvoker));                                 
            //string retval = d("DtoSearch.GetArtistsList");

            //retWidthProperty();







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

                WebaoEmitter.MethodEmitter(methodBuilder, typeInfo, parametersInfo);
            }
            Type webaoType = typBuilder.CreateTypeInfo().AsType();

            asmBuilder.Save(DLL_NAME);

            return (object)Activator.CreateInstance(webaoType, req);
        }
    }
}
