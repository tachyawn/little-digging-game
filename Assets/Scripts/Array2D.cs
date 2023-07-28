[System.Serializable]

public class Array2D
{
    public int width = 16, height = 8;
 
    /// <summary>2D array stored in 1D array.</summary>
    public int[] SingleArray;
 
    public int this[int x, int y]
    {
        get => SingleArray[y * width + x];
        set => SingleArray[y * width + x] = value;
    }
 
    ///<summary>Array2D default instantiation contains values used for coin patterns only</summary>
    public Array2D()
    {
        width = 16;
        height = 8;
        SingleArray = new int[16 * 8];
    }

    public Array2D(int x, int y)
    {
        width = x;
        height = y;
        SingleArray = new int[x * y];
    }
 
    /// <summary>Gets the total number of elements in X dimension (1st dimension). </summary>
    public int Get_X_Length => width;
 
    /// <summary>Gets the total number of elements in Y dimension. (2nd dimension).</summary>
    public int Get_Y_Length => height;
 
    /// <summary>Gets the total number of elements all dimensions.</summary>
    public int Length => SingleArray.Length;
}
