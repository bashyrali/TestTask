using System.Threading.Tasks;
using TestApp.Models.Dtos;

namespace TestApp.Services
{
    public interface ICheckIin
    {
        Task SearchIin(ClientVm clientVm);
    }
}