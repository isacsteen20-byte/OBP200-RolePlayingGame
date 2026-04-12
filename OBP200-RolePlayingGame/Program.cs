using System.Text;

namespace OBP200_RolePlayingGame;

class Program
{
    // ======= Globalt tillstånd  =======
   
    // Fiendemall
    static List<Enemy> EnemyTemplates;

    // Ny player från klass
    static Player player;
    
    // Rum: [type, label]
    // types: battle, treasure, shop, rest, boss
    static List<string[]> Rooms = new List<string[]>();
    
    // Status för kartan
    static int CurrentRoomIndex = 0;

    // Random
    static Random Rng = new Random();

    // ======= Main =======

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        // Hämta enemies från ny klass
        EnemyTemplates = Enemy.GetEnemyTemplates();

        while (true)
        {
            ShowMainMenu();
            Console.Write("Välj: ");
            var choice = (Console.ReadLine() ?? "").Trim();

            if (choice == "1")
            {
                StartNewGame();
                RunGameLoop();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Avslutar...");
                return;
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

            Console.WriteLine();
        }
    }

    // ======= Meny & Init =======

    static void ShowMainMenu()
    {
        Console.WriteLine("=== Text-RPG ===");
        Console.WriteLine("1. Nytt spel");
        Console.WriteLine("2. Avsluta");
    }

    static void StartNewGame()
    {
        Console.Write("Ange namn: ");
        var name = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(name)) name = "Namnlös";

        Console.WriteLine("Välj klass: 1) Warrior  2) Mage  3) Rogue");
        Console.Write("Val: ");
        var k = (Console.ReadLine() ?? "").Trim();

        string cls = "Warrior";
        int hp = 0, maxhp = 0, atk = 0, def = 0;
        int potions = 0, gold = 0;
        int xp = 0, level = 1;
        
        switch (k)
        {
            case "1": // Warrior: tankig
                cls = "Warrior";
                maxhp = 40; hp = 40; atk = 7; def = 5; potions = 2; gold = 15;
                break;
            case "2": // Mage: hög damage, låg def
                cls = "Mage";
                maxhp = 28; hp = 28; atk = 10; def = 2; potions = 2; gold = 15;
                break;
            case "3": // Rogue: krit-chans
                cls = "Rogue";
                maxhp = 32; hp = 32; atk = 8; def = 3; potions = 3; gold = 20;
                break;
            default:
                cls = "Warrior";
                maxhp = 40; hp = 40; atk = 7; def = 5; potions = 2; gold = 15;
                break;
        }

        player = new Player(name, cls, hp, maxhp, atk, def, gold, xp, level, potions);

        // Initiera karta (linjärt äventyr)
        Rooms.Clear();
        Rooms.Add(new[] { "battle", "Skogsstig" });
        Rooms.Add(new[] { "treasure", "Gammal kista" });
        Rooms.Add(new[] { "shop", "Vandrande köpman" });
        Rooms.Add(new[] { "battle", "Grottans mynning" });
        Rooms.Add(new[] { "rest", "Lägereld" });
        Rooms.Add(new[] { "battle", "Grottans djup" });
        Rooms.Add(new[] { "boss", "Urdraken" });

        CurrentRoomIndex = 0;

