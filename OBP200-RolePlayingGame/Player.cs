namespace OBP200_RolePlayingGame;

public class Player
{
    public string Name { get; set; }
    
    public string PlayerClass { get; set; }
    
    public int HP { get; set; }

    public int MaxHP { get; set; }
    
    public int Attack  { get; set; }
    
    public int Defence { get; set; }
    
    public int Gold { get; set; }
    
    public int XP { get; set; }
    
    public int Level { get; set; }
    
    public int Potions { get; set; }
    
    public List<string> Inventory { get; set; }

    public Player()
    {
        Name = "Namnlös";
        PlayerClass = "Warrior";
        MaxHP = HP = 10;
        Attack  = 1;
        Defence = 1;
        Gold = 0;
        XP = 0;
        Level = 1;
        Potions = 0;
        Inventory = new List<string>();

    }
    
   
    public Player (string name, string playerClass, int hp, int maxHp, int atk, int def, int gold, int xp, int level, int potions,  List<string> inventory)
    {
        Name = name;
        PlayerClass = playerClass;
        HP = hp;
        MaxHP = maxHp;
        Attack  = atk;
        Defence = def;
        Gold = gold;
        XP = xp;
        Level = level;
        Potions = potions;
        Inventory = inventory;
    }
    


}