namespace OBP200_RolePlayingGame;

public class Player : Character
{
    public string PlayerClass { get; }
    public int Gold { get; private set; }
    public int XP { get; private set; }
    public int Level { get; private set; }
    public int Potions { get; private set; }
    public List<string> Inventory { get; }

    
   
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
    public void AddGold(int amount)
    {
        Gold += Math.Max(0, amount);
    }

    public void SpendGold(int amount)
    {
        if (amount <= Gold)
            Gold -= amount;
    }

    public void AddXP(int amount)
    {
        XP += Math.Max(0, amount);
    }

    public void LevelUp()
    {
        Level++;
    }

    public bool UsePotion()
    {
        if (Potions <= 0)
            return false;
            
        Potions--;
        Heal(12);
        return true;
    }

    public void AddPotion(int amount)
    {
        Potions += amount;
    }

}