using UnityEngine;

[System.Serializable]
public class PatternList
{
    public int width = 0, height = 16, depth = 8;
    public bool isEmpty = true;

    /// <summary>3D array stored in 1D array.</summary>
    public int[] SingleArray;
 
    public int this[int x, int y, int z]
    {
        //get => SingleArray[y * width + x];
        //set => SingleArray[y * width + x] = value;
        get => SingleArray[x + height * (y + depth * z)];
    }
 
    public PatternList()
    {
        width = 0;
        height = 16;
        depth = 8; 
        SingleArray = new int[width * height * depth];
    }

    public void Add(Array2D newPattern)
    {
        int[] temp = new int[(width + 1) * height * depth];

        int j = 0;
        for (int i = 0; i < SingleArray.Length; i++)
        {
            if (isEmpty) break; //If SingleArray is empty, exit loop to not populate incorrectly
            temp[i] = SingleArray[i];
            j++;
        }
        for (int i = 0; i < newPattern.SingleArray.Length; i++)
        {
            temp[j] = newPattern.SingleArray[i];
            j++;
        }

        SingleArray = temp;
        width++;
        isEmpty = false;
    }

    ///<summary>Gets a specified pattern section and its values from the array. </summary>
    public int[] GetPattern(int index)
    {
        if (isEmpty)
        {
            Debug.LogError("The pattern you are trying to find does not exist because PatternList is Empty");
            return null;
        } 

        int patternLength = height * depth;
        int[] temp = new int[patternLength];
        int startIndex = index * (patternLength);

        for (int i = 0; i < patternLength; i++)
        {
            temp[i] = SingleArray[i + startIndex];
        }
        
        return temp;
    }

    public void RemovePatternAt(int index) 
    {
        if (isEmpty) return;
        else if (width == 1)
        {
            ClearPatterns();
            return;
        }

        int patternLength = height * depth;
        int[] temp = new int[(width-1) * patternLength];
        int startIgnore = index * patternLength;
        int j = 0;

        for (int i = 0; i < SingleArray.Length; i++)
        {
            if (i >= startIgnore && i < startIgnore + patternLength) continue;
            temp[j] = SingleArray[i];
            j++;
        }

        SingleArray = temp;
        width--;
    }

    public void ClearPatterns()
    {
        width = 0;
        isEmpty = true;
        SingleArray = new int[width * height * depth];
    }
 
    /// <summary>Gets the total number of patterns that make up the X dimension (1st dimension). </summary>
    public int Get_Patterns => width;
 
    /// <summary>Gets the total number of elements in Y dimension. (2nd dimension).</summary>
    public int Get_Y_Length => height;

    /// <summary>Gets the total number of elements in Z dimension. (3nd dimension).</summary>
    public int Get_Z_Length => depth;
 
    /// <summary>Gets the total number of elements all dimensions.</summary>
    public int Length => SingleArray.Length;
}