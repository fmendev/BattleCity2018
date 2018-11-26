using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShellWipe : MonoBehaviour {

    public int shellDisplayIndex;
    public Texture2D targetTexture, originalTexture;

    private Color[,] targetTextureColors;
    private Texture2D tmpTexture;

    private bool isCurrentlyLoading = false;
    private float reloadingTimeFactor = .8f;

    private void Awake()
    {
        LoadTextures();
        ResetTextures();
        shellDisplayIndex = transform.GetSiblingIndex();
    }

    private void Update()
    {
        //Check if other shells are currently reloading
        var otherReloadingStatus = ShellDisplay.isReloading.Where((s, i) => i != shellDisplayIndex).ToList();
        var isOtherReloading = otherReloadingStatus.Any(s => s == 1);

        //If no other shells are reloading, start reloading this shell
        if (!isOtherReloading && !isCurrentlyLoading)
        {
            StartCoroutine("DrawPixels");
        }
    }

    IEnumerator DrawPixels()
    {
        int currentPixelRow = 0;
        ShellDisplay.isReloading[shellDisplayIndex] = 1; //this line is so that other shells now this one is reloading
        isCurrentlyLoading = true; //this is one is so that the coroutine just runs once

        //filling a part of the temporary texture with the target texture
        while (currentPixelRow < targetTexture.height)
        {
            //Debug.Log("currentPixel: " + currentPixelRow);
            for (int y = currentPixelRow; y < currentPixelRow + 1; y++)
            {
                for (int x = 0; x < tmpTexture.width; x++)
                {
                    tmpTexture.SetPixel(x, y, targetTextureColors[x, y]);
                }

                tmpTexture.Apply();
                gameObject.GetComponent<RawImage>().texture = tmpTexture;
            }
            currentPixelRow++;
            yield return new WaitForSeconds(Time.deltaTime * reloadingTimeFactor);
        }

        isCurrentlyLoading = false;

        int ammo = WeaponsController.GetCurrentAmmo();
        WeaponsController.SetCurrentAmmo(ammo + 1);

        ShellDisplay.isReloading[shellDisplayIndex] = 0;
        WeaponsController.SetShellAsLoaded(shellDisplayIndex);

        ResetTextures();
        enabled = false;
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
