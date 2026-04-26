using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SharedEvents : MonoBehaviour
{
    public static event Action<DamagePackage> damageHit; //When an attack hits an agent
    public static event Action<DamagePackage> damageTaken; //Announcement the agent does with how much damage it took. 
    public static event Action<GameObject> unitDeath;
    public static event Action startRound;
    public static event Action<string, bool> skillHit;

    public static void SendDamage(DamagePackage info){
        damageHit?.Invoke(info);
    }
    public static void AnnounceDamage(DamagePackage info){
        damageTaken?.Invoke(info);
    }

    public static void AnnounceDeath(GameObject unit){
        unitDeath?.Invoke(unit);
    }
    public static void BeginRound(){
        startRound?.Invoke();
    }
    public static void DidAbilityHit(string label, bool hit){
        skillHit?.Invoke(label, hit);
    }
}
