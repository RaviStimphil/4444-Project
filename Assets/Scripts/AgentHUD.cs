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
    public Image basicIcon;
    public Image specialIcon;

    void Update()
    {
        rewardText.text = label + " Reward: " + agent.GetCumulativeReward().ToString("F1");
        hpBar.value = stats.currentHP;
        hpBar.maxValue = stats.maxHP;

        float percent = stats.currentHP / stats.maxHP;
        hpFill.color = Color.Lerp(Color.red, Color.green, percent);

        SpriteRenderer sr = agent.basicAttack.ammoInfo.GetComponent<SpriteRenderer>();
        basicIcon.sprite = sr.sprite; 

        SpriteRenderer sr2 = agent.specialAttack.ammoInfo.GetComponent<SpriteRenderer>();
        specialIcon.sprite = sr2.sprite; 

    }
}