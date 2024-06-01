using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public CharacterTypesByUniqueName CharacterTypeByUniqueName { get; private set; }
    public CharacterTypesByCathegory CharacterTypeByCathegory { get; private set; }
    public CharacterQualities CharacterQuality { get; private set; }
    public int Level { get; private set; }
    public float CurrentSpeed { get; private set; }

    public Action OnCharacterDead;
    public float MaxHP { get => (BaseHP + DeltaHP * Level) * HPKoeff; }
    public float CurrentHP { get; private set; }
    public float BaseHP { get; private set; }
    public float DeltaHP { get; private set; }
    public float HPKoeff { get; private set; } = 1;

    public float MaxDamage { get => (BaseDamage + DeltaDamage * Level) * DamageKoeff; }
    public float CurrentDamage { get; private set; }
    public float BaseDamage { get; private set; }
    public float DeltaDamage { get; private set; }
    public float AttackSpeed { get; private set; }
    public float DPS { get => CurrentDamage / AttackSpeed; }
    public float DamageKoeff { get; private set; } = 1;

    public float DamageRadius { get; private set; }
    public float HitRadius { get; private set; }
    public float AggroRadius { get; private set; }
    public AttackTypes AttackType { get; private set; }
    public CharacterGameplayRoles CharacterGameplayRole { get; private set; }

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
            case CharacterTypesByUniqueName.VikingHero:
                c = VikingHero(level);
                break;

            case CharacterTypesByUniqueName.PriestSimpleHuman:
                c = PriestSimpleHuman(level);
                break;
        }
              
        CharacterTypeByUniqueName = c.CharacterTypeByUniqueName;
        Name = c.Name;
        Level = level;
        
        CharacterTypeByCathegory = c.CharacterTypeByCathegory;
       
        BaseHP = c.BaseHP;
        DeltaHP = c.DeltaHP;

        BaseDamage = c.BaseDamage;
        DeltaDamage = c.DeltaDamage;
        DamageRadius = c.DamageRadius;
        HitRadius = c.HitRadius;
        AggroRadius = c.AggroRadius;
        AttackType = c.AttackType;
        AttackSpeed = c.AttackSpeed;
        Size = c.Size;
        CurrentSpeed = c.CurrentSpeed;

        CharacterQuality = c.CharacterQuality;
        CurrentHP = MaxHP;
        CurrentDamage = MaxDamage;
        CharacterGameplayRole = c.CharacterGameplayRole;
    }

    private Character WarriorSam(int level)
    {
        Character c = new Character();
        c.CharacterTypeByUniqueName = CharacterTypesByUniqueName.WarriorSam;

        c.BaseHP = 100;
        c.DeltaHP = 5;
        c.BaseDamage = 10;
        c.DeltaDamage = 2;

        c.Name = "Воин";
                        
        c.DamageRadius = 0.25f;
        c.HitRadius = 1.1f;
        c.AggroRadius = 5f;
        c.CharacterQuality = CharacterQualities.common;
        c.AttackType = AttackTypes.melee_hit;
        c.CharacterGameplayRole = CharacterGameplayRoles.meleeDamager;
        c.AttackSpeed = 0.7f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        c.CharacterTypeByCathegory = CharacterTypesByCathegory.Squad;
        return c;
    }

    private Character ShooterMike(int level)
    {
        Character c = new Character();
        c.CharacterTypeByUniqueName = CharacterTypesByUniqueName.ShooterMike;

        c.BaseHP = 80;
        c.DeltaHP = 3;
        c.BaseDamage = 15;
        c.DeltaDamage = 3;

        c.Name = "Лучник";
        
        c.DamageRadius = 0.25f;
        c.HitRadius = 4f;
        c.AggroRadius = 5f;
        c.CharacterQuality = CharacterQualities.common;
        c.AttackType = AttackTypes.ranged_hit;
        c.CharacterGameplayRole = CharacterGameplayRoles.rangedDamager;
        c.AttackSpeed = 0.8f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        c.CharacterTypeByCathegory = CharacterTypesByCathegory.Squad;
        return c;
    }

    private Character PriestSimpleHuman(int level)
    {
        Character c = new Character();
        c.CharacterTypeByUniqueName = CharacterTypesByUniqueName.PriestSimpleHuman;

        c.BaseHP = 70;
        c.DeltaHP = 5;
        c.BaseDamage = 25;
        c.DeltaDamage = 5;

        c.Name = "Жрец";

        c.DamageRadius = 0.25f;
        c.HitRadius = 6f;
        c.AggroRadius = 5f;
        c.CharacterQuality = CharacterQualities.common;
        c.AttackType = AttackTypes.heal;
        c.CharacterGameplayRole = CharacterGameplayRoles.healer;
        c.AttackSpeed = 1.5f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        c.CharacterTypeByCathegory = CharacterTypesByCathegory.Squad;
        return c;
    }

    private Character TestBoss(int level)
    {
        Character c = new Character();
        c.CharacterTypeByUniqueName = CharacterTypesByUniqueName.TestBoss;

        c.BaseHP = 300;
        c.DeltaHP = 10;
        c.BaseDamage = 30;
        c.DeltaDamage = 3;

        c.Name = "TestBoss";
        
        c.DamageRadius = 0.25f;
        c.HitRadius = 1.1f;
        c.AggroRadius = 5f;
        c.CharacterQuality = CharacterQualities.improved;
        c.AttackType = AttackTypes.melee_hit;
        c.CharacterGameplayRole = CharacterGameplayRoles.meleeDamager;
        c.AttackSpeed = 0.9f;
        c.Size = CharacterSized.small;
        c.CurrentSpeed = 3;
        c.CharacterTypeByCathegory = CharacterTypesByCathegory.NPS_medium;
        return c;
    }

    private Character VikingHero(int level)
    {
        Character c = new Character();
        c.CharacterTypeByUniqueName = CharacterTypesByUniqueName.VikingHero;

        c.BaseHP = 300;
        c.DeltaHP = 20;
        c.BaseDamage = 30;
        c.DeltaDamage = 10;

        c.Name = "Викинг";

        c.DamageRadius = 0.35f;
        c.HitRadius = 1.25f;
        c.AggroRadius = 5f;
        c.CharacterQuality = CharacterQualities.legendary;
        c.AttackType = AttackTypes.melee_hit;
        c.CharacterGameplayRole = CharacterGameplayRoles.meleeDamager;
        c.AttackSpeed = 0.9f;
        c.Size = CharacterSized.medium;
        c.CurrentSpeed = 3;
        c.CharacterTypeByCathegory = CharacterTypesByCathegory.SquadHero;
        return c;
    }


    //=========================Public=============================



    public void ReceiveDamage(float damage)
    {
        if (!IsAlive) return;
        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            OnCharacterDead.Invoke();
            CurrentHP = 0;
        }
    }

    public void ReceiveHeal(float heal)
    {
        if (!IsAlive) return;
        CurrentHP += heal;

        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
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

}

public enum CharacterTypesByUniqueName
{
    WarriorSam,
    ShooterMike,
    TestBoss,
    WarriorShieldSwordSimpleHuman,
    WarriorShieldSwordImprovedHuman,
    VikingHero,
    PriestSimpleHuman
}

public enum CharacterTypesByCathegory
{
    Squad,
    SquadHero,
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
    ranged_hit,
    magic_hit,
    heal
}

public enum CharacterSized
{
    small,
    medium,
    big
}

public enum CharacterGameplayRoles
{
    meleeDamager,
    rangedDamager,
    tank,
    healer,
    magicDamager
}

