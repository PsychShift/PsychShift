using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainJuiceBarTest : MonoBehaviour
{
    
    public Slider BrainBar;

    public int maxBrain = 100;
    public int currentBrain;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static BrainJuiceBarTest instance;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentBrain = maxBrain;
        BrainBar.maxValue = maxBrain;
        BrainBar.value = maxBrain;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
public bool UseBrain(int amount)
    {
        if(currentBrain - amount >= 0)
        {
            currentBrain -= amount;
            BrainBar.value = currentBrain;

            if(regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenBrain());
            return true;
        }
        else
        {
            Debug.Log("Not enough brain juice");
            return false;
        }
    }
    int distanceToCostModifier = 2;
    public int DistanceCheck(float distance)
    {
        int cost = (int)Mathf.Floor(distance/distanceToCostModifier);
        Debug.Log(cost);
        return cost;
    }

    private IEnumerator RegenBrain()
    {
        yield return new WaitForSeconds(5);

        while(currentBrain < maxBrain)
        {
            currentBrain += maxBrain / 100;
            BrainBar.value = currentBrain;
            yield return regenTick;
        }
        regen = null;
    }
}
