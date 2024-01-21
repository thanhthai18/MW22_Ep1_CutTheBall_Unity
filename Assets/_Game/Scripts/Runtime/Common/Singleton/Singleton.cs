﻿using System.Runtime.CompilerServices;

namespace Runtime.Common.Singleton
{
    // Note: This singleton is used for classes that are not mono-targeted ones.
    // For mono-targeted ones, use MonoSingleton instead.
    // The differerence between the two is this singleton remains constantly through scenes, 
    // therefore its data stays in memory all the time unless getting disposed properly.
    // In contrast with it, the MonoSingleton only exists in a scene, and once a new scene loaded,
    // everything belongs to that MonoSingleton will be got rid.
    // ==> Final thought: This project initially intented not to use this kind of Singleton design 'cause 
    // it's not a good design generally. That said, things have been settled, use this Singleton design properly!

    public static class Singleton
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