using System.Collections;
using UnityEngine;

public class BossBarFadeScript : MonoBehaviour
{
    public static BossBarFadeScript Instance;
   public CanvasGroup BossBar;
   public float fadeSpeed = 1;
   [SerializeField] private bool fadeInBar = false;
   [SerializeField] private bool fadeOutBar = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        BossBar.alpha = 0;
        BossBar.enabled = true;
    }

    public IEnumerator FadeIn()
    {
        BossBar.enabled = true;
        BossBar.alpha = 0;
        fadeInBar = true;
        while (fadeInBar)
        {
            if (BossBar.alpha >= 0)
            {
                BossBar.alpha += Time.deltaTime * fadeSpeed;
                if (BossBar.alpha >= 1)
                {
                    fadeInBar = false;
                }
            }
            yield return null;
        }
        BossBar.alpha = 1;
    }
    public IEnumerator FadeOut()
    {
        fadeOutBar = true;
        while (fadeOutBar)
        {
            if (BossBar.alpha >= 0)
            {
                BossBar.alpha -= Time.deltaTime * fadeSpeed;
                if (BossBar.alpha == 0)
                {
                    fadeOutBar = false;
                }
            }
            yield return null;
        }
        BossBar.alpha = 0;
        //BossBar.enabled = false;
    }

    /* // Update is called once per frame
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
    } */
}
