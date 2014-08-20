/*
 * by Kacper Domański
 * kqpa93@gmail.com
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour{

    public GameObject wall;
    public GameObject ground;
    public GameObject door;
    public GameObject corner;

    public Color color;
    public int width;
    public int length;
    private Vector2 position;
    private List<GameObject> floor = new List<GameObject>();

    public void create(int w, int l, Vector2 pos)
    {
        width = w;
        length = l;
        transform.position = new Vector3(pos.x, 0, pos.y);

        //Temporary floor objects
        for (int i = 0; i < w+1; i++)
        {
            for (int j = 0; j < l+1; j++)
            {
                if (j == 0 || j == l || i == 0 || i == w)
                {
                    GameObject wal = Instantiate(wall) as GameObject;
                    wal.transform.position = this.transform.position - new Vector3(w / 2.0f - i , -1, l / 2.0f - j);
                    wal.transform.parent = this.transform;
                    wal.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
        //Temporary wall objects
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < l; j++)
            {
                GameObject f = Instantiate(ground) as GameObject;
                f.transform.position = this.transform.position - new Vector3(w / 2.0f - i - 0.5f, 0, l / 2.0f - j - 0.5f);
                f.transform.parent = this.transform;
                floor.Add(f);
                if (w >= 5 && l >= 5)
                {
                    f.GetComponent<MeshRenderer>().material.color = color;
                }
            
            }
        }
    }

}
