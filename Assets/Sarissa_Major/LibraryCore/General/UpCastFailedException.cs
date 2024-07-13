using System;
using UnityEngine;

public class UpCastFailedException : Exception
{
    public UpCastFailedException()
    {
    }

    public UpCastFailedException(string message) : base(message)
    {
    }

    public UpCastFailedException(string message, Exception inner) : base(message, inner)
    {
    }
}
