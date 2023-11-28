using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlateManager pm;
    Rigidbody rb;
    Achievement ac;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float force = 5;
    [SerializeField] float waitTime = 1;
    [SerializeField] float powerUpDuration = 1;
    [Range(0.1f,1f)]
    [SerializeField] float maxPlayTimer = 0.2f;
    [SerializeField] float autoPlayTimer = 0.2f;
    public TextMeshProUGUI text;
    public TextMeshProUGUI historyText;
    public TextMeshProUGUI stateText;
    public Transform model;
    bool replaying;
    Plate touchedPlate;

    public Material defaultMat;
    public Material powerupMat;

    // Commands
    Command cmd_W = new MoveForwardCommand();
    Command cmd_S = new MoveBackCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_D = new MoveRightCommand();

    Command lastCommand;

    // Stacks
    Stack<Command> commands = new Stack<Command>();
    Stack<Command> undoed = new Stack<Command>();

    enum State
    {
        Standing,
        Jumping,
        Crouching,
    }

    public enum PowerState
    {
        NONE,
        PowerUp
    }

    State currentState = State.Standing;
    public PowerState currentPS = PowerState.NONE;

    // Start is called before the first frame update
    void Start()
    {
        autoPlayTimer = maxPlayTimer;

        // Get Rigidbody & plate manager & achivement
        rb = GetComponent<Rigidbody>();
        pm = FindObjectOfType<PlateManager>();
        ac = FindObjectOfType<Achievement>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleAutoPlay();

        //State and command texts
        HandleText();

        if (replaying) return; //Disable commands while replaying

        //Inputs and commands
        HandleInput();

        //Player and power up states
        HandleStates();
    }

    void HandleAutoPlay()
    {
        autoPlayTimer -= Time.deltaTime;

        if(autoPlayTimer < 0)
        {
            if (lastCommand != null)
            {
                undoed.Clear();
                lastCommand.Execute(rb);
                UpdateGame(ac.nCoins);
            }
            autoPlayTimer = maxPlayTimer;
        }
        //model.position = Vector3.Lerp(model.position, model.position + GetNextPos(), Time.deltaTime * maxPlayTimer);
    }

    void HandleInput()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKeyDown(KeyCode.A) && lastCommand != cmd_D) // Left action
        {
            undoed.Clear();
            //cmd_A.Execute(rb);
            commands.Push(cmd_A);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_A;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastCommand != cmd_A) // Right action
        {
            undoed.Clear();
            //cmd_D.Execute(rb);
            commands.Push(cmd_D);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_D;
        }
        else if (Input.GetKeyDown(KeyCode.W) && lastCommand != cmd_S) // Up action
        {
            undoed.Clear();
            //cmd_W.Execute(rb);
            commands.Push(cmd_W);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_W;
        }
        else if (Input.GetKeyDown(KeyCode.S) && lastCommand != cmd_W) // Down action
        {
            undoed.Clear();
            //cmd_S.Execute(rb);
            commands.Push(cmd_S);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_S;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && currentState == State.Standing) // Jump action
        {
            currentState = State.Jumping;
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentState == State.Crouching) //Uncrouch
            {
                currentState = State.Standing;
            }
            else if (currentState == State.Standing) //Crouch
            {
                currentState = State.Crouching;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z)) // Undo action
        {
            if (commands.Count != 0)
            {
                undoed.Push(commands.Peek());

                commands.Pop().Undo(rb);

                touchedPlate.Restart();
            }
        }
        else if (Input.GetKeyDown(KeyCode.V)) // Redo action
        {
            if (undoed.Count != 0)
            {
                commands.Push(undoed.Peek());
                undoed.Pop().Execute(rb);
            }
        }
        else if (Input.GetKeyDown(KeyCode.X)) // Replay 
        {
            StartCoroutine(Replay());
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Reset
        {
            Restart();
        }

        IEnumerator Replay()
        {
            while (undoed.Count > 0)
            {
                replaying = true;
                yield return new WaitForSeconds(0.2f);
                commands.Push(undoed.Peek());
                undoed.Pop().Execute(rb);
            }
            replaying = false;
        }

    }

    void HandleText()
    {
        stateText.text = "Current state: " + currentState.ToString();

        // Action history
        text.text = "";
        if (commands.Count != 0)
        {
            foreach (Command command in commands)
            {
                text.text += command.GetName();
            }
        }

        // Undo history
        historyText.text = "";
        if (undoed.Count != 0)
        {
            foreach (Command command in undoed)
            {
                historyText.text += command.GetName();
            }
        }
    }

    void HandleStates()
    {
        powerUpDuration -= Time.deltaTime;
        if (powerUpDuration < 0) currentPS = PowerState.NONE;


        switch (currentState)
        {
            case State.Standing:
                model.localScale = Vector3.one;
                model.localPosition = Vector3.zero;

                break;

            case State.Jumping:
                model.localScale = new Vector3(1, 0.33f, 1);

                break;

            case State.Crouching:
                model.localScale = new Vector3(1, 0.33f, 1);
                model.localPosition = new Vector3(0, -0.7f, 0);

                break;

            default:
                break;
        }

        switch (currentPS)
        {
            case PowerState.NONE:
                model.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
                break;

            case PowerState.PowerUp:
                model.gameObject.GetComponent<MeshRenderer>().material = powerupMat;
                break;

            default: break;
        }
    }

    Vector3 GetNextPos()
    {
        Vector3 result = Vector3.zero;
        
        if(lastCommand == cmd_A) 
        {
            result += new Vector3(-2, 0, 0);
        }
        else if(lastCommand == cmd_S) 
        {
            result += new Vector3(0, 0, -2);
        }
        else if(lastCommand == cmd_D) 
        {
            result += new Vector3(2, 0, 0);
        }
        else if(lastCommand == cmd_W) 
        {
            result += new Vector3(0, 0, 2);
        }

        return result;
    }

    private void Restart()
    {
        lastCommand = null;
        undoed.Clear();
        commands.Clear();
        transform.position = Vector3.zero;
        pm.Restart();
        ac.nCoins = 0;
    }

    public void UpdateGame(int coinAmount)
    {
        pm.UpdateGame(coinAmount +1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Plate>() != null)
        {
            touchedPlate = other.gameObject.GetComponent<Plate>();
            if(touchedPlate != null && touchedPlate.colored) 
            {
                Restart();
            }
            else touchedPlate.ChangeColor();
        }
        else if( other.gameObject.GetComponent<Coin>() != null)
        {
            currentPS = PowerState.PowerUp;
            powerUpDuration = 2;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && currentState != State.Crouching)
        {
            currentState = State.Standing;
        }
        else if(collision.gameObject.tag == "Death")
        {
            Die();
        }
    }

    public void Die()
    {
        Restart();
    }
}