namespace Skymate.DependencyManagement
{
    using Autofac.Builder;

    /// <summary>
    /// The container manager extensions.
    /// </summary>
    public static class ContainerManagerExtensions
    {
        /// <summary>
        /// The with static cache.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <typeparam name="TLimit">
        /// </typeparam>
        /// <typeparam name="TReflectionActivatorData">
        /// </typeparam>
        /// <typeparam name="TStyle">
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IRegistrationBuilder</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithStaticCache<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration) where TReflectionActivatorData : ReflectionActivatorData
        {
            return null;
            /*return registration.WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICacheManager>("static"));*/
        }

        /// <summary>
        /// The with asp net cache.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <typeparam name="TLimit">
        /// </typeparam>
        /// <typeparam name="TReflectionActivatorData">
        /// </typeparam>
        /// <typeparam name="TStyle">
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IRegistrationBuilder</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithAspNetCache<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration) where TReflectionActivatorData : ReflectionActivatorData
        {
            /*return registration.WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICacheManager>("aspnet"));*/
            return null;
        }

        /// <summary>
        /// The with null cache.
        /// </summary>
        /// <param name="registration">
        /// The registration.
        /// </param>
        /// <typeparam name="TLimit">
        /// </typeparam>
        /// <typeparam name="TReflectionActivatorData">
        /// </typeparam>
        /// <typeparam name="TStyle">
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>IRegistrationBuilder</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> WithNullCache<TLimit, TReflectionActivatorData, TStyle>(this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration) where TReflectionActivatorData : ReflectionActivatorData
        {
            /*return registration.WithParameter(Autofac.Core.ResolvedParameter.ForNamed<ICacheManager>("null"));*/
            return null;
        }
    }
}