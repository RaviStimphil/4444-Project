using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DamageManager : MonoBehaviour
{
    void OnEnable(){
        SharedEvents.damageHit += DamageUnit;
    }
    void OnDisable(){
        SharedEvents.damageHit -= DamageUnit;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void DamageUnit(DamagePackage pack){
        //DamagePopup.Create(pack, FloatingText);
        if(pack.target.GetComponent("UnitStats") as UnitStats){
            pack.target.GetComponent<UnitStats>().TakeDamage(pack.damageAmount);
        }
        
    }
    
}
