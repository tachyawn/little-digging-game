using Ink.Runtime;

[System.Serializable] //can be saved
public class GameData
{
    public int money;
    public int shovelLevel;
    public int detectorLevel;
    public float recordDepth;

    public bool visitedCurator;
    public bool visitedShop;

    public bool[] _artifactsFound;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        money = 0;
        shovelLevel = 1;
        detectorLevel = 1;
        recordDepth = 0.0f;

        visitedCurator = false;
        visitedShop = false;

        _artifactsFound = new bool[15];
    }
}
