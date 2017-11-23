/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Core {

	public class BaseEvent {
	    public object sender;
	    protected string eventName;
	    protected object parameters;

	    public BaseEvent(string eventName)
	        : this(eventName, null) {
	    }

	    public BaseEvent(string eventName, object parameters) {
	        this.eventName = eventName;
	        this.parameters = parameters;
	    }

	    public string EventName {
	        get {
	            return this.eventName;
	        }
	    }

	    public object Parameters {
	        get {
	            return this.parameters;
	        }
	    }
	}
}
