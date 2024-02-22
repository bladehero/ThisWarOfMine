namespace ThisWarOfMine.Infrastructure.Books;

public abstract class BookFolderPath
{
    private readonly string _localPath;

    public abstract string Path { get; }

    private BookFolderPath(string localPath) => _localPath = localPath;

    public static BookFolderPath From(BookFolderSettings settings)
    {
        var (bookFolderType, localPath) = settings;
        return bookFolderType switch
        {
            BookFolderType.Absolute => AsAbsolute(localPath),
            BookFolderType.AppData => AsAppData(localPath),
            _ => throw new InvalidOperationException($"Cannot create book folder from settings: `{settings}`")
        };
    }

    public static BookFolderPath AsAbsolute(string path) => new AbsoluteBookFolderPath(path);

    public static BookFolderPath AsAppData(string path) => new AppDataBookFolderPath(path);

    public static implicit operator BookFolderPath(string path) => AsAbsolute(path);

    public static implicit operator BookFolderPath(BookFolderSettings settings) => From(settings);

    private sealed class AbsoluteBookFolderPath : BookFolderPath
    {
        public AbsoluteBookFolderPath(string localPath)
            : base(localPath) { }

        public override string Path => _localPath;
    }

    private sealed class AppDataBookFolderPath : BookFolderPath
    {
        private static readonly string AppDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData
        );

        public AppDataBookFolderPath(string localPath)
            : base(localPath)
        {
            if (System.IO.Path.IsPathRooted(localPath))
            {
                throw new InvalidOperationException("Cannot create path because local path should be relative");
            }
        }

        public override string Path => System.IO.Path.Combine(AppDataPath, _localPath);
    }

    public sealed class BookFolderSettings
    {
        public BookFolderType Type { get; set; }
        public string? LocalPath { get; set; }

        public void Deconstruct(out BookFolderType type, out string? localPath)
        {
            type = Type;
            localPath = LocalPath;
        }
    }

    public enum BookFolderType
    {
        Absolute = 1,
        AppData = 2,
    }
}
