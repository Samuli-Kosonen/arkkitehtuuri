using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackCommand : Command
{
    string name = " D ";

    public override void Execute(Rigidbody rb)
    {
        rb.transform.position -= 2 * rb.transform.forward;
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position += 2 * rb.transform.forward;
    }

    public override string GetName()
    {
        return name;
    }
}
