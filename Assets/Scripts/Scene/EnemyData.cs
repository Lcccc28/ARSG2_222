public class EnemyData {

    public string modelName; // 怪物模型名称
    public int type; // 怪物类型 0：小怪 1：BOSS
    public int curHP;
    public int maxHP;
	public BulletData[] bulletDatas;	// 子弹数据
}

public enum EnemyStatus
{
	Stand = 0,
	Move = 1,
	Hit = 2,
	Die = 3,
	Attack = 100,
};