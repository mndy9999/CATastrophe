using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool isBoss;
    public Vector2 gridPos;
    public ENVIRONMENT ENVIRONMENT;
    public Tile(Vector2 gridPos)
    {
        this.gridPos = gridPos;
    }
}
public enum ENVIRONMENT { JUNGLE, DESERT, CAVE}
public class LevelGenerator : MonoBehaviour
{
    Vector2 WorldSize = new Vector2(4, 4);
    int gridSizeX = 20, gridSizeZ = 20, NUMBER_OF_TILES = 20;
    public static float TILE_OFFSET = 22.5f;
    Tile[,] tiles;
    List<Vector2> takenPositions = new List<Vector2>();
    public GameObject Environment;
    public GameObject Caveman_Melee;
    public GameObject Caveman_Ranged;
    public Tile BossTile;
    public GameObject BossTrigger;
    private Camera cam;
    public GameObject plr;
    public GameObject grenadePowerup;
    public GameObject healthPowerup;
    public GameObject meteorite;

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.left))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.up))
            ret++;
        if (usedPositions.Contains(checkingPos + Vector2.down))
            ret++;
        return ret;
    }
    void GenerateBoss()
    {
       
    }
    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        
        
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeZ || y < -gridSizeZ);
        return checkingPos;
    }
    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeZ || y < -gridSizeZ);
        return checkingPos;
    }
    void CreateRooms()
    {
        tiles = new Tile[(gridSizeX * 2), (gridSizeZ * 2)];
        tiles[gridSizeX, gridSizeZ] = new Tile(Vector2.zero);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
        for(int i = 0; i < NUMBER_OF_TILES - 1; i++)
        {
            float randomPerc = ((float) i / ((float)NUMBER_OF_TILES - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
            checkPos = NewPosition();

            if(NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
            }
            tiles[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeZ] = new Tile(checkPos);
            if (BossTile == null)
                BossTile = tiles[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeZ];
            else
            {
                if (Mathf.Abs(BossTile.gridPos.x) + Mathf.Abs(BossTile.gridPos.y) <
                    Mathf.Abs(tiles[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeZ].gridPos.x) +
                    Mathf.Abs(tiles[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeZ].gridPos.y))
                {
                    BossTile = tiles[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeZ];
                } 
            }
            takenPositions.Insert(0, checkPos);
        }
    }
    void SetRoomPaths(ENVIRONMENT env)
    {
        if(env == ENVIRONMENT.JUNGLE || env == ENVIRONMENT.DESERT)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                    {
                        continue;
                    }
                    if (z - 1 < 0)
                    {
                        tiles[x, z].down = false;
                    }
                    else
                    {
                        tiles[x, z].down = (tiles[x + 1, z] != null);
                    }
                    if (z + 1 >= gridSizeZ * 2)
                    {
                        tiles[x, z].up = false;
                    }
                    else
                    {
                        tiles[x, z].up = (tiles[x - 1, z] != null);
                    }

                    if (x - 1 < 0)
                    {
                        tiles[x, z].left = false;
                    }
                    else
                    {
                        tiles[x, z].left = (tiles[x, z - 1] != null);
                    }
                    if (x + 1 >= gridSizeX * 2)
                    {
                        tiles[x, z].right = false;
                    }
                    else
                    {
                        tiles[x, z].right = (tiles[x, z + 1] != null);
                    }
                }
            }
        } else if(env == ENVIRONMENT.CAVE)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                    {
                        continue;
                    }
                    if (z - 1 < 0)
                    {
                        tiles[x, z].down = false;
                    }
                    else
                    {
                        tiles[x, z].down = (tiles[x - 1, z] != null);
                    }
                    if (z + 1 >= gridSizeZ * 2)
                    {
                        tiles[x, z].up = false;
                    }
                    else
                    {
                        tiles[x, z].up = (tiles[x + 1, z] != null);
                    }

                    if (x - 1 < 0)
                    {
                        tiles[x, z].left = false;
                    }
                    else
                    {
                        tiles[x, z].left = (tiles[x, z + 1] != null);
                    }
                    if (x + 1 >= gridSizeX * 2)
                    {
                        tiles[x, z].right = false;
                    }
                    else
                    {
                        tiles[x, z].right = (tiles[x, z - 1] != null);
                    }
                }
            }
        }
        else if (env == ENVIRONMENT.DESERT)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                    {
                        continue;
                    }
                    if (z - 1 < 0)
                    {
                        tiles[x, z].down = false;
                    }
                    else
                    {
                        tiles[x, z].down = (tiles[x, z + 1] != null);
                    }
                    if (z + 1 >= gridSizeZ * 2)
                    {
                        tiles[x, z].up = false;
                    }
                    else
                    {
                        tiles[x, z].up = (tiles[x, z + 1] != null);
                    }

                    if (x - 1 < 0)
                    {
                        tiles[x, z].left = false;
                    }
                    else
                    {
                        tiles[x, z].left = (tiles[x - 1, z] != null);
                    }
                    if (x + 1 >= gridSizeX * 2)
                    {
                        tiles[x, z].right = false;
                    }
                    else
                    {
                        tiles[x, z].right = (tiles[x + 1, z] != null);
                    }
                }
            }
        }

    }
    void DrawTiles(ENVIRONMENT env)
    {
        if (env == ENVIRONMENT.JUNGLE)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                        continue;

                    string layout = $"{(tiles[x, z].left == true ? 1 : 0)}{(tiles[x, z].right == true ? 1 : 0)}{(tiles[x, z].up == true ? 1 : 0)}{(tiles[x, z].down == true ? 1 : 0)}";
                    GameObject g = Instantiate(Resources.Load($"JungleTiles/{layout}"), transform) as GameObject;
                    g.transform.position = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                }
            }
        }
        else if (env == ENVIRONMENT.CAVE)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                        continue;

                    string layout = $"{(tiles[x, z].left == true ? 1 : 0)}{(tiles[x, z].right == true ? 1 : 0)}{(tiles[x, z].up == true ? 1 : 0)}{(tiles[x, z].down == true ? 1 : 0)}";

                    GameObject g = Instantiate(Resources.Load($"CaveTiles/{layout}"), transform) as GameObject;
                    g.transform.position = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                }
            }
        }
        else if(env == ENVIRONMENT.DESERT)
        {
            for (int x = 0; x < gridSizeX * 2; x++)
            {
                for (int z = 0; z < gridSizeZ * 2; z++)
                {
                    if (tiles[x, z] == null)
                        continue;

                    string layout = $"{(tiles[x, z].left == true ? 1 : 0)}{(tiles[x, z].right == true ? 1 : 0)}{(tiles[x, z].up == true ? 1 : 0)}{(tiles[x, z].down == true ? 1 : 0)}";
                    Debug.Log(layout.ToString());
                    GameObject g = Instantiate(Resources.Load($"DesertTiles/{layout}"), transform) as GameObject;
                    g.transform.position = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                }
            }
        }
    }
    Vector3 GenerateSpawnPosition(Vector3 tilePos)
    {
        float xPos = tilePos.x + (Random.value * 3);
        float zPos = tilePos.z + (Random.value * 3);
        Vector3 spawnPos = new Vector3(xPos, 2f, zPos);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, -transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
                spawnPos = GenerateSpawnPosition(tilePos);
        }
        return spawnPos;
    }
    void GenerateEnemies(ENVIRONMENT env)
    { 
        for(int x = 0; x < gridSizeX * 2; x++)
        {
            for(int z = 0; z < gridSizeZ * 2; z++)
            {
                if (tiles[x, z] == null) //Don't spawn an empty tile
                    continue;
                if (tiles[x, z].gridPos == BossTile.gridPos) //Don't spawn on a boss tile
                    continue;
                Vector3 tilePos = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                if (env == ENVIRONMENT.JUNGLE || env == ENVIRONMENT.DESERT)
                {
                    int toSpawn = Mathf.RoundToInt(Random.value * 4);
                    for (int i = 0; i < toSpawn; i++)
                    {
                        Vector3 spawnPos = GenerateSpawnPosition(tilePos);
                        bool isRanged = (Random.value > .5f);
                        if (isRanged)
                            GameObject.Instantiate(Caveman_Melee, spawnPos, Quaternion.identity, Environment.transform);
                        else
                            GameObject.Instantiate(Caveman_Ranged, spawnPos, Quaternion.identity, Environment.transform);
                    }
                }
                else if (env == ENVIRONMENT.CAVE)
                {
                    int toSpawn = Mathf.RoundToInt(Random.value * 4);
                    for (int i = 0; i < toSpawn; i++)
                    {
                        Vector3 spawnPos = GenerateSpawnPosition(tilePos);
                        bool isRanged = (Random.value > .5f);
                        if (isRanged)
                            GameObject.Instantiate(Caveman_Melee, spawnPos, Quaternion.identity, Environment.transform);
                        else
                            GameObject.Instantiate(Caveman_Ranged, spawnPos, Quaternion.identity, Environment.transform);
                    }
                }
            }
        }
        
       
        
    }

    void GeneratePowerups()
    {
        for(int x = 0; x < gridSizeX * 2; x++)
        {
            for(int z = 0; z < gridSizeZ * 2; z++)
            {
                if (tiles[x, z] == null)
                    continue;
                if (tiles[x, z].gridPos == BossTile.gridPos)
                    continue;
                if (Random.value > 0.7f) //Spawn Grenade
                {
                    Vector3 tilePos = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                    float xPos = tilePos.x + (Random.value * 3);
                    float zPos = tilePos.z + (Random.value * 3);
                    GameObject.Instantiate(grenadePowerup, new Vector3(xPos, 2, zPos), Quaternion.identity, transform);
                }
                if (Random.value > 0.7f) //Spawn Health
                {
                    Vector3 tilePos = new Vector3(tiles[x, z].gridPos.x * TILE_OFFSET, 0, tiles[x, z].gridPos.y * TILE_OFFSET);
                    float xPos = tilePos.x + (Random.value * 3);
                    float zPos = tilePos.z + (Random.value * 3);
                    GameObject.Instantiate(healthPowerup, new Vector3(xPos, 2, zPos), Quaternion.identity, transform);
                }
            }
        }
    }
    void SpawnPlayer(ENVIRONMENT env)
    {
        if (env == ENVIRONMENT.JUNGLE )
            cam.GetComponent<FollowPlayer>().target = Instantiate(plr, new Vector3(0,0.1f,0), Quaternion.identity, transform);
        if (env == ENVIRONMENT.CAVE || env == ENVIRONMENT.DESERT)
        {
            cam.GetComponent<FollowPlayer>().target = Instantiate(plr, new Vector3(0, 0.15f, 0), Quaternion.identity, transform);
        }
            
    }
    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
    }

    public Vector3 BuildMap(ENVIRONMENT env)
    {
        if (NUMBER_OF_TILES >= (WorldSize.x) * (WorldSize.y))
        {
            NUMBER_OF_TILES = Mathf.RoundToInt((WorldSize.x * 2) * (WorldSize.y * 2));
        }
        CreateRooms();
        SetRoomPaths(env);
        DrawTiles(env);
        gameObject.GetComponent<NavMeshBaker>().Build();
        GenerateEnemies(env);
        SpawnPlayer(env);
        GeneratePowerups();
        return new Vector3(BossTile.gridPos.x * TILE_OFFSET, 0, BossTile.gridPos.y * TILE_OFFSET);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearMap()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
