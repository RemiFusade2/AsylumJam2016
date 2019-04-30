using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {


    public GameObject Player;
    public GameObject Camera;

    public Blink BlinkScript;

    public GameObject Monster;
    public Transform MonsterHead;

    public GameObject GameOverPanel;
    public GameObject RedOverlay;

    public List<AudioSource> OtherSoundSources;

    public AudioSource GameOverSoundSource;
    public AudioClip GameOverSound;
    

	// Use this for initialization
	void Start () {
        shutingeyes = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoseGame()
    {
        if (BlinkScript.invisible < 0.95f)
        {
            // Player sees things
            Ray ray = new Ray(Camera.transform.position, (Monster.transform.position - Camera.transform.position));
            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50))
            {
                if (hit.collider.tag.Equals("Monster"))
                {
                    // Player sees Monster
                    PlayGameOver_Front();
                }
                else
                {
                    // Player is running away
                    PlayGameOver_Back();
                }
            }
        }
        else
        {
            // Player has eyes closed
            // Here we can have fun
            PlayGameOver_EyesShut();
        }
    }

    private void PlayGameOver_Front()
    {
        // Monster is in front of player and it has seen him
        // It is too close to do anything
        // Player is attacked and screen turns to red has he is eaten
        // Eyes wide open, fade to red really quickly, play sound
        Player.GetComponent<FirstPersonController>().enabled = false;
        Player.GetComponent<MouseLook>().enabled = false;
        Camera.GetComponent<MouseLook>().enabled = false;
        //Camera.GetComponent<Blink>().ini = false;
        BlinkScript.ForceInvisible(0);

        Vector3 newPosition = Monster.transform.position + Monster.transform.forward * 8.0f;
        newPosition = new Vector3(newPosition.x, 0.3f, newPosition.z);
        //Camera.transform.position = newPosition;
        Camera.transform.parent = null;
        SphereCollider cameraCapsule = Camera.AddComponent<SphereCollider>();
        cameraCapsule.material = new PhysicMaterial();
        cameraCapsule.material.bounciness = 1.0f;
        cameraCapsule.radius = 0.5f;
        Rigidbody cameraRigidbody = Camera.AddComponent<Rigidbody>();
        cameraRigidbody.useGravity = true;
        cameraRigidbody.angularDrag = 1000;
        StartCoroutine(WaitAndLookAt(0.1f, MonsterHead));


        //Camera.transform.LookAt(MonsterHead);
       // Camera.GetComponent<Camera>().fieldOfView = 60.0f;

        GameOverSoundSource.Play();
        foreach(AudioSource source in OtherSoundSources)
        {
            source.mute = true;
        }
        GameOverPanel.SetActive(true);
        RedOverlay.SetActive(true);
        StartCoroutine(WaitAndDisplayRedOveraly(0.1f));
    }

    private void PlayGameOver_Back()
    {
        // Monster is behind the player and it has seen him
        // It is too close to do anything
        // Player is draged down in the forest
        // Eyes wide open, drop light, slide camera on the ground, play sound, close eyes (no monster seen?)
        PlayGameOver_Front();
    }
    
    private void PlayGameOver_EyesShut()
    {
        // Monster is close to the player but player has eyes closed
        // Sounds are really close, but suddently stop
        // Player believes he is safe
        // As soon as he opens his eyes => Eyes wide open, Monster in front of him, PlayGameOver_Front()
        // OR As soon as he opens his eyes => Nothing in front of him, is he safe?, he can walk 5 seconds, Monster appears above him, fade to red, play sound
        PlayGameOver_Front();
    }

    IEnumerator WaitAndLookAt(float timer, Transform target)
    {
        yield return new WaitForSeconds(timer);

        Camera.transform.LookAt(target);
        StartCoroutine(WaitAndLookAt(0.02f, target));
    }

    IEnumerator WaitAndShutEyes(float timer)
    {
        yield return new WaitForSeconds(timer);
        BlinkScript.ForceInvisible(BlinkScript.invisible + 0.01f);
        if (BlinkScript.invisible > 1)
        {
            //Application.LoadLevel("level1");
            SceneManager.LoadScene("level1");
        }
        else
        {
            StartCoroutine(WaitAndShutEyes(0.02f));
        }
    }

    private bool shutingeyes;

    IEnumerator WaitAndDisplayRedOveraly(float timer)
    {
        yield return new WaitForSeconds(timer);

        RedOverlay.GetComponent<Image>().color = new Color(1, 0, 0, RedOverlay.GetComponent<Image>().color.a + 0.01f);


        if (RedOverlay.GetComponent<Image>().color.a >= 0.5f && !shutingeyes)
        {
            shutingeyes = true;
            StartCoroutine(WaitAndShutEyes(1.0f));
        }

        StartCoroutine(WaitAndDisplayRedOveraly(0.05f));
    }

}
