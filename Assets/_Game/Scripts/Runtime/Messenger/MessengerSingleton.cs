using System.Runtime.CompilerServices;

namespace Runtime.Message
{
    public static class MessengerSingleton
    {
        #region Class Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Of<T>() where T : class, new()
            => Single<T>.Instance;

        #endregion Class Methods

        #region Owner Classes

        private static class Single<T> where T : class, new()
        {
            #region Membres

            public static readonly T Instance = new T();

            #endregion Members
        }

        #endregion Owner Classes
    }
}