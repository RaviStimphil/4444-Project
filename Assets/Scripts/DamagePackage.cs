using UnityEngine;

public class DamagePackage 
{
    public GameObject source;
    public GameObject target;
    public int amount;
    public float damagePercent;
    public Vector2 knockbackDirection;
    public float knockbackForce;

    public DamagePackage(){

    }
    public DamagePackage(GameObject source, GameObject target, int amount){
        this.source = source;
        this.target = target;
        this.amount = amount;
    }
    public void KnockbackAdjust(Vector2 direct, float force){
        knockbackDirection = direct;
        knockbackForce = force;
    }
    public Alignment TargetAlignment(){
        if(target == null){
            return Alignment.Zearo;
        }
        return target.GetComponent<UnitStats>().align;
    }    
    public Alignment SourceAlignment(){
        if(source == null){
            return Alignment.Zearo;
        }
        return source.GetComponent<UnitStats>().align;
    }               
}