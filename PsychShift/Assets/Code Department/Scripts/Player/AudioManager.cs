using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace AudioEnemy 
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public AudioClip[] enemyClips;
        public AudioClip[] bombEnemy;
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }
    }

}

