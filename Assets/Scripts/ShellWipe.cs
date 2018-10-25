using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShellWipe : MonoBehaviour {

    public int shellDisplayIndex;
    public Texture2D targetTexture, originalTexture;

    private Color[,] targetTextureColors;
    private Texture2D tmpTexture;
    private int currentPixelRow = 0;
    private int increment = 1;

    private void Awake()
    {
        LoadTextures();
        ResetTextures();
        shellDisplayIndex = transform.GetSiblingIndex();
    }

    private void Update()
    {
        //Check if other shells are currently reloading
        var reloadingStatus = ShellDisplay.isReloading.Where((s, i) => i != shellDisplayIndex).ToList();
        var otherReloading = reloadingStatus.Any(s => s == 1);

        //If no other shells are reloading, start reloading this shell
        if (increment < targetTexture.height && !otherReloading)
        {
            StartCoroutine("DrawPixels");
        }
        else if (increment == targetTexture.height)
        {
            increment = 1;
            currentPixelRow = 0;

            int ammo = TankGun.GetCurrentAmmo();
            TankGun.SetCurrentAmmo(ammo + 1);

            ShellDisplay.isReloading[shellDisplayIndex] = 0;
            TankGun.SetShellAsLoaded(shellDisplayIndex);

            ResetTextures();
            enabled = false;
        }
    }

    IEnumerator DrawPixels()
    {
        ShellDisplay.isReloading[shellDisplayIndex] = 1;
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

    private void LoadTextures()
    {
        targetTextureColors = new Color[targetTexture.width, targetTexture.height];

        for (int y = 0; y < targetTexture.height; y++)
        {
            for (int x = 0; x < targetTexture.width; x++)
            {
                targetTextureColors[x, y] = targetTexture.GetPixel(x, y);
            }
        }
    }

    private void ResetTextures()
    {
        tmpTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int y = 0; y < tmpTexture.height; y++)
        {
            for (int x = 0; x < tmpTexture.width; x++)
            {
                tmpTexture.SetPixel(x, y, originalTexture.GetPixel(x, y));
            }
        }

        gameObject.GetComponent<RawImage>().texture = targetTexture;
    }
}
