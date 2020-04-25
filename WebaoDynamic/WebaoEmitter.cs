using System;
using System.Reflection;
using System.Reflection.Emit;

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
            throw new NotImplementedException();
        }
    }
}
