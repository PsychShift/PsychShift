using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IManipulate
{
    bool IsInteracted { get; set; }
    bool CanInteract { get; set; }
    void Interacted();
    void Interact();
    void ReverseInteract();
}
