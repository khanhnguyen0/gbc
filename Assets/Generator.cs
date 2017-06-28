using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{

#pragma warning disable 78

    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject block_wood;
    public GameObject block_leaf;
    public GameObject Grass;
    public GameObject Flower;
    public GameObject Player;

    public int minX = -16;
    public int maxX = 16;
    public int minY = -10;
    public int maxY = 10;
    public float grassFreq = 0;
    public float flowerFreq = 0;
    public long seed;
    PerlinNoise noise;

    void Start()
    {
        noise = new PerlinNoise(seed);
        Regenerate();
    }

    private void Regenerate()
    {

        float width = dirtPrefab.transform.lossyScale.x;

        float height = dirtPrefab.transform.lossyScale.y;
     //   Physics.IgnoreCollision(Flower.GetComponent<Collider>(), Player.GetComponent<Collider>());
        for (int i = minX; i < maxX; i++)
        {//columns (x values
            int columnHeight = 2 + noise.getNoise(i - minX, maxY - minY - 2);
            for (int j = minY; j < minY + columnHeight; j++)
            {//rows (y values)
                GameObject block = (j == minY + columnHeight - 1) ? Random.value<flowerFreq?Flower: grassPrefab : dirtPrefab;
                
                Instantiate(block, new Vector2(i * width, j * height), Quaternion.identity);
             
                

            }

        }
    }

    private void createGrass(float i, float j)
    {
        if (grassFreq >Random.value)
        {
            GameObject block = Grass;
            Instantiate(block, new Vector2(i,j), Quaternion.identity);
        }
    }


    private void createFlower(float i, float j)
    {
        if (grassFreq > Random.value)
        {
            GameObject block = Flower;
            Instantiate(block, new Vector2(i, j), Quaternion.identity);
        }
    }
}
