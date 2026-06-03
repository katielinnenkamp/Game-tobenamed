using UnityEngine;
using UnityEngine.SceneManagement;

public class parkourmanager : MonoBehaviour
{
    public AnomalyText text;
    public static parkourmanager instance;

    public GameObject door;
    public GameObject player;
    public GameObject fogwall;

    [SerializeField]
    private Vector3 doorclosepos;

    [SerializeField]
    private Vector3 dooropenpos;

    [SerializeField]
    private Vector3 playerstartpos;

    [SerializeField]
    private Vector3 fogstartpos;

    public amstate curstate;

    private float movetimer;

    [SerializeField]
    private float timetomove;

    [SerializeField]
    private float doorspeed;

    [SerializeField]
    private float fogspeed;

    [SerializeField]
    private int lives = 5;

    private bool fogmoving = false;

    private bool started = false;

    //private string newScene = "";

    void Awake()
    {
        instance = this;

        movetimer = 0;
        curstate = amstate.IDLE;

        door.transform.localPosition = doorclosepos;
        player.transform.position = playerstartpos;
        fogwall.transform.position = fogstartpos;
    }

    void Start()
    {
        ShowIntro();
    }

    void Update()
    {
        switch(curstate)
        {
            case amstate.IDLE:
                if(Input.GetMouseButtonDown(0))
                {
                    StartParkour();
                }
                break;
            
            case amstate.DOORCLOSING:
                door.transform.localPosition = Vector3.MoveTowards(
                    door.transform.localPosition,
                    doorclosepos,
                    doorspeed * Time.deltaTime
                );

                if((door.transform.localPosition - doorclosepos).sqrMagnitude <= 0.01f)
                {
                    ChangeState(amstate.IDLE);
                }
                break;

            case amstate.DOOROPENING:
                door.transform.localPosition = Vector3.MoveTowards(
                    door.transform.localPosition,
                    dooropenpos,
                    doorspeed * Time.deltaTime
                );

                if((door.transform.localPosition - dooropenpos).sqrMagnitude <= 0.01f)
                {
                    fogmoving = true;
                    ChangeState(amstate.IDLE);
                }
                break;

            case amstate.MOVING:
                movetimer += Time.deltaTime;

                if(movetimer >= timetomove)
                {
                    ChangeState(amstate.DOOROPENING);
                }
                break;
        }

        if(fogmoving)
        {
            fogwall.transform.position += Vector3.left * fogspeed * Time.deltaTime;
        }
    }

    private void ShowIntro()
    {
        text.ShowMessage(
            "Welcome to the parkour zone. \n" +
            "You have 5 tries to make it to freedom before you are trapped here forever.\n" +
            "Fall into the water and you lose a life.\n" +
            "Left click when you're ready to start, but you better be faster than the Fog.",
            StartParkour
        );
    }

    public void StartParkour()
    {
        if(started)
        {
            return;
        }

        started = true;
        ChangeState(amstate.DOOROPENING);
    }

    public void LoseLife()
    {
        lives--;

        if(lives <= 0)
        {
            SceneManager.LoadScene("FullGameLose");
            return;
        }

        RespawnPlayer();
        CancelInvoke(nameof(HideLifeMessage));

        text.ShowMessage(
            "Lives Remaining: " + lives,
            null
        );

        Invoke(nameof(HideLifeMessage), 2f);
    }

    private void RespawnPlayer()
    {
        player.transform.position = playerstartpos;
        fogwall.transform.position = fogstartpos;
        fogmoving = true;
    }

    private void HideLifeMessage()
    {
        text.HideMessage();
    }

    public void WinGame()
    {
        SceneManager.LoadScene("FullGameWin");
    }

    public void ChangeState(amstate newstate)
    {
        if(curstate == newstate)
        {
            return;
        }

        switch(newstate)
        {
            case amstate.IDLE:
                break;

            case amstate.DOORCLOSING:
                fogmoving = false;
                break;

            case amstate.DOOROPENING:
                break;

            case amstate.MOVING:
                movetimer = 0;
                break;
        }

        curstate = newstate;
    }
}