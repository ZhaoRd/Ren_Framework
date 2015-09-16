// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Skymate.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using Fasterflect;

    using Skymate;

    /// <summary>
    /// The type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The s_predefined types.
        /// </summary>
        private static Type[] predefinedTypes;

        /// <summary>
        /// The s_predefined generic types.
        /// </summary>
        private static Type[] predefinedGenericTypes;

        /// <summary>
        /// Initializes static members of the <see cref="TypeExtensions"/> class.
        /// </summary>
        static TypeExtensions()
        {
            predefinedTypes = new[] { typeof(string), typeof(decimal), typeof(DateTime), typeof(TimeSpan), typeof(Guid) };
            predefinedGenericTypes = new[] { typeof(Nullable<>) };
        }

        /// <summary>
        /// The assembly qualified name without version.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            if (type.AssemblyQualifiedName == null)
            {
                return string.Empty;
            }
            var strArray = type.AssemblyQualifiedName.Split(new[] { ',' });
            return string.Format("{0},{1}", strArray[0], strArray[1]);
        }

        /// <summary>
        /// The is sequence type.
        /// </summary>
        /// <param name="seqType">
        /// The seq type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsSequenceType(this Type seqType)
        {
            return (((seqType != typeof(string))
                && (seqType != typeof(byte[])))
                && (seqType != typeof(char[])))
                && (FindIEnumerable(seqType) != null);
        }

        /// <summary>
        /// The is predefined simple type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsPredefinedSimpleType(this Type type)
        {
            if ((type.IsPrimitive && (type != typeof(IntPtr))) && (type != typeof(UIntPtr)))
            {
                return true;
            }

            return type.IsEnum || predefinedTypes.Any(t => t == type);
        }

        /// <summary>
        /// The is struct.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsStruct(this Type type)
        {
            if (type.IsValueType)
            {
                return !type.IsPredefinedSimpleType();
            }

            return false;
        }

        /// <summary>
        /// The is predefined generic type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsPredefinedGenericType(this Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }
            else
            {
                return false;
            }

            return predefinedGenericTypes.Any(t => t == type);
        }

        /// <summary>
        /// The is predefined type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsPredefinedType(this Type type)
        {
            if ((!IsPredefinedSimpleType(type) && !IsPredefinedGenericType(type)) && (type != typeof(byte[])))
            {
                return string.Compare(type.FullName, "System.Xml.Linq.XElement", StringComparison.Ordinal) == 0;
            }

            return true;
        }

        /// <summary>
        /// The is integer.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsInteger(this Type type)
        {

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The is nullable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullable(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// The is null assignable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsNullAssignable(this Type type)
        {
            return !type.IsValueType || type.IsNullable();
        }

        /// <summary>
        /// The is constructable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsConstructable(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");

            if (type.IsAbstract || type.IsInterface || type.IsArray || type.IsGenericTypeDefinition
                || type == typeof(void))
            {
                return false;
            }

            if (!HasDefaultConstructor(type))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The is anonymous.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsAnonymous(this Type type)
        {
            if (type.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
                    if (attributes.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The has default constructor.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type type)
        {
            Guard.ArgumentNotNull(() => type);

            if (type.IsValueType)
            {
                return true;
            }

            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Any(ctor => ctor.GetParameters().Length == 0);
        }

        /// <summary>
        /// The is sub class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="check">
        /// The check.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsSubClass(this Type type, Type check)
        {
            Type implementingType;
            return IsSubClass(type, check, out implementingType);
        }

        /// <summary>
        /// The is sub class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="check">
        /// The check.
        /// </param>
        /// <param name="implementingType">
        /// The implementing type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsSubClass(this Type type, Type check, out Type implementingType)
        {
            Guard.ArgumentNotNull(type, "type");
            Guard.ArgumentNotNull(check, "check");

            return IsSubClassInternal(type, type, check, out implementingType);
        }

        /// <summary>
        /// The is sub class internal.
        /// </summary>
        /// <param name="initialType">
        /// The initial type.
        /// </param>
        /// <param name="currentType">
        /// The current type.
        /// </param>
        /// <param name="check">
        /// The check.
        /// </param>
        /// <param name="implementingType">
        /// The implementing type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool IsSubClassInternal(Type initialType, Type currentType, Type check, out Type implementingType)
        {
            if (currentType == check)
            {
                implementingType = currentType;
                return true;
            }

            // don't get interfaces for an interface unless the initial type is an interface
            if (check.IsInterface && (initialType.IsInterface || currentType == initialType))
            {
                foreach (Type t in currentType.GetInterfaces())
                {
                    if (IsSubClassInternal(initialType, t, check, out implementingType))
                    {
                        // don't return the interface itself, return it's implementor
                        if (check == implementingType)
                            implementingType = currentType;

                        return true;
                    }
                }
            }

            if (currentType.IsGenericType && !currentType.IsGenericTypeDefinition)
            {
                if (IsSubClassInternal(initialType, currentType.GetGenericTypeDefinition(), check, out implementingType))
                {
                    implementingType = currentType;
                    return true;
                }
            }

            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }

            return IsSubClassInternal(initialType, currentType.BaseType, check, out implementingType);
        }

        /// <summary>
        /// The is indexed.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsIndexed(this PropertyInfo property)
        {
            Guard.ArgumentNotNull(property, "property");
            return !property.GetIndexParameters().IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the member is an indexed property.
        /// </summary>
        /// <param name="member">
        /// The member.
        /// </param>
        /// <returns>
        /// <c>true</c> if the member is an indexed property; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIndexed(this MemberInfo member)
        {
            Guard.ArgumentNotNull(member, "member");

            var propertyInfo = member as PropertyInfo;

            return propertyInfo != null && propertyInfo.IsIndexed();
        }

        /// <summary>
        /// Checks to see if the specified type is assignable.
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsType<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }

        /// <summary>
        /// The get single member.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="memberTypes">
        /// The member types.
        /// </param>
        /// <returns>
        /// The <see cref="MemberInfo"/>.
        /// </returns>
        public static MemberInfo GetSingleMember(this Type type, string name, MemberTypes memberTypes)
        {
            return type.GetSingleMember(
                name, 
                memberTypes, 
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// The get single member.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="memberTypes">
        /// The member types.
        /// </param>
        /// <param name="bindingAttr">
        /// The binding attr.
        /// </param>
        /// <returns>
        /// The <see cref="MemberInfo"/>.
        /// </returns>
        public static MemberInfo GetSingleMember(this Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr)
        {
            return type.GetMember(
                name, 
                memberTypes, 
                bindingAttr).SingleOrDefault();
        }

        /// <summary>
        /// The get name and assembly name.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetNameAndAssemblyName(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// The get fields and properties.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="bindingAttr">
        /// The binding attr.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<MemberInfo> GetFieldsAndProperties(this Type type, BindingFlags bindingAttr)
        {
            foreach (var fi in type.GetFields(bindingAttr))
            {
                yield return fi;
            }

            foreach (var pi in type.GetProperties(bindingAttr))
            {
                yield return pi;
            }
        }

        /// <summary>
        /// The get field or property.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="ignoreCase">
        /// The ignore case.
        /// </param>
        /// <returns>
        /// The <see cref="MemberInfo"/>.
        /// </returns>
        public static MemberInfo GetFieldOrProperty(this Type type, string name, bool ignoreCase)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            if (ignoreCase)
                flags |= BindingFlags.IgnoreCase;

            return type.GetSingleMember(
                name, 
                MemberTypes.Field | MemberTypes.Property, 
                flags);
        }

        /// <summary>
        /// The find members.
        /// </summary>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="memberType">
        /// The member type.
        /// </param>
        /// <param name="bindingAttr">
        /// The binding attr.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="filterCriteria">
        /// The filter criteria.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public static List<MemberInfo> FindMembers(this Type targetType, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            Guard.ArgumentNotNull(targetType, "targetType");

            List<MemberInfo> memberInfos = new List<MemberInfo>(targetType.FindMembers(memberType, bindingAttr, filter, filterCriteria));

            // fix weirdness with FieldInfos only being returned for the current Type
            // find base type fields and add them to result
            if ((memberType & MemberTypes.Field) != 0
              && (bindingAttr & BindingFlags.NonPublic) != 0)
            {
                // modify flags to not search for public fields
                BindingFlags nonPublicBindingAttr = bindingAttr ^ BindingFlags.Public;

                while ((targetType = targetType.BaseType) != null)
                {
                    memberInfos.AddRange(targetType.FindMembers(MemberTypes.Field, nonPublicBindingAttr, filter, filterCriteria));
                }
            }

            return memberInfos;
        }

        /// <summary>
        /// The create generic.
        /// </summary>
        /// <param name="genericTypeDefinition">
        /// The generic type definition.
        /// </param>
        /// <param name="innerType">
        /// The inner type.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CreateGeneric(this Type genericTypeDefinition, Type innerType, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, new[] { innerType }, args);
        }

        /// <summary>
        /// The create generic.
        /// </summary>
        /// <param name="genericTypeDefinition">
        /// The generic type definition.
        /// </param>
        /// <param name="innerTypes">
        /// The inner types.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, innerTypes, (t, a) => Activator.CreateInstance(t, args));
        }

        /// <summary>
        /// The create generic.
        /// </summary>
        /// <param name="genericTypeDefinition">
        /// The generic type definition.
        /// </param>
        /// <param name="innerTypes">
        /// The inner types.
        /// </param>
        /// <param name="instanceCreator">
        /// The instance creator.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, Func<Type, object[], object> instanceCreator, params object[] args)
        {
            Guard.ArgumentNotNull(() => genericTypeDefinition);
            Guard.ArgumentNotNull(() => innerTypes);
            Guard.ArgumentNotNull(() => instanceCreator);
			if (innerTypes.Length == 0)
				throw Error.Argument("innerTypes", "The sequence must contain at least one entry.");

            Type specificType = genericTypeDefinition.MakeGenericType(innerTypes);

            return instanceCreator(specificType, args);
        }

        /// <summary>
        /// The create generic list.
        /// </summary>
        /// <param name="listType">
        /// The list type.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public static IList CreateGenericList(this Type listType)
        {
            Guard.ArgumentNotNull(listType, "listType");
            return (IList)typeof(List<>).CreateGeneric(listType);
        }

        /// <summary>
        /// The is enumerable.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsEnumerable(this Type type)
        {
            Guard.ArgumentNotNull(type, "type");
            return type.IsAssignableFrom(typeof(IEnumerable));
        }

        /// <summary>
        /// The is generic dictionary.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsGenericDictionary(this Type type)
        {
            if (type.IsInterface && type.IsGenericType)
            {
                return typeof(IDictionary<,>).Equals(type.GetGenericTypeDefinition());
            }

            return type.GetInterface(typeof(IDictionary<,>).Name) != null;
        }

        /// <summary>
        /// Gets the member's value on the object.
        /// </summary>
        /// <param name="member">
        /// The member.
        /// </param>
        /// <param name="target">
        /// The target object.
        /// </param>
        /// <returns>
        /// The member's value on the object.
        /// </returns>
        public static object GetValue(this MemberInfo member, object target)
        {
            Guard.ArgumentNotNull(member, "member");
            Guard.ArgumentNotNull(target, "target");

            var type = target.GetType();

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return target.GetFieldValue(member.Name);

                    // return ((FieldInfo)member).GetValue(target);
                case MemberTypes.Property:
                    return target.GetPropertyValue(member.Name);
                default:
                    throw new ArgumentException(
                        "MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatInvariant(member.Name), 
                        "member");
            }
        }

        /// <summary>
        /// Sets the member's value on the target object.
        /// </summary>
        /// <param name="member">
        /// The member.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetValue(this MemberInfo member, object target, object value)
        {
            Guard.ArgumentNotNull(member, "member");
            Guard.ArgumentNotNull(target, "target");

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    target.SetFieldValue(member.Name, value);
                    break;

                    // return ((FieldInfo)member).GetValue(target);
                case MemberTypes.Property:
                    try
                    {
                        target.SetPropertyValue(member.Name, value);
                    }
                    catch (TargetParameterCountException e)
                    {
                        throw new ArgumentException(
                            "PropertyInfo '{0}' has index parameters".FormatInvariant(member.Name), 
                            "member", 
                            e);
                    }

                    break;
                default:
                    throw new ArgumentException(
                        "MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatInvariant(member.Name), 
                        "member");
            }
        }

        /// <summary>
        /// Gets the underlying type of a <see cref="Nullable{T}"/> type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetNonNullableType(this Type type)
        {
            if (!IsNullable(type))
            {
                return type;
            }

            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be read.
        /// </summary>
        /// <param name="member">
        /// The MemberInfo to determine whether can be read.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified MemberInfo can be read; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// For methods this will return <c>true</c> if the return type
        /// is not <c>void</c> and the method is parameterless.
        /// </remarks>
        public static bool CanReadValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanRead;
                case MemberTypes.Method:
                    MethodInfo mi = (MethodInfo)member;
                    return mi.ReturnType != typeof(void) && mi.GetParameters().Length == 0;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be set.
        /// </summary>
        /// <param name="member">
        /// The MemberInfo to determine whether can be set.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified MemberInfo can be set; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanSetValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanWrite;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns single attribute from the type
        /// </summary>
        /// <typeparam name="T">
        /// Attribute to use
        /// </typeparam>
        /// <param name="target">
        /// Attribute provider
        /// </param>
        /// <param name="inherits">
        /// The inherits.
        /// </param>
        /// <returns>
        /// <em>Null</em> if the attribute is not found
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// If there are 2 or more attributes
        /// </exception>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target.GetCustomAttributes(typeof(TAttribute), inherits);
                if (attributes.Length > 1)
                {
                    throw Error.MoreThanOneElement();
                }

                return (TAttribute)attributes[0];
            }

            return null;

        }

        /// <summary>
        /// The has attribute.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="inherits">
        /// The inherits.
        /// </param>
        /// <typeparam name="TAttribute">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            return target.IsDefined(typeof(TAttribute), inherits);
        }

        /// <summary>
        /// Given a particular MemberInfo, return the custom attributes of the
        /// given type on that member.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute to retrieve.
        /// </typeparam>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="inherits">
        /// True to include attributes inherited from base classes.
        /// </param>
        /// <returns>
        /// Array of found attributes.
        /// </returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target
                    .GetCustomAttributes(typeof(TAttribute), inherits)
                    .Cast<TAttribute>();

                return SortAttributesIfPossible(attributes).ToArray();

                
                // return target
                // .GetCustomAttributes(typeof(TAttribute), inherits)
                // .ToArray(a => (TAttribute)a);
                
            }

            return new TAttribute[0];

        }

        /// <summary>
        /// Given a particular MemberInfo, find all the attributes that apply to this
        /// member. Specifically, it returns the attributes on the type, then (if it's a
        /// property accessor) on the property, then on the member itself.
        /// </summary>
        /// <typeparam name="TAttribute">
        /// Type of attribute to retrieve.
        /// </typeparam>
        /// <param name="member">
        /// The member to look at.
        /// </param>
        /// <param name="inherits">
        /// true to include attributes inherited from base classes.
        /// </param>
        /// <returns>
        /// Array of found attributes.
        /// </returns>
        public static TAttribute[] GetAllAttributes<TAttribute>(this MemberInfo member, bool inherits)
            where TAttribute : Attribute
        {
            List<TAttribute> attributes = new List<TAttribute>();

            if (member.DeclaringType != null)
            {
                attributes.AddRange(GetAttributes<TAttribute>(member.DeclaringType, inherits));

                MethodBase methodBase = member as MethodBase;
                if (methodBase != null)
                {
                    PropertyInfo prop = GetPropertyFromMethod(methodBase);
                    if (prop != null)
                    {
                        attributes.AddRange(GetAttributes<TAttribute>(prop, inherits));
                    }
                }
            }

            attributes.AddRange(GetAttributes<TAttribute>(member, inherits));
            return attributes.ToArray();
        }

        /// <summary>
        /// The sort attributes if possible.
        /// </summary>
        /// <param name="attributes">
        /// The attributes.
        /// </param>
        /// <typeparam name="TAttribute">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        internal static IEnumerable<TAttribute> SortAttributesIfPossible<TAttribute>(IEnumerable<TAttribute> attributes)
            where TAttribute : Attribute
        {
            if (typeof(IOrdered).IsAssignableFrom(typeof(TAttribute)))
            {
                return attributes.Cast<IOrdered>().OrderBy(x => x.Ordinal).Cast<TAttribute>();
            }

            return attributes;
        }

        /// <summary>
        /// Given a MethodBase for a property's get or set method,
        /// return the corresponding property info.
        /// </summary>
        /// <param name="method">
        /// MethodBase for the property's get or set method.
        /// </param>
        /// <returns>
        /// PropertyInfo for the property, or null if method is not part of a property.
        /// </returns>
        public static PropertyInfo GetPropertyFromMethod(this MethodBase method)
        {
            Guard.ArgumentNotNull(method, "method");

            PropertyInfo property = null;
            if (method.IsSpecialName)
            {
                Type containingType = method.DeclaringType;
                if (containingType != null)
                {
                    if (method.Name.StartsWith("get_", StringComparison.InvariantCulture) ||
                        method.Name.StartsWith("set_", StringComparison.InvariantCulture))
                    {
                        string propertyName = method.Name.Substring(4);
                        property = containingType.GetProperty(propertyName);
                    }
                }
            }

            return property;
        }

        /// <summary>
        /// The find i enumerable.
        /// </summary>
        /// <param name="seqType">
        /// The seq type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        internal static Type FindIEnumerable(this Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                        return ienum;
                }
            }

            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null)
                        return ienum;
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
                return FindIEnumerable(seqType.BaseType);
            return null;
        }
    }

}
