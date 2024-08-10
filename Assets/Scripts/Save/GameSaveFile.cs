using System;

[Serializable]
public class GameSaveFile
{
    // 左右手武器
    public WeaponType leftWeaponType;
    public WeaponType rightWeaponType;
    
    // 当前关卡
    public string currentLevel;
    // 当前的关卡小节
    public string currentSection;
    
    // 训练场解锁进度
    public int currentProgress;
}