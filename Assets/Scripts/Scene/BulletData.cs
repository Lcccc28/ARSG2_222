public class BulletData {

    public int atk = 10; // 攻击力
    public int num = 0; // 一次射击的子弹数量
	public float speed = 10f; // 子弹速度
	public float lifeTime = 5f; // 子弹生存时间
    public int curNum = 0; // 当前子弹数
    public int maxNum = 30; // 最大子弹数量
    public float interval = 0; // 多颗子弹之间的发射间隔
    public string modelName = ""; // 子弹模型名称
    public bool isRandomRotation = false; // 是否随机角度

	public string hitModelName = "Hit"; // 子弹击中模型名称
	public float hitDestroyTime = 0.3f; // 击中销毁时间
    public string sound = "";
	public float cdData = 0;
	public float cd = 0;

	public string script = "Bullet";
}
