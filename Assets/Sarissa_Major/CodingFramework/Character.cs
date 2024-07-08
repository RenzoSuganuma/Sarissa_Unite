using System;
using Sarissa;
using UnityEngine;

public class Character : CodingFramework
{
    protected Single _healthPoint;

    public virtual Single GetHealthPoint()
    {
        return _healthPoint;
    }

    public virtual void SetHealthPoint(Single value)
    {
        _healthPoint = value;
    }
}
