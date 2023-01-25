using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : ScriptableObject
{
    public Sprite sprite;
    public int index;

    public int publicValue;
    protected int protectedValue;
    private int privateValue;

    void Start()
    {
        publicValue = 0;
        protectedValue = 0;
        privateValue = 0;
    }
}

public class Other
{
    Test test;

    void Start()
    {
        test.publicValue = 0;
        //test.protectedValue = 0;
        //test.privateValue = 0;
    }
}
public class Child : Test
{
    void Start()
    {
        publicValue = 0;
        protectedValue = 0;
        // privateVlaue = 0; (X)
    }
}
