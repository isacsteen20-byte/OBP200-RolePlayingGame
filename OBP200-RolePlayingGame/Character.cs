namespace OBP200_RolePlayingGame;

public abstract class Character
{
    public string Name { get; private set; }
    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public int Attack { get; private set; }
    public int Defence { get; private set; }
    
    
    public Character(string name, int hp, int maxHp, int attack, int defence)
    {
        Name = name;
        HP = hp;
        MaxHP = maxHp;
        Attack = attack;
        Defence = defence;
    }
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
    
    public virtual int GetXpReward()
    {
        return 0;
    }
    public virtual int GetGoldReward() 
    {
        return 0;
    }
    public void IncreaseAttack(int amount)
    {
        if  (amount < 0) return;
        Attack += amount;
    }

    public void IncreaseDefence(int amount)
    {
        if (amount < 0) return;
        Defence += amount;
    }
}