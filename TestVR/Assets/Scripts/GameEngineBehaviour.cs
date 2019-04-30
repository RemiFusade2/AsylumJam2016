using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEngineBehaviour : MonoBehaviour {

    public static GameEngineBehaviour instance;

    public GameObject MainMenuPanel;
    public GameObject HelpPanel;
    public GameObject GameOverPanel;
    public GameObject SuccessPanel;

    public GameObject BlinkAlphaPanel;

    public AudioClip MenuHoverSound;
    public AudioClip MenuClickSound;

    public List<string> HintMessages;
    public List<string> HintMessagesWithoutKey;
    public List<string> HintMessagesWithKey;

    public Blink BlinkScript;

    private Coroutine checkformessages;

    public GameObject EyesClosedMessage;

    public bool gameIsStarted;

    public BackgroundSoundScript BkgSoundScript;

    public bool HasKey;

    public bool messageChange;

    void Awake()
    {
        instance = this;
    }

    IEnumerator WaitAndCheckForMessage(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (BlinkScript.invisible >= 0.95f && EyesClosedMessage.activeInHierarchy && messageChange)
        {
            if (HasKey)
            {
                int randomIndex = Random.Range(0, HintMessages.Count + HintMessagesWithKey.Count);
                if (randomIndex < HintMessages.Count)
                {
                    EyesClosedMessage.GetComponent<Text>().text = HintMessages[Random.Range(0, HintMessages.Count)];
                }
                else
                {
                    EyesClosedMessage.GetComponent<Text>().text = HintMessagesWithKey[Random.Range(0, HintMessagesWithKey.Count)];
                }
            }
            else
            {
                int randomIndex = Random.Range(0, HintMessages.Count + HintMessagesWithoutKey.Count);
                if (randomIndex < HintMessages.Count)
                {
                    EyesClosedMessage.GetComponent<Text>().text = HintMessages[Random.Range(0, HintMessages.Count)];
                }
                else
                {
                    EyesClosedMessage.GetComponent<Text>().text = HintMessagesWithoutKey[Random.Range(0, HintMessagesWithoutKey.Count)];
                }
            }
            EyesClosedMessage.GetComponent<Animator>().SetTrigger("Show");
        }

        checkformessages = StartCoroutine(WaitAndCheckForMessage(timer));
    }


    // Use this for initialization
    void Start ()
    {
        gameIsStarted = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }

        if (!gameIsStarted && !GameOverPanel.activeInHierarchy && !SuccessPanel.activeInHierarchy && BlinkScript.invisible >= 1)
        {
            StartGame();
        }
	}

    public bool IsGameOver()
    {
        return GameOverPanel.activeInHierarchy;
    }

    public void Escape()
    {
        if (HelpPanel.activeInHierarchy)
        {
            this.GetComponent<AudioSource>().clip = MenuClickSound;
            this.GetComponent<AudioSource>().Play();
            HelpPanel.SetActive(false);
        }
        else if (GameOverPanel.activeInHierarchy)
        {
            /*
            this.GetComponent<AudioSource>().clip = MenuClickSound;
            this.GetComponent<AudioSource>().Play();
            GameOverPanel.SetActive(false);*/
        }
        else if (SuccessPanel.activeInHierarchy)
        {
            /*
            this.GetComponent<AudioSource>().clip = MenuClickSound;
            this.GetComponent<AudioSource>().Play();
            SuccessPanel.SetActive(false);*/
        }
        else if (MainMenuPanel.activeInHierarchy)
        {
            QuitGame();
        }
        else if (!gameIsStarted)
        {
            this.GetComponent<AudioSource>().clip = MenuClickSound;
            this.GetComponent<AudioSource>().Play();
            MainMenuPanel.SetActive(true);
        }
        else if (gameIsStarted)
        {
            this.GetComponent<AudioSource>().clip = MenuClickSound;
            this.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("level1");
        }
    }

    public void StartGame()
    {
        gameIsStarted = true;
        this.GetComponent<AudioSource>().clip = MenuClickSound;
        this.GetComponent<AudioSource>().Play();
        MainMenuPanel.SetActive(false);
        HelpPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        checkformessages = StartCoroutine(WaitAndCheckForMessage(15.0f));
        BlinkAlphaPanel.SetActive(true);
        BkgSoundScript.StartBackgroundSound();
        EyesClosedMessage.GetComponent<Animator>().SetTrigger("Show");
        messageChange = true;
    }


    public void OpenMenu()
    {
        MainMenuPanel.SetActive(true);
        Cursor.visible = true;
    }

    public void ShowHelpPanel()
    {
        this.GetComponent<AudioSource>().volume = 0.8f;
        this.GetComponent<AudioSource>().clip = MenuClickSound;
        this.GetComponent<AudioSource>().Play();
        HelpPanel.SetActive(true);
    }

    public void QuitGame()
    {
        this.GetComponent<AudioSource>().volume = 0.8f;
        this.GetComponent<AudioSource>().clip = MenuClickSound;
        this.GetComponent<AudioSource>().Play();
        Application.Quit();
    }

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        BkgSoundScript.StopBackgroundSound();
    }

    public void ShowSuccessPanel()
    {
        SuccessPanel.SetActive(true);
        BkgSoundScript.StopBackgroundSound();
    }

    public void MenuButtonHover()
    {
        this.GetComponent<AudioSource>().volume = 0.2f;
        this.GetComponent<AudioSource>().clip = MenuHoverSound;
        this.GetComponent<AudioSource>().Play();
    }
}
