using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SharedEvents : MonoBehaviour
{
    public static event Action<DamagePackage> damageHit;
    
    public static void SendDamage(DamagePackage info){
        damageHit?.Invoke(info);
    }
}
