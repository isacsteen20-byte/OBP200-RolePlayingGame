namespace OBP200_RolePlayingGame;

public class Player : Character
{
    public string PlayerClass { get; set; }
    public int Gold { get; set; }
    public int XP { get; set; }
    public int Level { get; set; }
    public int Potions { get; set; }
    public List<string> Inventory { get; set; }
    
   
    public Player (string name, string playerClass, int hp, int maxHp, int atk, int def, int gold, int xp, int level, int potions) 
        : base(name, hp, maxHp, atk, def)
    {
        PlayerClass = playerClass;
        Level = level;
        XP = xp;
        Gold = gold;
        Potions = potions;
        
        Inventory = new List<string>{"Wooden Sword", "Cloth Armor"};
    }
}