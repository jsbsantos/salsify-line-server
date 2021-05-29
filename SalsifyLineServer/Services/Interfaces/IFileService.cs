using System.Threading.Tasks;

namespace SalsifyLineServer.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> GetLine(int index);
    }
}