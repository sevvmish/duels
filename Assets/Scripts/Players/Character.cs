using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string Name { get; private set; }
    public CharacterTypesByUniqueName CharacterTypes { get; private set; }
    public int Level { get; private set; }
    public float HP { get; private set; }
    public float Damage { get; private set; }

    public Character() { }

    public Character(string name, int level, float hp, float damage, CharacterTypesByUniqueName characterTypes)
    {
        Name = name;
        Level = level;
        HP = hp;
        Damage = damage;
        CharacterTypes = characterTypes;
    }

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
        }
              
        CharacterTypes = c.CharacterTypes;
        Name = c.Name;
        Level = level;
        Damage = c.Damage;
        HP = c.HP;

    }

    private Character WarriorSam(int level)
    {
        Character c = new Character();
        c.CharacterTypes = CharacterTypesByUniqueName.WarriorSam;

        float deltaHP = 5f;
        float deltadamage = 2f;

        c.Name = "WarriorSam";
        c.HP = 100 + deltaHP * level;
        c.Damage = 10 + deltadamage * level;
        return c;
    }

    private Character ShooterMike(int level)
    {
        Character c = new Character();
        c.CharacterTypes = CharacterTypesByUniqueName.ShooterMike;

        float deltaHP = 3f;
        float deltadamage = 3f;

        c.Name = "ShooterMike";
        c.HP = 80 + deltaHP * level;
        c.Damage = 15 + deltadamage * level;
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
        }

        return null;
    }
}

public enum CharacterTypesByUniqueName
{
    WarriorSam,
    ShooterMike
}

public enum CharacterQualities
{
    common,
    improved,
    unique,
    rare,
    legendary
}
