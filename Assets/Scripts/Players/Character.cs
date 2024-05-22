using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public CharacterTypesByUniqueName CharacterTypes { get; private set; }
    public CharacterQualities CharacterQuality { get; private set; }
    public int Level { get; private set; }
    public float CurrentSpeed { get; private set; }

    public float CurrentHP { get; private set; }
    public float BaseHP { get; private set; }
    public float DeltaHP { get; private set; }
    
    public float CurrentDamage { get; private set; }
    public float BaseDamage { get; private set; }
    public float DeltaDamage { get; private set; }
    public float AttackSpeed { get; private set; }

    public float DamageRadius { get; private set; }
    public float HitRadius { get; private set; }
    public AttackTypes AttackType { get; private set; }

    public bool IsAlive { get => CurrentHP > 0; }
    public CharacterSized Size { get; private set; }

    public Character() { }
        

    public Character(CharacterTypesByUniqueName characterType, int level) 
    {
        Character c = new Character();

        switch (characterType)
        {
            case CharacterTypesByUniqueName.WarriorSam: 
                c = WarriorSam(level);
                break;
            case CharacterTypesByUniqueName.ShooterMike:
                c = ShooterMike(level);
                break;

            case CharacterTypesByUniqueName.TestBoss:
                c = TestBoss(level);
                break;
        }
              
        CharacterTypes = c.CharacterTypes;
        Name = c.Name;
        Level = level;
        
        CurrentHP = c.CurrentHP;
        BaseHP = c.BaseHP;
        DeltaHP = c.DeltaHP;

        CurrentDamage = c.CurrentDamage;
        BaseDamage = c.BaseDamage;
        DeltaDamage = c.DeltaDamage;
        DamageRadius = c.DamageRadius;
        HitRadius = c.HitRadius;
        AttackType = c.AttackType;
        AttackSpeed = c.AttackSpeed;
        Size = c.Size;
        CurrentSpeed = c.CurrentSpeed;

        CharacterQuality = c.CharacterQuality;

    }

    private Character WarriorSam(int level)
    {
        Character c = new Character();
        c.CharacterTypes = CharacterTypesByUniqueName.WarriorSam;

        c.BaseHP = 100;
        c.DeltaHP = 5;
        c.BaseDamage = 10;
        c.DeltaDamage = 2;

        c.Name = "WarriorSam";
        c.CurrentHP = c.BaseHP + c.DeltaHP * level;
        c.CurrentDamage = c.BaseDamage + c.DeltaDamage * level;
        c.DamageRadius = 0.25f;
        c.HitRadius = 0.75f;
        c.CharacterQuality = CharacterQualities.common;
        c.AttackType = AttackTypes.melee_hit;
        c.AttackSpeed = 0.7f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        return c;
    }

    private Character ShooterMike(int level)
    {
        Character c = new Character();
        c.CharacterTypes = CharacterTypesByUniqueName.ShooterMike;

        c.BaseHP = 80;
        c.DeltaHP = 3;
        c.BaseDamage = 15;
        c.DeltaDamage = 3;

        c.Name = "ShooterMike";
        c.CurrentHP = c.BaseHP + c.DeltaHP * level;
        c.CurrentDamage = c.BaseDamage + c.DeltaDamage * level;
        c.DamageRadius = 0.25f;
        c.HitRadius = 4f;
        c.CharacterQuality = CharacterQualities.common;
        c.AttackType = AttackTypes.ranged_hit;
        c.AttackSpeed = 0.8f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        return c;
    }

    private Character TestBoss(int level)
    {
        Character c = new Character();
        c.CharacterTypes = CharacterTypesByUniqueName.TestBoss;

        c.BaseHP = 700;
        c.DeltaHP = 10;
        c.BaseDamage = 10;
        c.DeltaDamage = 3;

        c.Name = "TestBoss";
        c.CurrentHP = c.BaseHP + c.DeltaHP * level;
        c.CurrentDamage = c.BaseDamage + c.DeltaDamage * level;
        c.DamageRadius = 0.25f;
        c.CharacterQuality = CharacterQualities.improved;
        c.AttackType = AttackTypes.melee_hit;
        c.AttackSpeed = 0.9f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        return c;
    }


    //=========================Statics============================

    public static string GetCharacterQualityString(CharacterQualities _type)
    {
        switch(_type)
        {
            case CharacterQualities.common:     return Globals.Language.Common;
            case CharacterQualities.improved:   return Globals.Language.Improved;
            case CharacterQualities.unique:     return Globals.Language.Unique;
            case CharacterQualities.rare:       return Globals.Language.Rare;
            case CharacterQualities.legendary:  return Globals.Language.Legendary;
        }

        return "";
    }

    public static GameObject GetCharacterObject(CharacterTypesByUniqueName _type)
    {
        switch (_type)
        {
            case CharacterTypesByUniqueName.WarriorSam: return Resources.Load<GameObject>("Characters/WarriorSam");
            case CharacterTypesByUniqueName.ShooterMike: return Resources.Load<GameObject>("Characters/ShooterMike");
            case CharacterTypesByUniqueName.TestBoss: return Resources.Load<GameObject>("Characters/TestBoss");
        }

        return null;
    }
}

public enum CharacterTypesByUniqueName
{
    WarriorSam,
    ShooterMike,
    TestBoss
}

public enum CharacterTypesByCathegory
{
    Squad,
    NPS_small,
    NPS_medium,
    NPS_large,
    NPS_boss
}

public enum CharacterQualities
{
    common,
    improved,
    unique,
    rare,
    legendary
}

public enum AttackTypes
{
    melee_hit,
    ranged_hit
}

public enum CharacterSized
{
    small,
    medium,
    big
}
