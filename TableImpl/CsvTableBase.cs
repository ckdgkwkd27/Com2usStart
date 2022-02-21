using System.Text;

namespace com2us_start.TableImpl;

public interface ICsvTableBase
{
    public bool Execute(string filename);
    public void Load();
}
