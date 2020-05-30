using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using Webao.Attributes;
using WebaoDynamic;

namespace WebaoDynamicPart3
{
    public class Context
    {
        private readonly static List<Info> list = new List<Info>();

        private static void Add(Info info)
        {
            if (list.Find(item => item.Equals(info)) != null)
                list.Add(info);
        }

        private static Info Find(Info info)
        {
            return list.Find(item => item.Equals(info));
        }

        public Info info;
        public InfoMethod current;

        public Context(Type type, string url)
        {
            this.info = new Info(type);
            this.info.url = url;
        }


        public Context AddParameter(string name, string value)
        {
            if (info != null)
            {
                info.parameters.Add(name, value);
            }
            else
            {
                throw new Exception();
            }

            return this;
        }

        public Context On(string method)
        {
            if (info != null)
            {
                if (current == null || current.methodReturnType!=null)
                {
                    current = new InfoMethod(method);
                }
            }

            return this;
        }

        public Context GetFrom(string query)
        {
            if (info != null)
            {
                if (current != null && current.methodReturnType == null)
                {
                    current.query = query;
                }
            }

            return this;
        }

        public Context Mapping<T>(Func<T, object> func)
        {
            if (info != null)
            {
                if (current != null && current.methodReturnType == null)
                {
                    current.methodReturnType = typeof(T);
                    current.Del = func;

                    this.info.list.Add(current);
                }
            }

            return this;
        }

        public object Build3b(IRequest req)
        {
            /* Add to static context the information to user in emitter
             * and keep alive the delegate
             */
            Context.Add(info);

            Type type = info.returnType;
            TypeInfo typeInfo = info.returnType.GetTypeInfo();
            string TheName = "Emit" + type.Name;

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

            WebaoEmitter3b.ConstructorEmitter(constBuilder, info);

            /*
             * Refazer esta parte toda 
             * NOTA: que temos de identificar os parametros dos metodos
             * atraves do conjunto de chavetas {page}
             */

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

                InfoMethod im = info.list.Where(item => item.name== method.Name).FirstOrDefault();

                WebaoEmitter3b.MethodEmitter3b(methodBuilder, typeInfo, parametersInfo, im);
            }
            Type webaoType = typBuilder.CreateTypeInfo().AsType();

            asmBuilder.Save(DLL_NAME);

            return (object)Activator.CreateInstance(webaoType, req);
        }
    }
}
