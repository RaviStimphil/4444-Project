using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData")]
public class AbilityData : ScriptableObject {
    public string label;
    
    public AnimationClip animationClip;
    //public ProjectileMove vfxPrefab;
    
    [SerializeReference] public List<AbilityEffect> effects;

    void OnEnable() {
        if (string.IsNullOrEmpty(label)) label = name;
        if (effects == null) effects = new List<AbilityEffect>();
    }
}

[Serializable]
public abstract class AbilityEffect {
    public abstract void Execute(GameObject caster, GameObject target);
}

[Serializable]
public class DamageEffect : AbilityEffect {
    public int amount;
    
    public override void Execute(GameObject caster, GameObject target) {
        DamagePackage damage = new DamagePackage(caster, target, amount);  
        SharedEvents.SendDamage(damage); 
    }
}

[Serializable]
public class KnockbackEffect : AbilityEffect {
    public float force;

    public override void Execute(GameObject caster, GameObject target) {
        var dir = (target.transform.position - caster.transform.position).normalized;
        target.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
        //Debug.Log($"{caster.name} knocked back {target.name} with force {force}");
    }
}

[Serializable]
public class SelfDamageEffect : AbilityEffect {
    public int amount;
    
    public override void Execute(GameObject caster, GameObject target) {
        DamagePackage damage = new DamagePackage(caster, caster, amount);  
        SharedEvents.SendDamage(damage); 
    }
}
