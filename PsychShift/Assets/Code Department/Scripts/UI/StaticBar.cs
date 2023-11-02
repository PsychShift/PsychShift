using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticBar : MonoBehaviour
{
    //[SerializeField] private Player.PlayerController
    public Slider staticBar;

    public int maxStatic = 100;
    public int currentStatic;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static StaticBar instance;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStatic = maxStatic;
        staticBar.maxValue = maxStatic;
        staticBar.value = maxStatic;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UseStatic(int amount)
    {
        if(currentStatic - amount >= 0)
        {
            currentStatic -= amount;
            staticBar.value = currentStatic;

            if(regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStatic());
        }
        else
        {
            //Player.SwitchMode(false);
        }
    }

    private IEnumerator RegenStatic()
    {
        yield return new WaitForSeconds(2);

        while(currentStatic < maxStatic)
        {
            currentStatic += maxStatic / 100;
            staticBar.value = currentStatic;
            yield return regenTick;
        }
        regen = null;
    }
}
