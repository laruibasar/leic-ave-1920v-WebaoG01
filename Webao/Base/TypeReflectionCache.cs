using System;
using System.Collections.Generic;
using System.Reflection;
using Webao.Attributes;

namespace Webao.Base
{
    /*
     * Associates Types to their corresponding TypeInformation objects.
     * The latter contains reflection info (e.g., custom attributes info)
     * obtained only once.
     */
    public class TypeInfoCache
    {
        private readonly static Dictionary<Type, TypeInformation> dict = new Dictionary<Type, TypeInformation>();

        public static TypeInformation Get(Type type)
        {
            // Check if the type 'type' was already processed.
            if (!dict.TryGetValue(type, out TypeInformation typeInfo))
            {
                /*
                 * If not yet consulted, create a TypeInformation object for
                 * this type, and add the pair (type, typeInfo) to dictionary.
                 * The TypeInformation contains reflection info (e.g., custom attributes info)
                 * obtained only once.
                 */
                typeInfo = new TypeInformation(type);
                dict.Add(type, typeInfo);
            }
            /*
             * Return the TypeInformation (newly created only once or the existing one)
             * for the type 'type'.
             */
            return typeInfo;
        }
    }

    public class TypeInformation
    {
        private Dictionary<string, List<Attribute>> customAttributes = new Dictionary<string, List<Attribute>>();

        /*
         * Search attributes from the object Type in arguments and add to the
         * dictionary list, in way of Key name, Value Attributes
         */
        public TypeInformation(Type type)
        {
            /* Check out the instance */
            Attribute[] typeAttributes = Attribute.GetCustomAttributes(type, true);
            if (typeAttributes.Length != 0)
            {
                setAttributes("", typeAttributes);
            }

            /* them on the members */
            MemberInfo[] membersInfo = type.GetMembers();
            foreach (MemberInfo mi in membersInfo)
            {
                Attribute[] miAttributes = Attribute.GetCustomAttributes(mi, true);
                if (miAttributes.Length != 0)
                {
                    setAttributes(mi.Name, miAttributes);
                }
            }
        }

        /*
         * Common method to parse the attributes
         */
        private void setAttributes(string prefix, Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                
                if (customAttributes.TryGetValue(prefix + attribute.GetType().FullName, out List<Attribute> attributeList))
                {
                    attributeList.Add(attribute);
                }
                else
                {
                    List<Attribute> list = new List<Attribute> { attribute };
                    customAttributes.Add(prefix + attribute.GetType().FullName, list);
                }
            }
        }

        /*
         * Use of custom indexers to get value from a key
         */
        public List<Attribute> this[string attributeKey]
        {
            get {
                return customAttributes[attributeKey];
            }
        }
    }
}