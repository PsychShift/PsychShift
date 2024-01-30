[System.Serializable]
public class ArrayLayout
{
    public int xAxis = 7, yAxis = 7;
    
    [System.Serializable]
    public struct rowData {
        public bool[] row;
        public rowData(int values)
        {
            row = new bool[values];
        }
    }

    public rowData[] rows;

    public ArrayLayout()
    {
        rows = new rowData[yAxis];
        for(int i = 0; i < yAxis; i++)
        {
            rows[i] = new rowData(xAxis);
        }
    }
}