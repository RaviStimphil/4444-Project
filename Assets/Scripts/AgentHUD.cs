using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AgentHUD : MonoBehaviour
{
    public BattlerAgent agent;
    public UnitStats stats;
    public TextMeshProUGUI rewardText;
    public Slider hpBar;
    public Image hpFill;
    public string label = "Agent 1";

    void Update()
    {
        rewardText.text = label + " Reward: " + agent.GetCumulativeReward().ToString("F1");
        hpBar.value = stats.currentHP;
        hpBar.maxValue = stats.maxHP;

        float percent = stats.currentHP / stats.maxHP;
        hpFill.color = Color.Lerp(Color.red, Color.green, percent);
    }
}