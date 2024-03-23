using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBarFadeScript : MonoBehaviour
{
   public CanvasGroup BossBar;
   private WaitForSeconds MenuTick = new WaitForSeconds(0.1f);
   private Coroutine WaitforFade;
   [SerializeField] private bool fadeInBar = false;
   [SerializeField] private bool fadeOutBar = false;
    // Start is called before the first frame update
    void Start()
    {
        BossBar.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeInBar)
        {
            if (BossBar.alpha >= 0)
            {
                BossBar.alpha += Time.deltaTime;
                if (BossBar.alpha >= 1)
                {
                    fadeInBar = false;
                }
            }
        }
        if (fadeOutBar)
        {
            if (BossBar.alpha >= 0)
            {
                BossBar.alpha -= Time.deltaTime;
                if (BossBar.alpha == 0)
                {
                    fadeOutBar = false;
                }
            }
        }
    }
     private void OnTriggerEnter(Collider other) 
    {
        fadeInBar = true;
    }
}
