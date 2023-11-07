using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PapaUnlockThing : MonoBehaviour
{
    [SerializeField] private List<ChildUnlockThing> childUnlockThings;

    private Dictionary<ChildUnlockThing, bool> childUnlockThingStates = new Dictionary<ChildUnlockThing, bool>();

    protected void UpdateDictionary(ChildUnlockThing thing, bool state)
    {
        childUnlockThingStates[thing] = state;
        /* foreach(var state in childUnlockThingStates.Values)
        {
            if(!state)
            {
                return;
            }
        } */
        if(childUnlockThingStates.Any(v => v.Value == false))
        {
            return;
        }

        AllUnlockConditionsMet();
    }

    public abstract void AllUnlockConditionsMet();

}

public class ExamplePuzzleScript : PapaUnlockThing
{
    public override void AllUnlockConditionsMet()
    {
        // Open door
    }
}




public abstract class ChildUnlockThing : MonoBehaviour 
{
    public abstract void TriggeredUnlock(ChildUnlockThing thing, bool state);
}
