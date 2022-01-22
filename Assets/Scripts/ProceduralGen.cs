using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGen : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] int minHeight, maxHeight;
    [SerializeField] int repeatNum;
    [SerializeField] GameObject dirt, grass;

    void Start()
    {
        Generation();
    }

    void Generation()
    {
        int repeatValue = 1;
        for (int x = -80; x < width-5; x++)
        {
         if(repeatValue == 1)
            {
                height = Random.Range(minHeight, maxHeight);
                GenerateFlatPlatform(x);
                repeatValue = repeatNum;
            }
            else
            {
                GenerateFlatPlatform(x);
                repeatValue--;
            }     
        }
    }

    void GenerateFlatPlatform(int x)
    {

        for (int y = -20; y < height; y++)
        {
            spawnObj(dirt, x, y);
        }
        spawnObj(grass, x, height);
              
    }

    void spawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
