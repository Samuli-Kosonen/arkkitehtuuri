using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected float speed = 1f;
    //Execute must be implemented by each child-class
    public abstract void Execute(Rigidbody rb);

    public abstract void Undo(Rigidbody rb);

    public abstract string GetName();
}