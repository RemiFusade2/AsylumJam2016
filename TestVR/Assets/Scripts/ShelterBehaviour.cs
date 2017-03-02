using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShelterBehaviour : MonoBehaviour
{
    public GameObject winMessage;

    public GameObject lightInside;

    public BackgroundSoundScript bkgSound;

    public List<AudioClip> footstepsWood;

    public FirstPersonController player;

    public Animator shelterAnimator;

    public GameObject door;

    public GameObject Monster;

    public Blink BlinkScript;

    public GameEngineBehaviour GameEngine;

    public Text hiddenMessage;

    public GameObject hiddenMessagePanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            winMessage.SetActive(true);
            lightInside.SetActive(true);
            bkgSound.StopBackgroundSound();
            player.SetFootStepsSound(footstepsWood);
            shelterAnimator.SetBool("DoorOpen", false);
            door.tag = "Untagged";
            Monster.SetActive(false);

            GameEngine.messageChange = false;
            hiddenMessage.text = "Thanks for playing";
            StartCoroutine(WaitAndForceCloseEyes(5.0f));
        }
    }

    IEnumerator WaitAndForceCloseEyes(float timer)
    {
        yield return new WaitForSeconds(timer);
        BlinkScript.ForceClose();

        StartCoroutine(WaitAndShowMessage(3.0f));
    }

    IEnumerator WaitAndShowMessage(float timer)
    {
        yield return new WaitForSeconds(timer);
        hiddenMessagePanel.GetComponent<Animator>().SetTrigger("Show");
        
        StartCoroutine(WaitAndLoadLevelAgain(5.0f));
    }

    IEnumerator WaitAndLoadLevelAgain(float timer)
    {
        yield return new WaitForSeconds(timer);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("level1");
    }
}
