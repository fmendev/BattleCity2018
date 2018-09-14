using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTileMapping : MonoBehaviour
{
    public static Dictionary<Vector3Int, int> tiles_x;
    public static Dictionary<Vector3Int, int> tiles_y;

    private Tilemap brick;

    private void Awake()
    {
        tiles_x = new Dictionary<Vector3Int, int>();
        tiles_y = new Dictionary<Vector3Int, int>();

        brick = GameObject.FindGameObjectWithTag("Brick").GetComponent<Tilemap>();
    }

    void Start ()
    {
        List<int> x_seq = new List<int> { 2, 2, 4, 4 };
        List<int> y_seq = new List<int> { 3, 3, 9, 9 };

        //Grid size coordinates should have no decimals
        int min_x = (int)brick.localBounds.min.x;
        int min_y = (int)brick.localBounds.min.y;
        int max_x = (int)brick.localBounds.max.x;
        int max_y = (int)brick.localBounds.max.y;

        int index_x = 0;
        int index_y = 0;

        Vector3 currentLocation = Vector3.zero;

        //Iterate through each row of tiles (L->R, B->T) and mark pairs of tiles that destruct together when hit
        for (int y = min_y; y < max_y + 1; y++)
        {
            for (int x = min_x; x < max_x + 1; x++)
            {
                currentLocation.y = y + .2f;
                currentLocation.x = x + .2f;

                Vector3Int tile = brick.WorldToCell(currentLocation);

                if (brick.GetTile(tile) != null)
                {
                    tiles_x.Add(tile, x_seq[index_x % x_seq.Count]);
                    index_x++;
                }
                else
                {
                    index_x = 0;
                }
            }
        }

        //Iterate through each column of tiles (B->T, L->R) and mark pairs of tiles that destruct together when hit
        for (int x = min_x; x < max_x + 1; x++)
        {
            for (int y = min_y; y < max_y + 1; y++)
            {
                currentLocation.y = y + .2f;
                currentLocation.x = x + .2f;

                Vector3Int tile = brick.WorldToCell(currentLocation);

                if (brick.GetTile(tile) != null)
                {
                    tiles_y.Add(tile, y_seq[index_y % y_seq.Count]);
                    index_y++;
                }
                else
                {
                    index_y = 0;
                }
            }
        }
    }

}
