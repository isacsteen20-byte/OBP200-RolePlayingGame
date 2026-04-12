namespace OBP200_RolePlayingGame;

public class Enemy : Character
{
    public string Type { get; private set; }
    public int XpReward  { get; }
    public int GoldReward  { get; }
    
   
    public Enemy(string type, string name, int hp, int maxHp, int atk, int def, int xpReward, int goldReward) 
        : base(name, hp, maxHp, atk, def)
    { 
            Type = type;
            XpReward = xpReward; 
            GoldReward = goldReward; 
    }

    public override int GetXpReward()
    {
        return XpReward;
    }

    public override int GetGoldReward()
    {
        return GoldReward;
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