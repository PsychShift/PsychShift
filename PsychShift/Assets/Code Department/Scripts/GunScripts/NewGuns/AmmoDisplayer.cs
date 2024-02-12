using System.Collections;
using System.Collections.Generic;
using Guns.Demo;
using TMPro;
using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoDisplayer : MonoBehaviour
{

    private TextMeshProUGUI AmmoText;
    private void Awake()
    {
        AmmoText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        AmmoText.SetText($"{PlayerGunSelector.Instance.ActiveGun.AmmoConfig.CurrentClipAmmo}" );
    }
}
