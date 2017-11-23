using System;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Core {
    public class Singleton<T> : EventObject where T : new() {
        private static T m_Instance;

        private static readonly object m_Lock;

        static Singleton() {
            Singleton<T>.m_Lock = new object();
        }

        public static T Instance {
            get {
                if (Singleton<T>.m_Instance == null) {
                    lock (Singleton<T>.m_Lock) {
                        Singleton<T>.m_Instance = (default(T) == null) 
                            ? Activator.CreateInstance<T>() : default(T);
                    }
                }
                return Singleton<T>.m_Instance;
            }
        }

        public void ClearInstance() {
            Singleton<T>.m_Instance = default(T);
        }
    }
}
