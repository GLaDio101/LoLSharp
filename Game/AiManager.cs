using System.Numerics;
using LoLSharp.Modules;

namespace LoLSharp.Game
{
  public class AiManager
  {
    public int _address;

    public AiManager(int address)
    {
      _address = address;
    }

    public Vector3 GetStartPath()
    {
      return GetPosition(OffsetManager.Ai.AiManagerStartPath);
    }

    public Vector3 GetEndPath()
    {
      float posX = Memory.Read<float>(_address + OffsetManager.Ai.AiManagerEndPath);
      float posY = Memory.Read<float>(_address + OffsetManager.Ai.AiManagerEndPath + 0x4);
      float posZ = Memory.Read<float>(_address + OffsetManager.Ai.AiManagerEndPath + 0x8);

      var position = new Vector3() { X = posX, Y = posY, Z = posZ };
      return position;
      
      return GetPosition(OffsetManager.Ai.AiManagerEndPath);
    }

    public bool IsMoving()
    {
      return Memory.Read<bool>(_address + OffsetManager.Ai.AiManagerIsMoving);
    }


    public Vector3 GetPosition(long address)
    {
      float posX = Memory.Read<float>(_address + address);
      float posY = Memory.Read<float>(_address + address + 0x4);
      float posZ = Memory.Read<float>(_address + address + 0x8);

      var position = new Vector3() { X = posX, Y = posY, Z = posZ };
      return position;
    }

  }
}
