using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    string name = " R ";
    public override void Execute(Rigidbody rb)
    {
        rb.transform.position += 2* rb.transform.right;
    }

    public override void Undo(Rigidbody rb)
    {
        rb.transform.position -= 2 * rb.transform.right;
    }

    public override string GetName()
    {
        return name;
    }
}
