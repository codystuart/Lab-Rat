using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip clickSound;

    public void HoverSound()
    {
        source.PlayOneShot(hoverSound);
    }
    public void ClickSound()
    {
        source.PlayOneShot(clickSound);
    }
}
