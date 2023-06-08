
namespace DoNotCopy.Core.Infrastructure
{
    public interface IPathProvider
    {
        string MapPath(string path);

        string MapPath(string path1, string path2);

        string MapPath(string path1, string path2, string path3);
        string MapPath(string path1, string path2, string path3,string path4);

    }
}
