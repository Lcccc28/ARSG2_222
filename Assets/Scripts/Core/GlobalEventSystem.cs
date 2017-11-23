using UnityEngine;
using System.Collections;

namespace Game.Core {

    public static class GlobalEventSystem {
        private static Hashtable events = new Hashtable();

        // 函数中用到的临时变量
        private static ArrayList bindList;
        private static ArrayList unbindList;
        private static EventObjectHandler handler;

        public static bool Bind(string eventName, EventObjectHandler handler) {
            if (!events.ContainsKey(eventName)) {
                events.Add(eventName, ArrayList.Synchronized(new ArrayList()));
            }

            bindList = events[eventName] as ArrayList;
            if (!bindList.Contains(handler)) {
                bindList.Add(handler);
                bindList = null;
                return true;
            }
            bindList = null;
            return false;
        }

        public static bool UnBind(string eventName, EventObjectHandler handler) {
            if (events.ContainsKey(eventName)) {
                unbindList = events[eventName] as ArrayList;
                int index = unbindList.IndexOf(handler);
                if (index > -1) {
                    unbindList.RemoveAt(index);
                    if (unbindList.Count == 0) {
                        events.Remove(eventName);
                    }
                    unbindList = null;
                    return true;
                }
            }
            unbindList = null;
            return false;
        }

        public static bool Fire(BaseEvent evt) {
            string eventName = evt.EventName;

            if (!events.ContainsKey(eventName)) {
                return false;
            }

            ArrayList fireList = ((events[eventName]) as ArrayList).Clone() as ArrayList;
            for (int i = 0; i < fireList.Count; ++i) {
                handler = fireList[i] as EventObjectHandler;
                handler(evt);
            }
            handler = null;
            fireList = null;
            return true;
        }

        public static void Clear() {
            events.Clear();
        }
    }

    public delegate void EventObjectHandler(BaseEvent evt);
}