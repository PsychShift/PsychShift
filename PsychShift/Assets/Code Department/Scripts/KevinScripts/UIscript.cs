using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using TMPro;

public class UIscript : MonoBehaviour
{
    [SerializeField]
    private PlayerStateMachine1 uiRef;
    [SerializeField]
    TextMeshProUGUI statFlowText;

    // Start is called before the first frame update
    void Start()
    {
        InputManager1.Instance.OnSwitchPressed+= SwitchMode;//Do the thing when dis pressed
    }

    // Update is called once per frame
    void Update()
    {
        //if(uiRef.StaticMode == true)
    }

    void OnDisable()
    {
        InputManager1.Instance.OnSwitchPressed-= SwitchMode;
    }

    private void SwitchMode(bool mode_Static)
    {
        if(mode_Static)
        {
            //UI dis
            statFlowText.text = "LT/L shift:Static";
        }
        else
        {
            //UI DAT
            statFlowText.text = "LT/L shift:Flow";
        }

    }
}
