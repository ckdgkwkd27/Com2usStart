using System.Text;

namespace com2us_start.TableImpl;

public interface ICsvTableBase
{
    public Task<bool> ExecuteAsync(string filename, IConfiguration conf);
}
