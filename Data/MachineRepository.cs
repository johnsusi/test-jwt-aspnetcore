using System;
using Interfaces;
using Models;

namespace Data
{
  public class MachineRepository : IMachineRepository
  {

    public Machine GetById(int machineId)
    {
      Console.WriteLine($"machine {machineId}");
      switch (machineId) {
        case 1: return new Machine { Id = 1, Name = "Alpha" };
        case 2: return new Machine { Id = 2, Name = "Beta" };
        case 3: return new Machine { Id = 3, Name = "Gamma" };
        default: return null;
      }
    }

  }
}