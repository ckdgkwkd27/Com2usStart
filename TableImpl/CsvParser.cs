using System.Text;
namespace com2us_start;

public class CsvParser
{
    private CsvParser() { }

    //Singleton
    private static readonly Lazy<CsvParser> _instance = new Lazy<CsvParser>(() => new CsvParser());
    public static CsvParser Instance { get { return _instance.Value; } }

    public List<List<string>> Parse(string filename)
    {
        List<List<string>> resultList = new List<List<string>>();
        List<string> rowList = new List<string>();
        StreamReader sr = new StreamReader(Path.Combine("Table/", filename), Encoding.Default);
        sr.ReadLine();
        
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line != null)
            {
                string[] data = line.Split(',');
                resultList.Add(data.ToList());
            } 
        }
        return resultList;
    }

}
