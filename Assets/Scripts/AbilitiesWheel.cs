using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesWheel : MonoBehaviour
{
    [HideInInspector]public int abilitiesChoice;
    [SerializeField] public Image cooldownFill;
    [SerializeField] private Image[] abilities;
    private Color darkGrey;

    [SerializeField]public int cooldownDuration;
    private int remainingCooldownDuration;

    void Start()
    {
        cooldownDuration = 0;
        BeginWheelCountdown(cooldownDuration);
        abilitiesChoice = 0;
        darkGrey = new Color(0.25f, 0.25f, 0.25f, 1); 
    }


    void Update()
    {
        //Cycle through abilities wheel
        if (Input.GetKeyDown("g"))
        {
            if(abilitiesChoice < 3)
            {
                abilitiesChoice++;
            }
            else
            {
                abilitiesChoice = 0;
            }


        }
        UpdateAbilities();
    }

    public void UpdateAbilities()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if(i == abilitiesChoice)
            {
                abilities[i].color = Color.white;
            }
            else
            {
                abilities[i].color = darkGrey;
            }
        }
    }

    public void BeginWheelCountdown(int Second)
    {
        remainingCooldownDuration = Second;
        StartCoroutine(UpdateTime());
    }
    private IEnumerator UpdateTime()
    {
        //resonsible for wheel loading & countdown
        while(remainingCooldownDuration >= 0)
        {
            cooldownFill.fillAmount = Mathf.InverseLerp(0, cooldownDuration, remainingCooldownDuration);
            remainingCooldownDuration--;
            yield return new WaitForSeconds(1f);
            Debug.Log(remainingCooldownDuration);
            Debug.Log(cooldownDuration);
            if (remainingCooldownDuration <= 0)
            {
                cooldownDuration = 0;
            }
        }
    }

    
}
