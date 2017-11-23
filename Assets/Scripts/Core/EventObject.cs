using System.Collections;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Core {
	public class EventObject : IEventObject{
	    private Hashtable events = null;
	    private string name;

	    public EventObject() : this("EventObject") {
	    }

	    public EventObject(string name) {
	        this.name = name;
	    }

		public virtual bool Bind(string eventName, EventObjectHandler handler) {
            if (events == null) this.events = new Hashtable();

	        if (!events.ContainsKey(eventName)) {
	            events.Add(eventName, ArrayList.Synchronized(new ArrayList()));
	        }

	        ArrayList list = events[eventName] as ArrayList;
	        if (!list.Contains(handler)) {
	            list.Add(handler);
	            return true;
	        }
	        return false;
	    }

		public virtual bool Unbind(string eventName, EventObjectHandler handler) {
            if (events == null) return true;

	        if (events.ContainsKey(eventName)) {
	            ArrayList list = events[eventName] as ArrayList;
	            int index = list.IndexOf(handler);
	            if (index > -1) {
	                list.RemoveAt(index);
	                return true;
	            }
	        }
	        return false;
	    }

		public virtual bool Fire(BaseEvent evt) {
            if (events == null) return false;

	        EventObjectHandler handler;
	        string eventName = evt.EventName;
	        if (!events.ContainsKey(eventName)) {
	            return false;
	        }
	        ArrayList list = ((events[eventName]) as ArrayList).Clone() as ArrayList;
            
            evt.sender = this;
	        for (int i = 0; i < list.Count; ++i) {
	            handler = list[i] as EventObjectHandler;
                handler(evt);
	        }
	        handler = null;
	        return true;
	    }

		public virtual bool HasEvent(string eventName) {
            if (events == null) return false;
	        return events.ContainsKey(eventName);
	    } 

		public virtual string Name {
			get {
				return name;
			}
		}

		public virtual void Destroy() {
            ClearEvents();
			events = null;
			name = null;
		}

        protected void ClearEvents () {
            if (events != null)
			    events.Clear();
        }
	}
}