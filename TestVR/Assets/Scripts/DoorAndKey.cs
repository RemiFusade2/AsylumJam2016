using UnityEngine;
using System.Collections;

public class DoorAndKey : MonoBehaviour {

    public string pickableMessage;

    public Animator ShelterAnimator;

    public GameEngineBehaviour gameEngine;

    public bool OpenDoor()
    {
        if (gameEngine.HasKey)
        {
            ShelterAnimator.SetBool("DoorOpen", true);
            return true;
        }
        return false;
    }

}
