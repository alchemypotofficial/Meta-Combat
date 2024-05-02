using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Animation[] animations;
    
    private Animation currentAnimation;

    private bool loop;
    private float frameTime;
    private int frame;

    private void Update()
    {
        AdvanceFrame();
    }

    private void AdvanceFrame()
    {
        if (currentAnimation != null)
        {
            float timing = currentAnimation.timings[frame];

            if (Time.time - frameTime >= timing)
            {
                frame++;

                if (frame >= currentAnimation.sprites.Length)
                {
                    if (loop)
                    {
                        frame = 0;
                        spriteRenderer.sprite = currentAnimation.sprites[frame];
                    }
                    else
                    {
                        frameTime = Time.time;
                        frame = 0;
                        currentAnimation = null;

                        return;
                    }
                }

                frameTime = Time.time;
                spriteRenderer.sprite = currentAnimation.sprites[frame];
            }
        }
    }

    public void StartAnimation(string name, bool loop = false)
    {
        foreach (Animation animation in animations)
        {
            if (animation.name == name)
            {
                this.loop = loop;
                frameTime = Time.time;
                frame = 0;
                currentAnimation = animation;

                spriteRenderer.sprite = currentAnimation.sprites[frame];
            }
        }
    }

    public void StopAnimation()
    {
        loop = false;
        frameTime = 0;
        frame = 0;

        currentAnimation = null;
    }

    public bool IsAnimationPlaying()
    {
        return currentAnimation != null;
    }

    public int GetFrame()
    {
        return frame;
    }

    public string GetCurrentAnimation()
    {
        if (currentAnimation != null)
        {
            return currentAnimation.name;
        }

        return null;
    }
}
