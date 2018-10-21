using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShellWipe : MonoBehaviour {

    public GameObject currentShell;
    public Texture2D targetTexture, originalTexture;

    private Color[,] targetTextureColors;
    private Texture2D tmpTexture;
    private int currentPixelRow = 0;
    private int increment = 1;

    private void Awake()
    {
        ResetTextures();
    }

    private void Update()
    {
        if (increment < targetTexture.height)
            StartCoroutine("DrawPixels");
        else
        {
            increment = 1;
            currentPixelRow = 0;
            GameObject.FindGameObjectWithTag("Player").GetComponent<TankGun>().shellAmmo++;
            Debug.Log("Loaded " + GameObject.FindGameObjectWithTag("Player").GetComponent<TankGun>().shellAmmo);
            ResetTextures();
            enabled = false;
        }
    }

    IEnumerator DrawPixels()
    {
        increment = currentPixelRow + 1;

        //filling a part of the temporary texture with the target texture 
        for (int y = currentPixelRow; y < increment; y++)
        {
            for (int x = 0; x < tmpTexture.width; x++)
            {
                tmpTexture.SetPixel(x, y, targetTextureColors[x, y]);
            }

            tmpTexture.Apply();
        }

        currentPixelRow++;
        gameObject.GetComponent<RawImage>().texture = tmpTexture;

        yield return null;
    }

    private void ResetTextures()
    {
        targetTextureColors = new Color[targetTexture.width, targetTexture.height];

        tmpTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int y = 0; y < tmpTexture.height; y++)
        {
            for (int x = 0; x < tmpTexture.width; x++)
            {
                tmpTexture.SetPixel(x, y, originalTexture.GetPixel(x, y));
            }
        }

        for (int y = 0; y < targetTexture.height; y++)
        {
            for (int x = 0; x < targetTexture.width; x++)
            {
                targetTextureColors[x, y] = targetTexture.GetPixel(x, y);
            }
        }
    }
}
