using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class AccuracyPackage
{
    public string label;
    public List<int> accuracyChecks;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public AccuracyPackage()
    {
        accuracyChecks = new List<int>();
    }
    public void AddData(bool hit){
        if(hit){
            accuracyChecks.Add(1);
        }else{
            accuracyChecks.Add(0);
        }
    }
    public float TotalAccuracy(float recentPercentage){
        int i = (int) Mathf.Round(accuracyChecks.Count - (recentPercentage * accuracyChecks.Count) );
        int success = 0;
        int amount = 0;
        for(i = i; i < accuracyChecks.Count; i++){
            amount++;
            if(accuracyChecks[i] == 1){
                success++;
            }
        }
        return (float) (amount - success)/amount; 
    }
}
