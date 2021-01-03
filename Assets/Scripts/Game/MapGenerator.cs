using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MapGenerator : MonoBehaviour
{

    public SpriteAtlas atlasCity;
    public GameObject prefab;
    public GameObject carPrefab;

    public byte sizeMapX = 9;   
    public byte sizeMapY = 9;
    public byte countMap = 3;
    public Vector3 startPosition;
    public float sizeTitles;
    public float deleteMapPositionY;
    public float mapSpeed = 3;

    private string roadLine = "roadCity_1";
    private string roadCorner = "roadCity_3";
    private string roadTriangle = "roadCity_0";
    private string roadQuadrangle = "roadCity_2";

    List<Map> maps; // лист с картами sizeX на sizeY где зранятся нижний план города

    struct Location
    {
        public int x, y;
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator == (Location firs, Location second)
        {
            if (firs.x == second.x && firs.y == second.y) return true;
            return false;
        }
        public static bool operator !=(Location firs, Location second)
        {
            if (firs.x != second.x || firs.y != second.y) return true;
            return false;
        }
    }

    enum TypeOfCell
    {
        Road = 1,
        SecondRoad,
        Anather
    }

    void Start()
    {
        maps = new List<Map>();
        // первоначально инициализируем карты блоков
        StartCoroutine(StartInitialization());
    }


    void Update()
    {
        if (Game.isGamePause) return;

        for (int i = maps.Count - 1; i >= 0; i--)
        {
            Vector3 movePosition = maps[i].GetPosition();
            movePosition.y -= (mapSpeed + Mathf.Abs(Game.GameSpeed / 1.5f - mapSpeed)) * Time.deltaTime;
            maps[i].SetPosition(movePosition);

            if (maps[i].GetPosition().y <= deleteMapPositionY)
            {
                Map tempMap = maps[i];
                maps.RemoveAt(i);

                Vector3 newPosition = maps[maps.Count - 1].GetPosition();
                newPosition.y += sizeMapY * sizeTitles;
                tempMap.CleanAll(newPosition);
                maps.Add(tempMap);
                StartCoroutine(GenerationAndIntializationMap());
            }
        }
    }

    IEnumerator GenerationAndIntializationMap()
    {
        yield return StartCoroutine(GenerateMap(maps[maps.Count - 1], maps[maps.Count - 2].endLocationRoad));
        yield return new WaitForSeconds(0.01f);
        yield return StartCoroutine(InitializationMap(maps[maps.Count - 1]));
    }


    IEnumerator StartInitialization()
    {
        for (byte i = 0; i < countMap; i++)
        {
            if (i == 0)
            {
                maps.Add(new Map(this.gameObject, sizeMapX, sizeMapY, prefab, sizeTitles));
                maps[maps.Count - 1].SetPosition(startPosition);
            }
            else
            {
                Vector3 pos = maps[maps.Count - 1].GetPosition();
                pos.y += sizeMapY * sizeTitles;
                maps.Add(new Map(this.gameObject, sizeMapX, sizeMapY, prefab, sizeTitles));
                maps[maps.Count - 1].SetPosition(pos);
            }

            Coroutine generator;
            if (maps.Count <= 1) generator = StartCoroutine(GenerateMap(maps[maps.Count - 1], new Location(Random.Range(1, sizeMapX - 2), sizeMapY - 1)));
            else generator = StartCoroutine(GenerateMap(maps[maps.Count - 1], maps[maps.Count - 2].endLocationRoad));
            
            yield return generator;
            yield return new WaitForSeconds(0.05f);
            yield return StartCoroutine(InitializationMap(maps[maps.Count - 1]));
        }

    }

    IEnumerator GenerateMap(Map map, Location? endTitleLocation)
    {
        // начало создание главной дороги которая может периходить из карты в карту 
        bool isFinish = false;
        Location vectorRoad;
        Vector2 lastDiraction;

        if (endTitleLocation == null) // если у нас на прошлой карте не было перехода дороги на новую карту 
        {
            if (Random.value >= 0.5)
            {
                map.cells[sizeMapX - 1, 0] = TypeOfCell.Road;
                vectorRoad = new Location(sizeMapX - 1, 0);
            }
            else
            {
                map.cells[(0), 0] = TypeOfCell.Road;
                vectorRoad = new Location(0, 0);
            }
            lastDiraction = Vector2.zero;
        }
        else
        {
            map.cells[endTitleLocation.Value.x, 0] = TypeOfCell.Road;
            vectorRoad = new Location(endTitleLocation.Value.x, 0);
            lastDiraction = Vector2.left;
        }

        // зацикливаем и случайно герерируем дорогу
        while (!isFinish)
        {
            if (lastDiraction == Vector2.left || lastDiraction == Vector2.right)
            {
                lastDiraction = Vector2.up;
            }
            else if (lastDiraction == Vector2.up)
            {
                float rand = Random.value;
                if (rand < 0.45) lastDiraction = Vector2.up;
                else if (rand < 0.75) lastDiraction = Vector2.left;
                else lastDiraction = Vector2.right;

            }
            else if (lastDiraction == Vector2.zero)
            {
                if (vectorRoad.x == 0) lastDiraction = Vector2.right;
                else lastDiraction = Vector2.left;
            }

            int duration = Random.Range(2, 4);

            bool isWentAbroad = false;

            while (duration != 0)
            {
                if (lastDiraction == Vector2.left)
                {
                    if ((vectorRoad.x - 1) >= 0)
                    {
                        map.cells[(vectorRoad.x - 1), (vectorRoad.y)] = TypeOfCell.Road;
                        vectorRoad.x--;
                        if (vectorRoad.x == 0) isWentAbroad = true;
                    }
                    else { isWentAbroad = true; break; }
                }
                else if (lastDiraction == Vector2.up)
                {
                    if ((vectorRoad.y + 1) < sizeMapY)
                    {
                        map.cells[(vectorRoad.x), (vectorRoad.y + 1)] = TypeOfCell.Road;
                        vectorRoad.y++;
                    }
                    else
                    {
                        isFinish = true;
                        map.endLocationRoad = vectorRoad;
                        break;
                    }
                }
                else if (lastDiraction == Vector2.right)
                {
                    if ((vectorRoad.x + 1) < sizeMapX)
                    {
                        map.cells[(vectorRoad.x + 1), (vectorRoad.y)] = TypeOfCell.Road;
                        vectorRoad.x++;
                        if (vectorRoad.x == (sizeMapX - 1)) isWentAbroad = true;
                    }
                    else { isWentAbroad = true; break; }
                }

                duration--;
            }

            if (isWentAbroad)
            {
                if ((vectorRoad.y + 2) < sizeMapY - 2)
                {
                    if (Random.value >= 0.5)
                    {
                        map.cells[(sizeMapX - 1), (vectorRoad.y + 2)] = TypeOfCell.Road;
                        vectorRoad.x = (sizeMapX - 1);
                        vectorRoad.y += 2;
                    }
                    else
                    {
                        map.cells[(0), (vectorRoad.y + 2)] = TypeOfCell.Road;
                        vectorRoad.x = 0;
                        vectorRoad.y += 2;
                    }
                    lastDiraction = Vector2.zero;
                }
                else
                {
                    isFinish = true;
                    map.endLocationRoad = null;
                }
            }

            if (vectorRoad.y >= sizeMapY - 1)
            {
                isFinish = true;
                map.endLocationRoad = vectorRoad;
            }
            yield return null;
        }
        // generate second road
        short roadLeft = 0;
        short roadRight = 0;

        for (int i = 0; i < sizeMapX; i++)
        {
            for (int j = 0; j < sizeMapY; j++)
            {
                if (map.cells[i, j] == TypeOfCell.Road)
                {
                    if (i < (sizeMapX / 2) + 1)
                    {
                        roadLeft++;
                    }
                    else
                    {
                        roadRight++;
                    }
                }
            }
        }

        float leftKoef = ((float)(roadLeft) / (roadLeft + roadRight));
        byte forExit = 10;

        Location startLocation = new Location(0, 0);
        // счучайно генерируем точку и проверяем возможно ли там поставить дорогу
        while (forExit > 0)
        {
            if (Random.value >= leftKoef)
            {
                //left
                startLocation = new Location(Random.Range(1, (sizeMapX / 2) + 1), Random.Range(1, sizeMapY - 1));
            }
            else
            {
                //right
                startLocation = new Location(Random.Range((sizeMapX / 2) + 1, sizeMapX - 1), Random.Range(1, sizeMapY - 1));
            }

            CellInformation info = new CellInformation(map, startLocation, TypeOfCell.Road);

            if (info.IsAllEmpty() && map.cells[startLocation.x, startLocation.y] != TypeOfCell.Road)
            {
                map.cells[startLocation.x, startLocation.y] = TypeOfCell.SecondRoad;
                break;
            }
            else
            {
                forExit--;
            }
        }

        if (forExit == 0) yield break;  // если у нас не нашло такой точки где свободно то на это карте не будет дополнительной дороги

        Location firstCellSecondRoad = new Location();
        for (int rep = 0; rep < 2; rep++)   // от точки проводим 2 случайно генерации в стороны ( ибо точка центр дороги. А нам нужно начало иконец дороги)
        {
            bool isEndGenerateSecondRoad = false;
            Vector2 roadDirection = Vector2.zero;
            bool isFirstTime = true;
            Location currentLocation = new Location(startLocation.x, startLocation.y);
            Location lastSecondRoad = new Location(currentLocation.x, currentLocation.y);

            if (rep == 1)
            {
                map.cells[startLocation.x, startLocation.y] = TypeOfCell.SecondRoad;
                map.cells[firstCellSecondRoad.x, firstCellSecondRoad.y] = TypeOfCell.SecondRoad;
                lastSecondRoad = firstCellSecondRoad;
            }

      
            while (!isEndGenerateSecondRoad)
            {
                CellInformation info = new CellInformation(map, currentLocation, TypeOfCell.SecondRoad);
                List<Vector2> random = new List<Vector2>();
                int roadDuration = Random.Range(2, 5);

                if (!info.isUp && currentLocation.y < map.cells.GetLength(1) - 3)
                {
                    random.Add(Vector2.up);
                    roadDuration = Random.Range(2, map.cells.GetLength(1) - 2 - currentLocation.y + 1);

                }
                if (!info.isDown && currentLocation.y > 2)
                {
                    random.Add(Vector2.down);
                    roadDuration = Random.Range(2, currentLocation.y);
                }
                if (!info.isRight) random.Add(Vector2.right);
                if (!info.isLeft) random.Add(Vector2.left);


                roadDirection = random[Random.Range(0, random.Count)];

                for (byte i = 0; i < roadDuration; i++)
                {
                    CellInformation cell = new CellInformation(map, currentLocation, TypeOfCell.Road);
                    CellInformation cellSecond = new CellInformation(map, currentLocation, TypeOfCell.SecondRoad);
                    Location nextLoc = new Location(currentLocation.x + (int)roadDirection.x, (int)roadDirection.y + currentLocation.y);

                    if ((currentLocation.y == 1 && roadDirection == Vector2.down) || (currentLocation.y == (map.cells.GetLength(1) - 2) && roadDirection == Vector2.up)) break;

                    if (!cell.GetValueByVector(roadDirection) && !cellSecond.GetValueByVector(roadDirection) && !map.IsOutOfBounds(nextLoc))
                    {
                        if (cell.IsBlockNear(roadDirection))
                        {

                            map.cells[lastSecondRoad.x, lastSecondRoad.y] = TypeOfCell.Road;
                            map.cells[currentLocation.x, currentLocation.y] = TypeOfCell.Road;
                            isEndGenerateSecondRoad = true;
                            break;

                        }
                        if (lastSecondRoad != currentLocation) map.cells[lastSecondRoad.x, lastSecondRoad.y] = TypeOfCell.Road;
                        lastSecondRoad = currentLocation;

                        if (isFirstTime) { isFirstTime = false; firstCellSecondRoad = nextLoc; }

                        map.cells[nextLoc.x, nextLoc.y] = TypeOfCell.SecondRoad;
                        currentLocation = nextLoc;

                    }
                    else
                    {
                        map.cells[lastSecondRoad.x, lastSecondRoad.y] = TypeOfCell.Road;
                        map.cells[currentLocation.x, currentLocation.y] = TypeOfCell.Road;
                        isEndGenerateSecondRoad = true;
                    }
                }
                yield return null;
            }
            yield return null;
        }

    }


    IEnumerator InitializationMap(Map map)
    {
        for (int x = 0; x < map.cells.GetLength(0); x++)
        {
            for (int y = 0; y < map.cells.GetLength(1); y++)
            {
              
                if (map.cells[x, y] == TypeOfCell.Road || map.cells[x,y] == TypeOfCell.SecondRoad)
                {
                    InstantRoad(map, new Location(x, y)); // отображение дороги со всеми поворотами
                }
                else
                {
                    // генерация случайного декора
                    GameObject gameObject1 = map.spriteObjects[x, y];
                    gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite("housesCity_" + Random.Range(0, 30));
                    gameObject1.transform.localPosition = new Vector3(x * sizeTitles, y * sizeTitles, 0);

                    gameObject1.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
                }
            }
            yield return null;
        }
    }

    
    void InstantRoad(Map map, Location location)
    {
        CellInformation info = new CellInformation(map, location, TypeOfCell.Road);
        GameObject gameObject1 = map.spriteObjects[location.x, location.y];
        
        //test
        if (map.cells[location.x,location.y] == TypeOfCell.SecondRoad)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = null;
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        if(info.isUp && info.isDown && info.isLeft && info.isRight)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadQuadrangle);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);
           
        }
        else if(info.isUp && info.isRight && info.isLeft)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadTriangle);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 270);
        }  
        else if(info.isUp && info.isDown && info.isLeft)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadTriangle);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (info.isUp && info.isDown && info.isRight)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadTriangle);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (info.isLeft && info.isDown && info.isRight)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadTriangle);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if ((info.isUp && info.isDown) || ((info.isDown || info.isUp) && (!info.isLeft && !info.isRight)))
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadLine);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);

            if(Random.value < 0.24f)
            {
                if(Random.value > 0.5f)
                {
                    Vector3 carPosition = new Vector3(gameObject1.transform.position.x - 0.3f, gameObject1.transform.position.y + (float)(0.5 * sizeTitles), gameObject1.transform.position.z);
                    GameObject car = Instantiate(carPrefab, carPosition, Quaternion.Euler(0,0,180), gameObject1.transform);
                    car.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite("carsCity_"+Random.Range(0,5));
                }
                else
                {
                    Vector3 carPosition = new Vector3(gameObject1.transform.position.x + 0.3f, gameObject1.transform.position.y - (float)(0.5 * sizeTitles), gameObject1.transform.position.z);
                    GameObject car = Instantiate(carPrefab, carPosition, Quaternion.Euler(0, 0, 0), gameObject1.transform);
                    car.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite("carsCity_" + Random.Range(0, 5));
                }
            }
        }
        else if ((info.isRight && info.isLeft) || ((info.isLeft || info.isRight) && (!info.isUp && !info.isDown)))
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadLine);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 90);

            if (Random.value < 0.23f)
            {
                if (Random.value > 0.5f)
                {
                    Vector3 carPosition = new Vector3(gameObject1.transform.position.x - (float)(0.5 * sizeTitles), gameObject1.transform.position.y - 0.3f, gameObject1.transform.position.z);
                    GameObject car = Instantiate(carPrefab, carPosition, Quaternion.Euler(0, 0, 270), gameObject1.transform);
                    car.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite("carsCity_" + Random.Range(0, 5));
                }
                else
                {
                    Vector3 carPosition = new Vector3(gameObject1.transform.position.x + (float)(0.5 * sizeTitles), gameObject1.transform.position.y + 0.3f, gameObject1.transform.position.z);
                    GameObject car = Instantiate(carPrefab, carPosition, Quaternion.Euler(0, 0, 90), gameObject1.transform);
                    car.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite("carsCity_" + Random.Range(0, 5));
                }
            }
        }
        else if (info.isLeft && info.isUp)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadCorner);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (info.isUp && info.isRight)
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadCorner);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if ((info.isDown && info.isRight))
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadCorner);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if ((info.isDown && info.isLeft))
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = atlasCity.GetSprite(roadCorner);
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject1.GetComponent<SpriteRenderer>().sprite = null;
            gameObject1.transform.rotation = Quaternion.Euler(0, 0, 0);

        }

        gameObject1.transform.localPosition = new Vector3(location.x * sizeTitles, location.y * sizeTitles, 0);
    }


    class Map
    {
        public TypeOfCell[,] cells;
        public Location? endLocationRoad;
        public GameObject parent;
        public GameObject[,] spriteObjects;
        private GameObject prefab;
        private float sizeTitle;
        public Map(GameObject gameParent, byte sizeMapX, byte sizeMapY, GameObject spritePrefab, float sizeTitle)
        {
            this.sizeTitle = sizeTitle;
            parent = new GameObject();
            parent.name = "map";
            parent.transform.SetParent(gameParent.transform);

            cells = new TypeOfCell[sizeMapX, sizeMapY];
            spriteObjects = new GameObject[sizeMapX, sizeMapY];
            prefab = spritePrefab;
            InstantiateGameObjects();

            endLocationRoad = null;
        }

        public bool IsOutOfBounds(Location location)
        {
            if (location.x < 0 || location.y < 0 || location.x >= cells.GetLength(0) || location.y >= cells.GetLength(1)) return true;
            return false;
        }

        public void SetPosition(Vector3 position)
        {
          
            parent.transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return parent.transform.position;
        }

        public void CleanAll(Vector3 position)
        {
            cells = new TypeOfCell[cells.GetLength(0), cells.GetLength(1)];
            endLocationRoad = null;
            parent.transform.position = position;

            for(int i = 0; i < spriteObjects.GetLength(0); i++)
            {
                for(int j = 0; j < spriteObjects.GetLength(1); j++)
                {
                    if (spriteObjects[i, j].transform.childCount > 0) Destroy(spriteObjects[i, j].transform.GetChild(0).gameObject);
                }
            }
        }

        private void InstantiateGameObjects()
        {
            for (int i = 0; i < spriteObjects.GetLength(0); i++)
            {
                for (int j = 0; j < spriteObjects.GetLength(1); j++)
                {
                    spriteObjects[i, j] = Instantiate(prefab, parent.transform, false);
                    spriteObjects[i, j].transform.localPosition = new Vector3(i * sizeTitle, j * sizeTitle, 0);
                    spriteObjects[i, j].transform.localScale = new Vector3(sizeTitle, sizeTitle, sizeTitle);
                }
            }
        }
    }
    /// <summary>
    /// Возражает информация о сотоянии 8 соседних блоков
    /// </summary>
    class CellInformation
    {
        public bool isUp;
        public bool isU_R;
        public bool isRight;
        public bool isR_D;
        public bool isDown;
        public bool isD_L;
        public bool isLeft;
        public bool isL_U;

        public CellInformation(Map map, Location location, TypeOfCell type)
        {
            isUp = (location.y + 1 >= map.cells.GetLength(1)) ? false : ((map.cells[location.x, location.y + 1] == type) ? true : false);
            isU_R = (location.x + 1 >= map.cells.GetLength(0) || location.y + 1 >= map.cells.GetLength(1)) ? false : ((map.cells[location.x + 1, location.y + 1] == type) ? true : false);

            isRight = (location.x + 1 >= map.cells.GetLength(0)) ? false : ((map.cells[location.x + 1, location.y] == type) ? true : false);
            isR_D = ((location.y - 1 < 0) || (location.x + 1) >= map.cells.GetLength(0)) ? false : ((map.cells[location.x + 1, location.y - 1] == type) ? true : false);

            isDown = (location.y - 1 < 0) ? false : ((map.cells[location.x, location.y - 1] == type) ? true : false);
            isD_L = ((location.y - 1 < 0) || (location.x - 1 < 0)) ? false : ((map.cells[location.x - 1, location.y - 1] == type) ? true : false);

            isLeft = (location.x - 1 < 0) ? false : ((map.cells[location.x - 1, location.y] == type) ? true : false);
            isL_U = ((location.y + 1 >= map.cells.GetLength(1)) || (location.x - 1 < 0)) ? false : ((map.cells[location.x - 1, location.y + 1] == type) ? true : false);
        }

        public bool GetValueByVector(Vector2 vector)
        {
            if (vector == Vector2.up) return isUp;
            if (vector == Vector2.down) return isDown;
            if (vector == Vector2.left) return isLeft;
            if (vector == Vector2.right) return isRight;

            return false;
        }

        public bool IsBlockNear(Vector2 vector)
        {
            return (isUp || isDown || isRight || isLeft);
        }

        public bool IsAllEmpty()
        {
            return !(isUp || isU_R || isRight || isR_D || isDown || isD_L || isLeft || isL_U);
        }

    }

}


