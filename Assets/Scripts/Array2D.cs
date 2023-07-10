using UnityEngine;

[System.Serializable]
public class Array2D<T> where T : struct
{
    public int width, height;
 
    /// <summary>2D array stored in 1D array.</summary>
    public T[] SingleArray;
 
    public T this[int x, int y]
    {
        get => SingleArray[y * width + x];
        set => SingleArray[y * width + x] = value;
    }
 
    public Array2D(int x, int y)
    {
        width = x;
        height = y;
        SingleArray = new T[x * y];
    }
 
    /// <summary>Gets the total number of elements in X dimension (1st dimension). </summary>
    public int Get_X_Length => width;
 
    /// <summary>Gets the total number of elements in Y dimension. (2nd dimension).</summary>
    public int Get_Y_Length => height;
 
    /// <summary>Gets the total number of elements all dimensions.</summary>
    public int Length => SingleArray.Length;
 
}
