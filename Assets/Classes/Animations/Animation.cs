using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation
{
    public string name;
    public Sprite[] sprites;
    public float[] timings;

    public Animation(string name, Sprite[] sprites, float[] timings)
    {
        this.name = name;
        this.sprites = sprites;
        this.timings = timings;
    }

    public float GetLength()
    {
        float length = 0;

        foreach (float timing in timings)
        {
            length += timing;
        }

        return length;
    }
}
