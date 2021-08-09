using System.IO;
using System.Threading.Tasks;

namespace Elevel.Application.Interfaces
{
    public interface IQuestionsImporter
    {
        Task GetDataAsync(Stream inputFileStream);
    }
}