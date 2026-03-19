namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    public string Type { get; set; }
    public int XpReward  { get; set; }
    public int GoldReward  { get; set; }
    public bool IsBoss  { get; set; }
    
   
    public Enemy(string type, string name, int hp, int maxHp, int atk, int def, int xpReward, int goldReward,  bool isBoss) 
        : base(name, hp, maxHp, atk, def)
    { 
            Type = type;
            XpReward = xpReward; 
            GoldReward = goldReward; 
            IsBoss = isBoss;
    } 
}