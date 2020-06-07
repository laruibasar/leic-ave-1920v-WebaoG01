using System;
using System.Collections.Generic;
using Webao;

namespace WebaoDynamic.TP3Fluent
{
    public class ContextCache
    {
        private static readonly Dictionary<Type, Context> list =
            new Dictionary<Type, Context>();

        public static void Add(Context context)
        {
            Type type = context.info.returnType;
            list.Add(type, context);
        }

        public static Context Get(Type type)
        {
            if (list.TryGetValue(type, out Context context))
            {
                return context;
            }
            else
            {
                throw new NoContextInCacheException(type);
            }
        }
    }

    public class NoContextInCacheException : Exception
    {
        public NoContextInCacheException() : base("There is no Context that matches the type sent") { }

        public NoContextInCacheException(Type type) :
            base(String.Format("There is no Context that matches the type sent: {0}", type.FullName))
        { }
    }

    public class Context
    {
        public readonly Info info;
        private InfoMethod currentInfoMethod;

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
                if (currentInfoMethod == null || currentInfoMethod.methodReturnType != null)
                {
                    currentInfoMethod = new InfoMethod(method);
                }
            }
            else
            {
                throw new Exception();
            }

            return this;
        }

        public Context GetFrom(string query)
        {
            if (info != null)
            {
                if (currentInfoMethod != null && currentInfoMethod.methodReturnType == null)
                {
                    currentInfoMethod.query = query;
                }
            }
            else
            {
                throw new Exception();
            }

            return this;
        }

        public Context Mapping<T>(Func<T, object> func)
        {
            if (info != null)
            {
                if (currentInfoMethod != null && currentInfoMethod.methodReturnType == null)
                {
                    currentInfoMethod.methodReturnType = typeof(T);
                    currentInfoMethod.Del = func;

                    this.info.list.Add(currentInfoMethod);
                }
            }
            else
            {
                throw new Exception();
            }

            return this;
        }

        public object Build(IRequest req)
        {
            // Add context to cache for availability through execution
            ContextCache.Add(this);

            // Add current context to build information on normal Build()
            WebaoOps.SetContext(this);

            // Use normal Builder for code reuse
            return WebaoDynBuilder.Build(this.info.returnType, req);
        }
    }
}
