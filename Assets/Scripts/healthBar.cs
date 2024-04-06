using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    private Transform bar;
    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
        bar.localScale = new Vector3(1f, 1f);
        Debug.Log(bar);
    }

    public void setSize(float sizeNormalised)
    {
        bar.localScale = new Vector3(sizeNormalised, 1f);
    }

    public void setColor(Color color)
    {
        bar.GetComponentInChildren<SpriteRenderer>().color = color;
    }
}
