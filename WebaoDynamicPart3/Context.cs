using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

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

        public Context DummyDel()
        {
            DtoResults results = new DtoResults
            {
                ArtistMatches = new DtoArtistMatches
                {
                    Artist = new List<Artist>() { new Artist { Name = "xpto artist" } }
                }
            };
            
            List<Artist> list = (List<Artist>)current.Del.DynamicInvoke(results);
            /*
               IL_0033: ldfld      class WebaoDynamicPart3.InfoMethod WebaoDynamicPart3.Context::current

               IL_0038:  callvirt instance [mscorlib] System.Delegate WebaoDynamicPart3.InfoMethod::get_Del()
               IL_003d:  ldc.i4.1

               IL_003e:  newarr[mscorlib] System.Object
               IL_0044:  ldc.i4.0
               IL_0045:  ldloc.0

               IL_0046:  stelem.ref
               IL_0047:  callvirt instance object[mscorlib] System.Delegate::DynamicInvoke(object[])
               IL_004c:  castclass[mscorlib] System.Collections.Generic.List`1<[WebaoTestProject] WebaoTestProject.Dto.Artist>
            */

            Console.WriteLine(list[0].Name);
            /*
               IL_0051:  ldc.i4.0

               IL_0052:  callvirt instance !0 [mscorlib] System.Collections.Generic.List`1<[WebaoTestProject] WebaoTestProject.Dto.Artist>::get_Item(int32)
               IL_0057:  callvirt instance string[WebaoTestProject] WebaoTestProject.Dto.Artist::get_Name()
               IL_005c:  call       void[mscorlib] System.Console::WriteLine(string)
               IL_0061:  ldarg.0
            */
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
                asmBuilder.DefineDynamicModule(MOD_NAME, DLL_NAME, true);

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
