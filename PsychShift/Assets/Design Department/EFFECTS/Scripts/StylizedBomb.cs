using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class StylizedBomb : MonoBehaviour
{
    [SerializeField] private GregCameraController cameraController;
    [SerializeField] private VisualEffect sparkParticles;

    private void Awake()
    {
        sparkParticles.Stop();
    }

    private void StartExplosion()
    {
        sparkParticles.Play();
        cameraController.StartExplosion();
    }
}
