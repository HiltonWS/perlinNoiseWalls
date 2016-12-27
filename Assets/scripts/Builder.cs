using UnityEngine;

public class Builder : MonoBehaviour
{
    //Setado no unity
    public GameObject[] objects;
    //Altura do chunk atual
    public int chunkHeight = 16;
    //largura do chunk atual
    public int chunkWidth = 16;
    //Espaço entre os blocos no caso cada bloco como são 1x1x1 terão 1-2 de espaço assim ficando um do lado do outro
    public float space = 2f;
    //Cria um mapa para atribuir os blocos
    private int[,] mapGrid;
    public float definedSeed = 0f;
    private float seed;

    void Start()
    {
        mapGrid = new int[chunkWidth, chunkHeight];
        createGrid();
        buildWall();
        build();
    }

    private void createGrid()
    {
        //Gera um random ou utiliza a seed
        seed = definedSeed == 0 ? Random.Range(0, 100) : definedSeed;
        //Percore o mapa gerando o noise
        for (int h = 0; h < chunkHeight; h++)
        {
            for (int w = 0; w < chunkWidth; w++)
            {
                mapGrid[w, h] = defaultNoise(w, h);
            }
        }
    }

    //Pega uma posicao do perlin
    private int defaultNoise(int x, int y)
    {
        return (int)(Mathf.PerlinNoise(x / space + seed, y / space + seed) * 10);
    }

    private void buildWall()
    {
        int x = 0;

        //Substitui por -1 onde serão as paredess constroi o X _ e y |   
        for (int n = 0; n < chunkHeight; n++)
        {
            for (int h = 0; h < chunkHeight; h++)
            {
                mapGrid[x, h] = -1;

            }
            //Utiliza a seed do perlin ou um randomico conforme setado em  createGrid
            x += defaultNoise(n, n);

            print(x);

            if (x >= chunkWidth)
            {
                break;
            }
        }
        int y = 0;
        for (int n = 0; n < chunkWidth; n++)
        {
            for (int w = 0; w < chunkWidth; w++)
            {
                mapGrid[w, y] = -1;

            }
            //Utiliza a seed do perlin ou um randomico conforme setado em  createGrid
            y += defaultNoise(n, n);

            if (y >= chunkHeight)
            {
                break;
            }
        }


    }

    private void build()
    {
        //Percorre o mapa e constroi os quartos
        for (int h = 0; h < chunkHeight; h++)
        {
            for (int w = 0; w < chunkWidth; w++)
            {

                Vector3 buildPosition = new Vector3(w * space, 0, h * space);
                int result = mapGrid[w, h];

                if (result == -1)
                {
                    //Paredes
                    Instantiate(objects[0], buildPosition, Quaternion.identity);

                }
                else
                {
                    //Chão
                    Instantiate(objects[1], buildPosition, Quaternion.identity);

                }
            }
        }
    }


}