        Console.WriteLine($"Välkommen, {name} the {cls}!");
        ShowStatus();
    }

    static void RunGameLoop()
    {
        while (true)
        {
            var room = Rooms[CurrentRoomIndex];
            Console.WriteLine($"--- Rum {CurrentRoomIndex + 1}/{Rooms.Count}: {room[1]} ({room[0]}) ---");

            bool continueAdventure = EnterRoom(room[0]);
            
            if (player.IsDead())
            {
                Console.WriteLine("Du har stupat... Spelet över.");
                break;
            }
            
            if (!continueAdventure)
            {
                Console.WriteLine("Du lämnar äventyret för nu.");
                break;
            }

            CurrentRoomIndex++;
            
            if (CurrentRoomIndex >= Rooms.Count)
            {
                Console.WriteLine();
                Console.WriteLine("Du har klarat äventyret!");
                break;
            }
            
            Console.WriteLine();
            Console.WriteLine("[C] Fortsätt     [Q] Avsluta till huvudmeny");
            Console.Write("Val: ");
            var post = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (post == "Q")
            {
                Console.WriteLine("Tillbaka till huvudmenyn.");
                break;
            }

            Console.WriteLine();
        }
    }

    // ======= Rumshantering =======

    static bool EnterRoom(string type)
    {
        switch ((type ?? "battle").Trim())
        {
            case "battle":
                return DoBattle(isBoss: false);
            case "boss":
                return DoBattle(isBoss: true);
            case "treasure":
                return DoTreasure();
            case "shop":
                return DoShop();
            case "rest":
                return DoRest();
            default:
                Console.WriteLine("Du vandrar vidare...");
                return true;
        }
    }

    // ======= Strid =======

    static bool DoBattle(bool isBoss)
    {
        //Enemy hanteras som en Character
        Character enemy = GenerateEnemy(isBoss);
        Console.WriteLine($"En {enemy.Name} dyker upp! (HP {enemy.HP}, ATK {enemy.Attack}, DEF {enemy.Defence})");

        while (!enemy.IsDead() && !player.IsDead())
        {
            Console.WriteLine();
            ShowStatus();
            Console.WriteLine($"Fiende: {enemy.Name} HP={enemy.HP}");
            Console.WriteLine("[A] Attack   [X] Special   [P] Dryck   [R] Fly");
            if (isBoss) Console.WriteLine("(Du kan inte fly från en boss!)");
            Console.Write("Val: ");

            var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (cmd == "A")
            {
                int damage = CalculatePlayerDamage(enemy.Defence);
                DealDamage(enemy, damage);
                Console.WriteLine($"Du slog {enemy.Name} för {damage} skada.");
            }
            else if (cmd == "X")
            {
                int special = UseClassSpecial(enemy.Defence, isBoss);
                DealDamage(enemy, special);
                Console.WriteLine($"Special! {enemy.Name} tar {special} skada.");
            }
            else if (cmd == "P")
            {
                UsePotion();
            }
            else if (cmd == "R" && !isBoss)
            {
                if (TryRunAway())
                {
                    Console.WriteLine("Du flydde!");
                    return true; // fortsätt äventyr
                }
                else
                {
                    Console.WriteLine("Misslyckad flykt!");
                }
            }
            else
            {
                Console.WriteLine("Du tvekar...");
            }

            if (enemy.IsDead()) break;

            // Fiendens tur
            int enemyDamage = CalculateEnemyDamage(enemy.Attack);
            DealDamage(player, enemyDamage);
            Console.WriteLine($"{enemy.Name} anfaller och gör {enemyDamage} skada!");
        }

        if (player.IsDead())
        {
            return false; // avsluta äventyr
        }

        // Vinstrapporter, XP, guld, loot
        player.AddXP(enemy.GetXpReward());
        player.AddGold(enemy.GetGoldReward());
        
        MaybeLevelUp();

        Console.WriteLine($"Seger! +{enemy.GetXpReward()} XP, +{enemy.GetGoldReward()} guld.");
        MaybeDropLoot(enemy.Name);

        return true;
    }

    static Enemy GenerateEnemy(bool isBoss)
    {
        if (isBoss)
        {
            // Boss-mall
            return new Enemy("boss", "Urdraken", 55, 55, 9, 4, 30, 50 );
        }
        else
        {
            // Slumpa bland templates
            var template = EnemyTemplates[Rng.Next(EnemyTemplates.Count)];
            
            // Slumpmässig justering av stats
            int hp = template.HP + Rng.Next(-1, 3);
            int atk = template.Attack + Rng.Next(0, 2);
            int def = template.Defence + Rng.Next(0, 2);
            int xp = template.XpReward + Rng.Next(0, 3);
            int gold = template.GoldReward + Rng.Next(0, 3);
            return new Enemy(template.Type, template.Name, hp, hp, atk, def, xp, gold );
        }
    }

    

    static int CalculatePlayerDamage(int enemyDef)
    {
        int atk = player.Attack;
        string cls = player.PlayerClass;

        // Beräkna grundskada
        int baseDmg = Math.Max(1, atk - (enemyDef / 2));
        int roll = Rng.Next(0, 3); // liten variation

        switch (cls.Trim())
        {
            case "Warrior":
                baseDmg += 1; // warrior buff
                break;
            case "Mage":
                baseDmg += 2; // mage buff
                break;
            case "Rogue":
                baseDmg += (Rng.NextDouble() < 0.2) ? 4 : 0; // rogue crit-chans
                break;
            default:
                baseDmg += 0;
                break;
        }

        return Math.Max(1, baseDmg + roll);
    }

    static int UseClassSpecial(int enemyDef, bool vsBoss)
    {
        string cls =player.PlayerClass;
        int specialDmg = 0;

        // Hantering av specialförmågor
        if (cls == "Warrior")
        {
            // Heavy Strike: hög skada men självskada
            Console.WriteLine("Warrior använder Heavy Strike!");
            int atk = player.Attack;
            specialDmg = Math.Max(2, atk + 3 - enemyDef);
           player.TakeDamage(2); // självskada
        }
        else if (cls == "Mage")
        {
            // Fireball: stor skada, kostar guld
            int gold = player.Gold;
            if (gold >= 3)
            {
                Console.WriteLine("Mage kastar Fireball!");
                player.SpendGold(3);
                int atk = player.Attack;
                specialDmg = Math.Max(3, atk + 5 - (enemyDef / 2));
            }
            else
            {
                Console.WriteLine("Inte tillräckligt med guld för att kasta Fireball (kostar 3).");
                specialDmg = 0;
            }
        }
        else if (cls == "Rogue")
        {
            // Backstab: chans att ignorera försvar, hög risk/hög belöning
            if (Rng.NextDouble() < 0.5)
            {
                Console.WriteLine("Rogue utför en lyckad Backstab!");
                int atk = player.Attack;
                specialDmg = Math.Max(4, atk + 6);
            }
            else
            {
                Console.WriteLine("Backstab misslyckades!");
                specialDmg = 1;
            }
        }
        else
        {
            specialDmg = 0;
        }

        // Dämpa skada mot bossen
        if (vsBoss)
        {
            specialDmg = (int)Math.Round(specialDmg * 0.8);
        }

        return Math.Max(0, specialDmg);
    }

    static int CalculateEnemyDamage(int enemyAtk)
    {
        int def = player.Defence;
        int roll = Rng.Next(0, 3);

        int dmg = Math.Max(1, enemyAtk - (def / 2)) + roll;

        // Liten chans till "glancing blow" (minskad skada)
        if (Rng.NextDouble() < 0.1) dmg = Math.Max(1, dmg - 2);

        return dmg;
    }
    
    //Metod för Dealdamage som använder Metoden takedamage, ifrån CharacterKlass
    static void DealDamage(Character target, int damage)
    {
        target.TakeDamage(damage);
    }

   
    static void UsePotion()
    {
        int before = player.HP;

        if (!player.UsePotion())
        {
            Console.WriteLine("Du har inga drycker kvar.");
            return;
        }

        // Helning av spelaren
        int healed = player.HP - before;
        Console.WriteLine($"Du dricker en dryck och återfår {healed} HP.");
    }

    static bool TryRunAway()
    {
        // Flyktschans baserad på karaktärsklass
        string cls = player.PlayerClass;
        double chance = 0.25;
        if (cls == "Rogue") chance = 0.5;
        if (cls == "Mage") chance = 0.35;
        return Rng.NextDouble() < chance;
    }

    

 


    static void MaybeLevelUp()
    {
        // Nivåtrösklar
        int xp = player.XP;
        int lvl = player.Level;
        int nextThreshold = lvl == 1 ? 10 : (lvl == 2 ? 25 : (lvl == 3 ? 45 : lvl * 20));

        if (xp >= nextThreshold)
        {
            player.LevelUp(1);

            // Uppgradering baserad på karaktärsklass
            string cls =player.PlayerClass;

            switch (cls)
            {
                case "Warrior":
                    player.IncreaseMaxHP(6); 
                    player.IncreaseAttack(2); 
                    player.IncreaseDefence(2);
                    break;
                case "Mage":
                    player.IncreaseMaxHP(4); 
                    player.IncreaseAttack(4); 
                    player.IncreaseDefence(1);
                    break;
                case "Rogue":
                    player.IncreaseMaxHP(5); 
                    player.IncreaseAttack(3); 
                    player.IncreaseDefence(1);
                    break;
                default:
                    player.IncreaseMaxHP(4); 
                    player.IncreaseAttack(3); 
                    player.IncreaseDefence(1);
                    break;
            }
            
        
            player.HealToMax(); // full heal vid level up

            Console.WriteLine($"Du når nivå {lvl + 1}! Värden ökade och HP återställd.");
        }
    }

    static void MaybeDropLoot(string enemyName)
    {
        // Enkel loot-regel
        if (Rng.NextDouble() < 0.35)
        {
            string item = "Minor Gem";
            if (enemyName.Contains("Urdraken")) item = "Dragon Scale";

            player.Inventory.Add(item);

            Console.WriteLine($"Föremål hittat: {item} (lagt i din väska)");
        }
    }

    // ======= Rumshändelser =======

    static bool DoTreasure()
    {
        Console.WriteLine("Du hittar en gammal kista...");
        
        if (Rng.NextDouble() < 0.5)
        {
            int gold = Rng.Next(8, 15);
            player.AddGold(gold);
            Console.WriteLine($"Kistan innehåller {gold} guld!");
        }
        else
        {
            var items = new[] { "Iron Dagger", "Oak Staff", "Leather Vest", "Healing Herb" };
            string found = items[Rng.Next(items.Length)];
            player.Inventory.Add(found);
            Console.WriteLine($"Du plockar upp: {found}");
        }
        return true;
    }

    static bool DoShop()
    {
        Console.WriteLine("En vandrande köpman erbjuder sina varor:");
        while (true)
        {
            Console.WriteLine($"Guld: {player.Gold} | Drycker: {player.Potions}");
            Console.WriteLine("1) Köp dryck (10 guld)");
            Console.WriteLine("2) Köp vapen (+2 ATK) (25 guld)");
            Console.WriteLine("3) Köp rustning (+2 DEF) (25 guld)");
            Console.WriteLine("4) Sälj alla 'Minor Gem' (+5 guld/st)");
            Console.WriteLine("5) Lämna butiken");
            Console.Write("Val: ");
            var val = (Console.ReadLine() ?? "").Trim();

            if (val == "1")
            {
                TryBuy(10, () => player.AddPotion(1), "Du köper en dryck.");            }
            else if (val == "2")
            {
                TryBuy(25, () => player.IncreaseAttack(2), "Du köper ett bättre vapen.");
            }
            else if (val == "3")
            {
                TryBuy(25, () => player.IncreaseDefence(2), "Du köper bättre rustning.");
            }
            else if (val == "4")
            {
                SellMinorGems();
            }
            else if (val == "5")
            {
                Console.WriteLine("Du säger adjö till köpmannen.");
                break;
            }
            else
            {
                Console.WriteLine("Köpmannen förstår inte ditt val.");
            }
        }
        return true;
    }

    static void TryBuy(int cost, Action apply, string successMsg)
    {
        int gold = player.Gold;
        if (gold >= cost)
        {
            player.SpendGold(gold - cost);
            apply();
            Console.WriteLine(successMsg);
        }
        else
        {
            Console.WriteLine("Du har inte råd.");
        }
    }

    static void SellMinorGems()
    {
       
        if (player.Inventory.Count == 0)
        {
            Console.WriteLine("Du har inga föremål att sälja.");
            return;
        }
        
        int count = player.Inventory.Count(x => x == "Minor Gem");
        if (count == 0)
        {
            Console.WriteLine("Inga 'Minor Gem' i väskan.");
            return;
        }

        player.Inventory = player.Inventory.Where(x => x != "Minor Gem").ToList();

        player.AddGold(count * 5);
        Console.WriteLine($"Du säljer {count} st Minor Gem för {count * 5} guld.");
    }

    static bool DoRest()
    {
        Console.WriteLine("Du slår läger och vilar.");
        player.HealToMax();
        Console.WriteLine("HP återställt till max.");
        return true;
    }

    // ======= Status =======

    static void ShowStatus()
    {
        Console.WriteLine($"[{player.Name} | {player.PlayerClass}]  HP {player.HP}/{player.MaxHP}  ATK {player.Attack}  DEF {player.Defence}  LVL {player.Level}  XP {player.XP}  Guld {player.Gold}  Drycker {player.Potions}");
       
        if (player.Inventory.Count > 0)
        {
            Console.WriteLine($"Väska: {string.Join("; ", player.Inventory)}");
        }
    }
 }
