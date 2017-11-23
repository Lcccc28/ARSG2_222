using UnityEngine;

public class StageData {
    
    // 规则3配置
    public int[] monsId; // 怪物ID
    public Vector3[] monsPos; // 怪物坐标
    public int[] monsNum; // 怪物数量
    public int[] cond; // 进入下一轮条件条件 0：多少时间后 1：所有怪物清除后 2：立马创建BOSS
    public int[] condParm; // 条件参数
    public bool[] condState; // 当前轮的条件

    // 规则2配置
    //public int[] monsId; // 怪物ID
    //public float[] interval; // 创建间隔
    //public float[] curInterval; // 当前间隔
    //public int[] num; // 创建个数
    //public bool isLoop; // 是否循环
}
