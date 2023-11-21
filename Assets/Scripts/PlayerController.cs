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
    public TextMeshProUGUI text;
    public TextMeshProUGUI historyText;
    bool replaying;
    Plate touchedPlate;

    // Commands
    Command cmd_W = new MoveForwardCommand();
    Command cmd_S = new MoveBackCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_D = new MoveRightCommand();

    Command lastCommand;

    // Stacks
    Stack<Command> commands = new Stack<Command>();
    Stack<Command> undoed = new Stack<Command>();

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody & plate manager & achivement
        rb = GetComponent<Rigidbody>();
        pm = FindObjectOfType<PlateManager>();
        ac = FindObjectOfType<Achievement>();
    }

    // Update is called once per frame
    private void Update()
    {
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

        if (replaying) return;
        // Wait time for jump
        waitTime -= Time.deltaTime;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKeyDown(KeyCode.A) && lastCommand != cmd_D) // Left action
        {
            undoed.Clear();
            cmd_A.Execute(rb);
            commands.Push(cmd_A);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_A;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastCommand != cmd_A) // Right action
        {
            undoed.Clear();
            cmd_D.Execute(rb);
            commands.Push(cmd_D);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_D;
        }
        else if (Input.GetKeyDown(KeyCode.W) && lastCommand != cmd_S) // Up action
        {
            undoed.Clear();
            cmd_W.Execute(rb);
            commands.Push(cmd_W);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_W;
        }
        else if (Input.GetKeyDown(KeyCode.S) && lastCommand != cmd_W) // Down action
        {
            undoed.Clear();
            cmd_S.Execute(rb);
            commands.Push(cmd_S);
            UpdateGame(ac.nCoins);
            lastCommand = cmd_S;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && waitTime < 0) // Jump action
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            waitTime = 1;
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
        else if( Input.GetKeyDown(KeyCode.V)) // Redo action
        {
            if (undoed.Count != 0)
            {
                commands.Push(undoed.Peek());
                undoed.Pop().Execute(rb);
            }
        }
        else if ( Input.GetKeyDown (KeyCode.X)) // Replay 
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

    private void Restart()
    {
        undoed.Clear();
        commands.Clear();
        transform.position = Vector3.zero;
        pm.Restart();
        ac.nCoins = 0;
    }

    public void UpdateGame(int coinAmount)
    {
        pm.UpdateGame(coinAmount);
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
            Destroy(other.gameObject);
        }
    }

    public void Die()
    {
        Restart();
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i< 25; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(-25 + i + i, -2, -25), new Vector3(-25 + i + i, -2, 25));
            Gizmos.DrawLine(new Vector3(-25, -2, -25 + i +i), new Vector3(25, -2, -25 + i + i));
        }
    }
}