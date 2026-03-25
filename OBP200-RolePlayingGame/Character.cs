namespace OBP200_RolePlayingGame;

public abstract class Character
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Attack { get; set; }
    public int Defence { get; set; }

    public bool IsDead()
    {
        return HP <= 0;
    }

    public void TakeDamage(int dmg)
    {
        HP -= Math.Max(0, dmg);
        if (HP < 0) HP = 0;
    }
    public Character(string name, int hp, int maxHp, int attack, int defence)
    {
        Name = name;
        HP = hp;
        MaxHP = maxHp;
        Attack = attack;
        Defence = defence;
    }
}