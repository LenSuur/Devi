
using System.Text;
using Devi.Services;

namespace Devi.UnitTests.ServiceTests;

public class LocalFileClientTests
{
    private readonly LocalFileClient _localFileClient;
    private const string TestContainer = "test-container";
    private const string RootPath = "wwwroot\\uploads";
    
    public LocalFileClientTests()
    {
        _localFileClient = new LocalFileClient();
    }
    
    [Fact]
    public async Task Exists_should_return_false_for_nonexistent_file()
    {
        //Arrange
        string testFileName = "test-file_a.txt";
        
        // Act
        var exists = await _localFileClient.Exists(TestContainer, testFileName);

        // Assert
        Assert.False(exists);
    }
    
    [Fact]
    public async Task Get_should_return_null_for_nonexistent_file()
    {
        //Arrange
        string testFileName = "test-file_b.txt";
        // Act
        var stream = await _localFileClient.Get(TestContainer, testFileName);

        // Assert
        Assert.Null(stream);
    }
    
    [Fact]
    public async Task Delete_should_delete_existing_file()
    {
        // Arrange
        string testFileName = "test-file_c.txt";
        var filePath = Path.Combine(RootPath, TestContainer, testFileName);
        Directory.CreateDirectory(Path.Combine(RootPath, TestContainer));
        File.WriteAllText(filePath, "Test file content");

        // Act
        await _localFileClient.Delete(TestContainer, testFileName);

        // Assert
        Assert.False(File.Exists(filePath));
    }

    [Fact]
    public async Task Delete_should_not_throw_exception_for_nonexistent_file()
    {
        // Arrange
        var container = "uploads";
        var fileName = "nonexistent.txt";

        // Act
        await _localFileClient.Delete(container, fileName);
    }
    
    [Fact]
    public async Task Exists_should_return_true_for_existing_file()
    {
        //Arrange
        string testFileName = "test-file_d.txt";
        var filePath = Path.Combine(RootPath, TestContainer, testFileName);
        Directory.CreateDirectory(Path.Combine(RootPath, TestContainer));
        File.WriteAllText(filePath, "Test file content");

        // Act
        var exists = await _localFileClient.Exists(TestContainer, testFileName);

        // Assert
        Assert.True(exists);
    }
    
    [Fact]
    public async Task Get_should_return_stream_for_existing_file()
    {
        // Arrange
        string testFileName = "test-file_e.txt";
        var filePath = Path.Combine(RootPath, TestContainer, testFileName);
        Directory.CreateDirectory(Path.Combine(RootPath, TestContainer));
        File.WriteAllText(filePath, "Test file content");

        // Act
        var stream = await _localFileClient.Get(TestContainer, testFileName);

        // Assert
        Assert.NotNull(stream);
        Assert.IsType<FileStream>(stream);
    }
    
    [Fact]
    public async Task List_should_return_file_list_for_existing_container()
    {
        // Arrange
        var filePath1 = Path.Combine(RootPath, TestContainer, "file1.txt");
        var filePath2 = Path.Combine(RootPath, TestContainer, "file2.txt");
        if (Directory.Exists(Path.Combine(RootPath, TestContainer)))
            Directory.Delete(Path.Combine(RootPath, TestContainer), true);
        Directory.CreateDirectory(Path.Combine(RootPath, TestContainer));
        File.WriteAllText(filePath1, "Test file 1 content");
        File.WriteAllText(filePath2, "Test file 2 content");

        // Act
        var fileList = await _localFileClient.List(TestContainer, string.Empty);
        
        // Assert
        Assert.Equal(new List<string> { filePath1, filePath2 }, fileList);
    }
    
    [Fact]
    public async Task Save_should_save_file()
    {
        // Arrange
        string testFileName = "test-file_f.txt";
        var containerPath = Path.Combine(RootPath, TestContainer);
        var filePath = Path.Combine(containerPath, testFileName);
        var content = "Test file content";
        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        await _localFileClient.Save(TestContainer, testFileName, inputStream);
        var savedContent = await File.ReadAllTextAsync(filePath);


        // Assert
        Assert.True(File.Exists(filePath));

        // Read the saved file content and verify
        Assert.Equal(content, savedContent);
    }
}