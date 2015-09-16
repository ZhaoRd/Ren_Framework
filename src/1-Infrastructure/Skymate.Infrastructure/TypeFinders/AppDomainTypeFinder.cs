// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDomainTypeFinder.cs" company="Skymate">
//   Copyright © 2015 Skymate. All rights reserved.
// </copyright>
// <summary>
//  定义TypeFinder类
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Skymate.TypeFinders
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 辅助类，扫描当前程序域下的程序集
    /// </summary>
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region 字段

        /// <summary>
        ///  引用错误.
        /// </summary>
        private const bool IgnoreReflectionErrors = true;

        /// <summary>
        /// 记录已经加载的程序集，避免再次读取
        /// </summary>
        private readonly List<AttributedAssembly> attributedAssemblies = new List<AttributedAssembly>();

        /// <summary>
        /// 记录已经搜索过的程序集
        /// </summary>
        private readonly List<Type> assemblyAttributesSearched = new List<Type>();

        /// <summary>
        ///  不加在的程序集的正则表达式.
        /// </summary>
        private string assemblySkipLoadingPattern = @"^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Nuget|^Castle|^Iesi|^log4net|^Autofac|^AutoMapper|^EntityFramework|^EPPlus|^Fasterflect|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^Telerik|^Antlr3|^Recaptcha|^FluentValidation|^ImageResizer|^itextsharp|^MiniProfiler|^Newtonsoft|^Pandora|^WebGrease|^Noesis|^DotNetOpenAuth|^Facebook|^LinqToTwitter|^PerceptiveMCAPI|^CookComputing|^GCheckout|^Mono\.Math|^Org\.Mentalis|^App_Web|^BundleTransformer|^ClearScript|^JavaScriptEngineSwitcher|^MsieJavaScriptEngine|^Glimpse|^Ionic|^App_GlobalResources|^AjaxMin|^MaxMind|^NReco|^OffAmazonPayments|^UAParser";

        /// <summary>
        ///  加载程序域的程序集.
        /// </summary>
        private bool loadAppDomainAssemblies = true;

        /// <summary>
        /// 程序集后缀名的正则.
        /// </summary>
        private string assemblyRestrictToLoadingPattern = ".*";

        /// <summary>
        /// 自定义程序集的名称.
        /// </summary>
        private IList<string> customAssemblyNames = new List<string>();

        #endregion 

        #region 构造函数

        #endregion 

        #region 属性

        /// <summary>提供当前所在程序域.</summary>
        public virtual AppDomain App
        {
            get { return AppDomain.CurrentDomain; }
        }

        /// <summary>
        /// 获取或设置加载程序域的程序集
        /// </summary>
        public bool LoadAppDomainAssemblies
        {
            get { return this.loadAppDomainAssemblies; }
            set { this.loadAppDomainAssemblies = value; }
        }

        /// <summary>
        /// 获取或设置自定义程序集名列表
        /// </summary>
        public IList<string> CustomAssemblyNames
        {
            get { return this.customAssemblyNames; }
            set { this.customAssemblyNames = value; }
        }

        /// <summary>
        /// 获取或设置不需要加载的程序集的dll
        /// </summary>
        public string AssemblySkipLoadingPattern
        {
            get { return this.assemblySkipLoadingPattern; }
            set { this.assemblySkipLoadingPattern = value; }
        }

        /// <summary>
        /// 获取或设计是否加载指定程序集
        /// </summary>
        public string AssemblyRestrictToLoadingPattern
        {
            get { return this.assemblyRestrictToLoadingPattern; }
            set { this.assemblyRestrictToLoadingPattern = value; }
        }

        #endregion

        #region ITypeFinder 接口

        /// <summary>
        /// 根据类型查找类.
        /// </summary>
        /// <param name="assignTypeFrom">
        /// 来源.
        /// </param>
        /// <param name="assemblies">
        /// 程序集.
        /// </param>
        /// <param name="onlyConcreteClasses">
        /// 只有构造类.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// 异常
        /// </exception>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();

            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch (Exception)
                    {
                        // EF6 不支持获取类型
                        if (!IgnoreReflectionErrors)
                        {
                            throw;
                        }
                    }

                    if (types == null)
                    {
                        continue;
                    }

                    foreach (var t in types.Where(t => assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && this.DoesTypeImplementOpenGeneric(t, assignTypeFrom))).Where(t => !t.IsInterface))
                    {
                        if (onlyConcreteClasses)
                        {
                            if (t.IsClass && !t.IsAbstract)
                            {
                                result.Add(t);
                            }
                        }
                        else
                        {
                            result.Add(t);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = ex.LoaderExceptions.Aggregate(string.Empty, (current, e) => current + (e.Message + Environment.NewLine));

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }

            return result;
        }

        /// <summary>
        /// 当前的实现相关的组件.
        /// </summary>
        /// <param name="ignoreInactivePlugins">
        /// 指示是否卸载插件的程序集应该过滤掉
        /// </param>
        /// <returns>
        /// 应该加载的程序集列表.
        /// </returns>
        public virtual IList<Assembly> GetAssemblies(bool ignoreInactivePlugins = false)
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (this.LoadAppDomainAssemblies)
            {
                this.AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
            }

            this.AddCustomAssemblies(addedAssemblyNames, assemblies);

            if (ignoreInactivePlugins)
            {
                return null;
            }

            return assemblies;
        }

        #endregion

        /// <summary>
        /// 检测一个dll文件是否需要扫描加载.
        /// </summary>
        /// <param name="assemblyFullName">
        /// 需要检测的程序集名称.
        /// </param>
        /// <returns>
        /// 返回True表示需要记载.
        /// </returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !this.Matches(assemblyFullName, this.AssemblySkipLoadingPattern)
                   && this.Matches(assemblyFullName, this.AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        /// 添加了具体的配置组件.
        /// </summary>
        /// <param name="addedAssemblyNames">
        /// 已添加的程序集名称.
        /// </param>
        /// <param name="assemblies">
        /// 程序集.
        /// </param>
        protected virtual void AddCustomAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (var assemblyName in this.CustomAssemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                if (addedAssemblyNames.Contains(assembly.FullName))
                {
                    continue;
                }

                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        /// <summary>
        /// 检测一个dll文件是否需要扫描加载.
        /// </summary>
        /// <param name="assemblyFullName">
        /// 程序集的全名.
        /// </param>
        /// <param name="pattern">
        /// 过滤的正则表达式.
        /// </param>
        /// <returns>
        ///  返回Ture表示正确名称.
        /// </returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 确保匹配组件提供的文件夹已经加载的应用程序域.
        /// </summary>
        /// <param name="directoryPath">
        /// 物理路径的目录包含dll加载的应用程序域.
        /// </param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = this.GetAssemblies().Select(a => a.FullName).ToList();

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (var dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (this.Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        this.App.Load(an);
                    }
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }
        }

        /// <summary>
        /// 判断类型是否实现另一个类型.
        /// </summary>
        /// <param name="type">
        /// 实现类型.
        /// </param>
        /// <param name="openGeneric">
        /// 接口类型.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                    {
                        continue;
                    }

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 遍历所有程序域中的所有程序集
        /// </summary>
        /// <param name="addedAssemblyNames">
        /// 已经加载程序集的名称
        /// </param>
        /// <param name="assemblies">
        /// 程序集列表
        /// </param>
        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            // 过滤掉指定正则和已经加载的程序集
            foreach (var assembly
                in
                AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => this.Matches(assembly.FullName))
                .Where(assembly => !addedAssemblyNames.Contains(assembly.FullName)))
            {
                assemblies.Add(assembly);
                addedAssemblyNames.Add(assembly.FullName);
            }
        }

        #region 内部装配类

        /// <summary>
        /// The attributed assembly.
        /// </summary>
        private abstract class AttributedAssembly
        {
            /// <summary>
            /// Gets or sets the assembly.
            /// </summary>
            internal Assembly Assembly { get; set; }

            /// <summary>
            /// Gets or sets the plugin attribute type.
            /// </summary>
            internal Type PluginAttributeType { get; set; }
        }

        #endregion
    }
}