using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public AudioClip gemAC;
    public Rigidbody2D rb;
    void Start()
    {
        rb.freezeRotation = false;
    }

    public void Throw(bool isThrowLeft)
    {
        int direction = isThrowLeft ? 300 : -300;

        SoundManager.Instance.Play(gemAC, SoundManager.SoundPriority.IMPORTANT);
        rb.AddForce(new Vector2(direction, 800));
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
