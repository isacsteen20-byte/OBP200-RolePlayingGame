namespace OBP200_RolePlayingGame;

public abstract class Character
{
    public string Name { get; set; }
    public int HP { get; protected set; }
    public int MaxHP { get; protected set; }
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
    public void Heal(int amount)
    {
        HP = Math.Min(MaxHP, HP + amount);
    }
    public void IncreaseMaxHP(int amount)
    {
       if (amount < 0 ) return;
       MaxHP += amount;
       
       if (HP > MaxHP)
           HP = MaxHP;
    }

    public void HealToMax()
    {
        HP = MaxHP;
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