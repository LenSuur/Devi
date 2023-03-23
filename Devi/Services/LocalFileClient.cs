namespace Devi.Services;

public class LocalFileClient : IFileClient
{
    private const string RootPath = "wwwroot\\uploads";

    public async Task Delete(string container, string fileName)
    {
        var path = GetPath(container, fileName);

        if(!File.Exists(path))
        {
            return;
        }

        File.Delete(path);
    }

    public async Task<bool> Exists(string container, string fileName)
    {
        var path = GetPath(container, fileName);

        return File.Exists(path);
    }

    public async Task<Stream> Get(string container, string fileName)
    {         
        if (!await Exists(container, fileName))
        {
            return null;
        }
        var path = GetPath(container, fileName);

        return File.OpenRead(path);
    }

    public async Task<IList<string>> List(string container, string prefix)
    {
        var path = Path.Combine(RootPath, container);

        return Directory.EnumerateFiles(path).ToList();
    }

    public async Task Save(string container, string fileName, Stream inputStream)
    {
        Delete(container, fileName);

        var path = GetPath(container, fileName);
        using(var outputStream = new FileStream(path, FileMode.CreateNew))
        {
            inputStream.CopyTo(outputStream);
        }
    }

    private string GetPath(string container, string fileName)
    {
        return Path.Combine(RootPath, container, fileName);
    }
}