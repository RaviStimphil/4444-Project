using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class DamageManager : MonoBehaviour
{
    public Dictionary<string, AccuracyPackage> accuracyData;
    public float accuracyInterval;
    public int count = 1;
    void OnEnable(){
        SharedEvents.damageHit += DamageUnit;
        SharedEvents.skillHit += CollectAccuracyData;
    }
    void OnDisable(){
        SharedEvents.damageHit -= DamageUnit;
        SharedEvents.skillHit -= CollectAccuracyData;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        accuracyData = new Dictionary<string, AccuracyPackage>();
        StartCoroutine(AccuracyChecker());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowAccuracy();
        }
    }
    private IEnumerator AccuracyChecker(){
        while(true){
            yield return new WaitForSeconds(accuracyInterval);
            ShowAccuracy();
            count++;
        }
        
    }
    private void CollectAccuracyData(string label, bool hit){
        if(accuracyData.ContainsKey(label)){
            accuracyData[label].AddData(hit);
        }else{
            AccuracyPackage pack = new AccuracyPackage();
            pack.label = label;
            accuracyData.Add(label, pack);
            accuracyData[label].AddData(hit);
        }
    }
    private void DamageUnit(DamagePackage pack){
        //DamagePopup.Create(pack, FloatingText);
        if(pack.target.GetComponent("UnitStats") as UnitStats){
            pack.target.GetComponent<UnitStats>().TakeDamage(pack);
        }
        
    }
    public void ShowAccuracy(){
        StringBuilder dataString = new StringBuilder();
        dataString.Append((count * 10) + " minutes update:\n");
        foreach(var pair in accuracyData){
            dataString.Append("Accuracy Data for Ability: " + pair.Key + "\n");
            dataString.Append("Total Accuracy: " + pair.Value.TotalAccuracy(1f) + ".\n");
            dataString.Append("Last 20% Accuracy: " + pair.Value.TotalAccuracy(0.20f) + ".\n\n");
        }
        Debug.Log(dataString);
    }
}
