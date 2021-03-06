﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class MonsterBehaviour : MonoBehaviour
{
	public static Vector3 lastSeenPosition; // set by watchers
    public static float lastSeenPositionUpdateTime;
    public static bool gofast; // set by watchers

    private float lastUpdateTime;

    private Vector3 currentDestination;
    
    private UnityEngine.AI.NavMeshAgent _agent;
	public bool _moving;
	public float _sightRange;
	public GameObject _player;
    public Blink blinkinfo;

    public List<AudioClip> footstepSounds;
    public AudioSource footstepSource;

    public List<AudioClip> growlSounds;
    public AudioSource growlSource;

    public AudioSource spotyouSource;
    public AudioSource screamSource;

    public AudioSource killSource;
    
    public Transform raycastsParent;

    public Animator animatorController;

    private Coroutine idleCoroutine;

    private Coroutine growlCoroutine;

    public float _safecallvisibilitythreshold;

    public GameOverManager GameOver;

    public float pauseTimeWhenBumpsOnPlayer;
    public static bool pauseAndWaitForPlayer;

    // Use this for initialization
    void Start ()
    {
        lastSeenPosition = Vector3.zero;
        lastSeenPositionUpdateTime = Time.time;
        lastUpdateTime = lastSeenPositionUpdateTime;
        currentDestination = this.transform.position;
        _agent = this.GetComponent<UnityEngine.AI.NavMeshAgent> ();
        footstepCoroutine = null;
        gofast = false;
        pauseAndWaitForPlayer = true;

        idleCoroutine = StartCoroutine(WaitAndSetNewDestination(2.0f));


        growlCoroutine = StartCoroutine(WaitAndPlayGrowlSound(Random.Range(5.0f, 20.0f)));
    }

    IEnumerator WaitAndPlayGrowlSound(float timer)
    {
        yield return new WaitForSeconds(timer);

        growlSource.Stop();
        growlSource.clip = growlSounds[Random.Range(0, growlSounds.Count)];
        growlSource.Play();

        growlCoroutine = StartCoroutine(WaitAndPlayGrowlSound(Random.Range(5.0f,20.0f)));
    }

    IEnumerator WaitAndPlayScreamSound(float timer)
    {
        yield return new WaitForSeconds(timer);

        screamSource.Stop();
        screamSource.Play();
    }

    private Coroutine footstepCoroutine;

    IEnumerator WaitAndChangeFootStepSound(float timer)
    {
        yield return new WaitForSeconds(timer);
        
        footstepSource.Stop();
        footstepSource.clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
        footstepSource.Play();

        footstepCoroutine = StartCoroutine(WaitAndChangeFootStepSound(footstepSource.clip.length));
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (GameEngineBehaviour.instance.gameIsStarted)
        {
            CheckForPlayerInSight();

            if (pauseAndWaitForPlayer)
            {
                if ((Time.time - lastUpdateTime) > pauseTimeWhenBumpsOnPlayer)
                {
                    pauseAndWaitForPlayer = false;
                
                    // go idle and set new random destination
                    animatorController.SetBool("IsRunning", false);
                    animatorController.SetBool("IsWalking", false);

                    _agent.SetDestination(this.transform.position);
                    idleCoroutine = StartCoroutine(WaitAndSetNewDestination(3.0f));
                    footstepSource.Stop();
                    if (footstepCoroutine != null)
                    {
                        StopCoroutine(footstepCoroutine);
                        footstepCoroutine = null;
                    }
                }
            }
            else
            {
                if (lastSeenPositionUpdateTime > lastUpdateTime)
                {
                    // player has been seen recently, run towards it
                    if (idleCoroutine != null)
                    {
                        StopCoroutine(idleCoroutine);
                        idleCoroutine = null;
                    }

                    if (footstepCoroutine != null)
                    {
                        StopCoroutine(footstepCoroutine);
                        footstepCoroutine = null;
                    }

                    footstepSource.Stop();
                    footstepSource.clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
                    footstepSource.Play();

                    footstepCoroutine = StartCoroutine(WaitAndChangeFootStepSound(footstepSource.clip.length));


                    currentDestination = lastSeenPosition;
                    lastUpdateTime = lastSeenPositionUpdateTime;
                    animatorController.SetBool("IsWalking", true);

                    if (gofast)
                    {
                        animatorController.SetBool("IsRunning", true);
                        _agent.speed = 15.0f;
                    }

                    _agent.SetDestination(currentDestination);

                    if (!screamSource.isPlaying)
                    {
                        //screamSource.Stop();
                        screamSource.Play();
                    }
                }

                if ((Time.time - lastUpdateTime) > 10.0f)
                {
                    animatorController.SetBool("IsRunning", false);
                    _agent.speed = 5.0f;
                    gofast = false;
                }

                if (Vector3.Distance(currentDestination, this.transform.position) < 2.0f && idleCoroutine == null)
                {
                    // destination has been reached
                    // go idle and set new random destination
                    animatorController.SetBool("IsRunning", false);
                    animatorController.SetBool("IsWalking", false);

                    _agent.SetDestination(this.transform.position);
                    idleCoroutine = StartCoroutine(WaitAndSetNewDestination(3.0f));
                    footstepSource.Stop();
                    if (footstepCoroutine != null)
                    {
                        StopCoroutine(footstepCoroutine);
                        footstepCoroutine = null;
                    }
                }
            }
        }
    }

    IEnumerator WaitAndSetNewDestination(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (GameEngineBehaviour.instance.gameIsStarted)
        {
            Vector3 newDestination = new Vector3(Random.Range(-150, 200), this.transform.position.y, Random.Range(-150, 200));
            currentDestination = newDestination;
            _agent.SetDestination(newDestination);
            animatorController.SetBool("IsWalking", true);

            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }

            footstepSource.Stop();
            footstepSource.clip = footstepSounds[Random.Range(0, footstepSounds.Count)];
            footstepSource.Play();

            footstepCoroutine = StartCoroutine(WaitAndChangeFootStepSound(footstepSource.clip.length));

            idleCoroutine = null;
        }
        else
        {
            idleCoroutine = StartCoroutine(WaitAndSetNewDestination(3.0f));
        }
    }

    private float lastTimeSeesYouSoundHasBeenPlayed;

    public void CheckForPlayerInSight()
    {
        // send raycasts to see player
        foreach (Transform point in raycastsParent.transform)
        {
            Ray ray = new Ray(this.transform.position + 0.1f * Vector3.up, (point.position - this.transform.position));
            RaycastHit hit;
            Debug.DrawRay(ray.origin, (point.position - this.transform.position), Color.blue, 0.1f, true);
            if (Physics.Raycast(ray, out hit, _sightRange))
            {
                if (hit.collider.tag.Equals("Player") && blinkinfo.invisible < 0.8f)
                {
                    gofast = true;
                    // if player in sight, update last known position
                    pauseAndWaitForPlayer = false;
                    lastSeenPosition = _player.transform.position;
                    lastSeenPositionUpdateTime = Time.time;

                    if (Time.time - lastTimeSeesYouSoundHasBeenPlayed < 3.0f)
                    {
                        lastTimeSeesYouSoundHasBeenPlayed = Time.time;
                        spotyouSource.Stop();
                        spotyouSource.Play();
                    }

                    break;
                }
            }
        }
        // if no player in sight, nothing changes

        // Safe call : if player is too close, monster runs toward him
        if (Vector3.Distance (this.transform.position, _player.transform.position) < 25.0f && blinkinfo.invisible < _safecallvisibilitythreshold)
        {
            lastSeenPosition = _player.transform.position;
            lastSeenPositionUpdateTime = Time.time;
            _agent.SetDestination(lastSeenPosition);
            gofast = true;
            pauseAndWaitForPlayer = false;
        }
    }

    /*
    void OnTriggerEnter(Collider objectCol)
    {
		if (objectCol.tag == "Player") 
		{
            if (blinkinfo.invisible > 0.9f)
            {
                // Player is invisible, Monster stops and wait for him to open his eyes
                currentDestination = this.transform.position;
                _agent.SetDestination(currentDestination);
                lastUpdateTime = Time.time;
                animatorController.SetBool("IsWalking", false);
                animatorController.SetBool("IsRunning", false);
                pauseAndWaitForPlayer = true;
                if (idleCoroutine != null)
                {
                    StopCoroutine(idleCoroutine);
                    idleCoroutine = null;
                }
                Debug.Log("Monster bumps into player. Waiting");
            }
            else
            {
                //Game Over
                GameOver.LoseGame();
                animatorController.SetTrigger("Eating");
                //Application.LoadLevel("level1");
            }
        }
	}*/

    void OnTriggerStay(Collider objectCol)
    {
        if (objectCol.tag == "Player" && !GameEngineBehaviour.instance.IsGameOver())
        {
            if (blinkinfo.invisible > _safecallvisibilitythreshold)
            {
                if (!pauseAndWaitForPlayer)
                {
                    // Player is invisible, Monster stops and waits for him to open his eyes
                    pauseAndWaitForPlayer = true;
                    currentDestination = this.transform.position;
                    _agent.SetDestination(currentDestination);
                    lastUpdateTime = Time.time;
                    animatorController.SetBool("IsWalking", false);
                    animatorController.SetBool("IsRunning", false);
                    footstepSource.Stop();
                    if (idleCoroutine != null)
                    {
                        StopCoroutine(idleCoroutine);
                        idleCoroutine = null;
                    }
                    if (footstepCoroutine != null)
                    {
                        StopCoroutine(footstepCoroutine);
                        footstepCoroutine = null;
                    }
                }
            }
            else
            {
                //Game Over
                GameOver.LoseGame();
                animatorController.SetTrigger("Eating");
                //Application.LoadLevel("level1");
            }
        }
    }

    void StopIdle()
	{
		_moving = false;
	}
}
