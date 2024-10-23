using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    GameObject staminaBarObject;
    DashBar staminaBar;

    public bool isSprint;
    public int currentStamina = 100;
    public int maxStamina = 100;
    public float recoveryTime = 0;
    bool sprintTick = true;
    public bool recoveryTick = true;

    // Start is called before the first frame update
    void Awake()
    {
        staminaBarObject = GameObject.Find("Dash Bar");
        staminaBar = staminaBarObject.gameObject.GetComponent<DashBar>();
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxDash(maxStamina);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSprint && sprintTick)
        {
            recoveryTime = 0;
            currentStamina -= 1;
            sprintTick = false;
            StartCoroutine(SprintBar());

            //reinitializes the recovery tick
            recoveryTick = true;
        }
        else if (!isSprint)
        {
            //counting the time of not sprinting
            recoveryTime += Time.deltaTime;

            // checks if 4 seconds has past before recovering stamina
            if (recoveryTime >= 4f && recoveryTick)
            {
                currentStamina += 1;
                recoveryTick = false;
                StartCoroutine(RecoverStamina());

                //reinitializes the sprint tick
                sprintTick = true;
            } 
        }

        staminaBar.SetDash(currentStamina);

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        recoveryTime = Mathf.Clamp(recoveryTime, 0, 4);
    }

    IEnumerator RecoverStamina()
    {
        //stamina recovery interval
        yield return new WaitForSeconds(0.2f);
        recoveryTick = true;
    }

    IEnumerator SprintBar()
    {
        //stamina consumption interval
        yield return new WaitForSeconds(0.3f);
        sprintTick = true;
    }
}
