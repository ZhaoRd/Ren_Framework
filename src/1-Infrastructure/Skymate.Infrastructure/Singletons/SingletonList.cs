namespace Skymate.Singletons
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a singleton list for a certain type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of list to store.
    /// </typeparam>
    public class SingletonList<T> : Singleton<IList<T>>
    {
        /// <summary>
        /// Initializes static members of the <see cref="SingletonList{T}"/> class.
        /// </summary>
        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }

        /// <summary>The singleton instance for the specified type T. Only one instance (at the time) of this list for each type of T.</summary>
        public new static IList<T> Instance
        {
            get { return Singleton<IList<T>>.Instance; }
        }
    }
}