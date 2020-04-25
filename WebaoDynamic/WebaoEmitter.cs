using System;
using System.Reflection;
using System.Reflection.Emit;
using Webao;

namespace WebaoDynamic
{
    public class WebaoEmitter
    {
        public static void MethodEmitter(MethodBuilder metBuilder, TypeInfo typeInfo)
        {
            WebaoDyn webao = (WebaoDyn)Activator.CreateInstance(typeof(WebaoDyn));
        }

        public static void ConstructorEmitter(ConstructorBuilder constBuilder, TypeInfo typeInfo)
        {
            ILGenerator il = constBuilder.GetILGenerator();

            // NOTE: ldarg.0 holds the "this" reference - ldarg.1, ldarg.2, and ldarg.3
            // hold the actual passed parameters. ldarg.0 is used by instance methods
            // to hold a reference to the current calling object instance. Static methods
            // do not use arg.0, since they are not instantiated and hence no reference
            // is needed to distinguish them.
            /*
                IL_0000: ldarg.0
                IL_0001: ldarg.1
                IL_0002: call instance void WebaoDynamic.WebaoDyn::.ctor([Webao]Webao.IRequest)
                IL_0007: ldarg.0
                IL_0008: ldstr      "https://www.boredapi.com/api/"
                IL_000d: call instance void WebaoDynamic.WebaoDyn::SetUrl(string)
                IL_0012: ldarg.0
                IL_0013: ldstr      "format"
                IL_0018: ldstr      "json"
                IL_001d: call instance void WebaoDynamic.WebaoDyn::SetParameter(string,
                                                                                string)

            */
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Call,
                typeof(IRequest).GetTypeInfo().GetMethod(
                "Write",
                new Type[] { typeof(string) }
            )
                );

            throw new NotImplementedException();
        }
    }
}
