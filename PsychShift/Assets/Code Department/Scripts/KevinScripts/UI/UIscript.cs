using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using TMPro;

public class UIscript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI statFlowText;
    public GameObject StaticImage;
    public GameObject FlowImage;
    

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.OnSwitchPressed += SwitchMode;//Do the thing when dis pressed
        if(FlowImage!=null)
            FlowImage.SetActive(true);
        if(StaticImage!=null)
            StaticImage.SetActive(false);
    }

    void OnDisable()
    {
        InputManager.Instance.OnSwitchPressed -= SwitchMode;
    }

    private void SwitchMode(bool mode_Static)
    {
        if(mode_Static)
        {
            //UI dis
            statFlowText.text = "LT/L shift:Static";
            StaticImage.SetActive(true);
            if(FlowImage != null)
                FlowImage.SetActive(false);
        }
        else
        {
            //UI DAT
            statFlowText.text = "LT/L shift:Flow";
            StaticImage.SetActive(false);
            if(FlowImage!=null)
                FlowImage.SetActive(true);
        }
    }
}
