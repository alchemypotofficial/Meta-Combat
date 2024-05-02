using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIAnimationController : MonoBehaviour
{
    [SerializeField] private Image image;
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
                        image.sprite = currentAnimation.sprites[frame];
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
                image.sprite = currentAnimation.sprites[frame];
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

                image.sprite = currentAnimation.sprites[frame];
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
}
