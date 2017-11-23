/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Core {
	public interface IEventObject : IObject {
		bool Bind(string eventName, EventObjectHandler handler);
		bool Fire(BaseEvent evt);
		bool HasEvent(string eventName);
		bool Unbind(string eventName, EventObjectHandler handler);
		
		string Name { get; }
	}
}
