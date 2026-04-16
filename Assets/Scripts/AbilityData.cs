using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "ScriptableObjects/AbilityData")]
class AbilityData : ScriptableObject {
    public string label;
    
    public AnimationClip animationClip;
    [Range(0.1f, 4f)] public float castTime = 2f;
    //public ProjectileMove vfxPrefab;
    
    [SerializeReference] public List<AbilityEffect> effects;

    void OnEnable() {
        if (string.IsNullOrEmpty(label)) label = name;
        if (effects == null) effects = new List<AbilityEffect>();
    }
}

[Serializable]
abstract class AbilityEffect {
    public abstract void Execute(GameObject caster, GameObject target);
}

[Serializable]
class DamageEffect : AbilityEffect {
    public int amount;
    
    public override void Execute(GameObject caster, GameObject target) {
        /*target.GetComponent<Health>().ApplyDamage(amount);
        Debug.Log($"{caster.name} dealt {amount} damage to {target.name}");
    */}
}

[Serializable]
class KnockbackEffect : AbilityEffect {
    public float force;

    public override void Execute(GameObject caster, GameObject target) {
        /*var dir = (target.transform.position - caster.transform.position).normalized;
        target.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        Debug.Log($"{caster.name} knocked back {target.name} with force {force}");
    */}
}
