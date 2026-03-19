namespace OBP200_RolePlayingGame;

public class Enemy
{
    public string Type { get; set; }
    public string Name { get; set; }
    
    public int HP  { get; set; }
    
    public int MaxHP  { get; set; }
    
    public int Attack   { get; set; }
    
    public int Defence { get; set; }
    
    public int XPReward  { get; set; }
    
    public int GoldReward  { get; set; }
    
    public bool IsBoss  { get; set; }
    
   
    public Enemy(string type, string name, int hp, int maxHp, int atk, int def, int xpReward, int goldReward,  bool isBoss)
    { 
            Type = type;
            Name = name; 
            HP  = hp; 
            MaxHP = maxHp;
            Attack = atk; 
            Defence = def; 
            XPReward = xpReward; 
            GoldReward = goldReward; 
            IsBoss = isBoss;
    } 
}