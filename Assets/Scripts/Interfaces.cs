using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaces {}

public interface IPlayer
{
    int TeamID { get; }
    Character Character { get; }
    void ReceiveHit(IPlayer player, Action<Character> killInfo);
    void ReceiveHeal(IPlayer player);
    Transform PlayerTransform { get; }
    bool SetBusy(bool isBusy);
    float CurrentSpeed { get; }
}
