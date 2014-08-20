/*
 * by Kacper Domański
 * kqpa93@gmail.com
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaveGenerator : MonoBehaviour {

    public GameObject roomTemplate;

    private List<GameObject> cells = new List<GameObject>(); 
    private List<GameObject> rooms = new List<GameObject>();
    private int count = 150;
    private int c= 0;
    private float lastTime;
    private bool moving = false;

    const int RANDOM_MAX = 100;
    const int RANGE = 30;

	// Use this for initialization
	void Start () {
        lastTime = Time.time;
        int w,l;
        Vector2 pos;

        for (int i = 0; i < count; i++)
        {
            w = ((int)Mathf.Abs(generateGaussianNoise())+1)*2 + 1;
            l = ((int)Mathf.Abs(generateGaussianNoise())+1)*2 + 1;
            pos = new Vector2((int)(generateGaussianNoise() * RANGE / 3), (int)(generateGaussianNoise() * RANGE/3)); 
            while (Vector2.Distance(pos, new Vector2(0, 0)) > RANGE)
                pos = new Vector2((int)(generateGaussianNoise() * RANGE / 3), (int)(generateGaussianNoise() * RANGE / 3));

            GameObject cell = Instantiate(roomTemplate) as GameObject;
            cell.GetComponent<Room>().create(w, l, pos);
            cell.transform.parent = this.transform;
            cell.name = "cell nr " + i;
            cells.Add(cell);

            //randomize cells (using gauss)
            //each big (5x5 or bigger) cell will be later used as room
            if (w >= 5 && l >= 5)
            {
                rooms.Add(cell);
            }
        }
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 v = new Vector3(0,0,0);
        int neighbours = 0;

        if (!moving)
        {
            if (Time.time - lastTime > 0.3)
            {
                lastTime = Time.time;

                c++;
                //simple steering behaviour temporary here - rendering every step :)
                foreach (GameObject myCell in cells)
                {
                    neighbours = 0;
                    v = new Vector3(0, 0, 0);
                    foreach (GameObject cell in cells)
                    {
                        if (myCell != cell)
                        {
                            //collecting data about neighbours
                            if (doCellsOverlapse(myCell, cell))
                            {
                                neighbours++;
                                v += myCell.transform.position - cell.transform.position;
                            }
                        }
                    }


                    if (neighbours > 0)
                    {
                        //chosing 'strongest' side
                        if (Mathf.Abs(v.x) > Mathf.Abs(v.z))
                        {
                            myCell.transform.position += new Vector3(Mathf.Sign(v.x), 0, 0);
                        }
                        else
                        {
                            myCell.transform.position += new Vector3(0, 0, Mathf.Sign(v.z));
                        }
                    }
                }
                Debug.Log(c);
            }

            if (!doCellsOverlapse())
            {
                moving = false;
            }
        }
        else
        {
            //TODO Delaunay triangulation and min spanning tree
        }
	}

    //Box Mueller Gaussian Noise
    float generateGaussianNoise()
    {
        float rand1, rand2;

        rand1 = (float)Random.Range(1, RANDOM_MAX) / RANDOM_MAX;
        //if (rand1 < 1e-100) rand1 = (float) 1e-100;
        rand1 = -2 * Mathf.Log(rand1);
        rand2 = (float)Random.Range(0, RANDOM_MAX) / RANDOM_MAX * 2 * Mathf.PI;

        return Mathf.Sqrt(rand1) * Mathf.Cos(rand2);
    }

    //Check if 2 cells overlapse
    bool doCellsOverlapse(GameObject cell1, GameObject cell2)
    {
        float w1 = cell1.GetComponent<Room>().width / 2.0f;
        float w2 = cell2.GetComponent<Room>().width / 2.0f;

        float l1 = cell1.GetComponent<Room>().length / 2.0f;
        float l2 = cell2.GetComponent<Room>().length / 2.0f;

        if (Mathf.Abs(cell1.transform.position.x - cell2.transform.position.x) < w1 + w2 && Mathf.Abs(cell1.transform.position.z - cell2.transform.position.z) < l1 + l2)
        {
            return true;
        }
        return false;
    }

    //Check if any 2 cells overlapse
    bool doCellsOverlapse()
    {
        foreach (GameObject myCell in cells)
        {
                foreach (GameObject cell in cells)
                {
                    if (myCell != cell)
                    {
                        if(doCellsOverlapse(cell, myCell)){
                            return true;
                        }
                    }
                }
        }
        return false;
    }
}
