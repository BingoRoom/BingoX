namespace BingoX.FileSystem
{
    public interface IHistroyProvider<PID>
    {
        Histroy[] GetHistroys(string APIFullName);
        Histroy[] GetHistroys(PID id);
    }
}
