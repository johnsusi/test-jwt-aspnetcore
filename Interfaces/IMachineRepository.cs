using Models;

namespace Interfaces
{

  public interface IMachineRepository
  {
    Machine GetById(int machineId);
  }

}