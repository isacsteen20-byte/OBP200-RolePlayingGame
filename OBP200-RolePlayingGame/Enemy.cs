namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    public string Type { get; set; }
    public override int XpReward  { get; set; }
    public override int GoldReward  { get; set; }
    
   
    public Enemy(string type, string name, int hp, int maxHp, int atk, int def, int xpReward, int goldReward) 
        : base(name, hp, maxHp, atk, def)
    { 
            Type = type;
            XpReward = xpReward; 
            GoldReward = goldReward; 
    } 
    public static List<Enemy> GetEnemyTemplates()
    {
        return new List<Enemy>
        {
            new Enemy("beast", "Vildsvin", 18, 18, 4, 1, 6, 4),
            new Enemy ("undead", "Skelett", 20, 20, 5, 2, 7, 5),
            new Enemy ("bandit", "Bandit", 16, 16, 6, 1, 8, 6 ),
            new Enemy ("slime", "Geléslem", 14, 14, 3, 0, 5, 3 )
        };

        
    }
}