using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LoLSharp.Game;
using SharpDX;

namespace LoLSharp.Modules
{
  class Memory
  {
    private static Process _hProcess;
    private static VAMemory _pubmemory;
    public static VAMemory Pubmemory
    {
      get => _pubmemory;
      set => _pubmemory = value;
    }
    public static Process HProcess
    {
      get => _hProcess;
      set => _hProcess = value;
    }

    public static void Init()
    {
      HProcess = Utils.GetLeagueProcess();
      if (HProcess != null)
      {
        Pubmemory = new VAMemory(HProcess.ProcessName);
      }
    }
    public static T Read<T>(long address)
    {
      var size = Marshal.SizeOf<T>();
      var intPtr = (IntPtr)address;
      var readByteArray = Pubmemory.ReadByteArray(intPtr, (uint)size);
      var ptr = Marshal.AllocHGlobal(size);
      Marshal.Copy(readByteArray, 0, ptr, size);
      var structure = Marshal.PtrToStructure<T>(ptr);
      Marshal.FreeHGlobal(ptr);
      return structure;
    }


    public static List<long> ReadVTable(long address)
    {
      var list = Read<long>(address + 0x8);
      var size = Read<int>(address + 0x10);
      List<long> result = new List<long>(size);


      for (var i = 0; i < size; i++)
      {
        result.Add(ReadByPtr(list + (i * 0x8)));
      }
      return result;
    }

    public static string ReadString(long address)
    {
      return Pubmemory.ReadStringASCII(new IntPtr(address), 50).Split('\0')[0];
    }
    public const int SizeBuff = 0x4100;
    
    public static void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count)
    {
      if (src == null || dst == null)
      {
        throw new ArgumentNullException();
      }
      if (srcOffset < 0 || dstOffset < 0 || count < 0)
      {
        throw new ArgumentOutOfRangeException();
      }
      if (src.Length - srcOffset < count || dst.Length - dstOffset < count)
      {
        throw new ArgumentException();
      }

      for (int i = 0; i < count; i++)
      {
        object value = src.GetValue(srcOffset + i);
        dst.SetValue(value, dstOffset + i);
      }
    }
    public static string ReadStringName(long address)
    {
      byte[] dataBuffer = new byte[SizeBuff];
      byte[] nameBuff = new byte[50];
      NativeImport.ReadProcessMemory(_hProcess.Handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out IntPtr bytesRead);
      var readDwordFromBuffer = ReadDwordFromBuffer(dataBuffer, OffsetManager.Object.Name);
      if (bytesRead == IntPtr.Zero)
      {
        return string.Empty;
      }
      NativeImport.ReadProcessMemory(_hProcess.Handle, (IntPtr)readDwordFromBuffer, nameBuff, nameBuff.Length, out bytesRead);

      if (ContainsOnlyASCII(nameBuff, 50))
      {
        string name = Encoding.ASCII.GetString(nameBuff);
        var readStringName = name.Split('\0')[0];

        if (!string.IsNullOrEmpty(readStringName))
        {
          return readStringName;
        }
      }

      return ReadString(address + OffsetManager.Object.Name);
    }

    public static bool ContainsOnlyASCII(byte[] buff, int maxSize)
    {
      for (int i = 0; i < maxSize; ++i)
      {
        if (buff[i] == 0)
          return true;
        if (buff[i] > 127)
          return false;
      }
      return true;
    }

    public static long ReadDwordFromBuffer(byte[] buffer, int offset)
    {
      if (buffer == null || buffer.Length < offset + sizeof(uint))
      {
        throw new ArgumentException("Invalid buffer or offset");
      }

      return BitConverter.ToInt64(buffer, offset);
    }

    public static Matrix ReadMatrix(long address)
    {
      Matrix tmp = Matrix.Zero;

      var Buffer = Pubmemory.ReadByteArray((IntPtr)address, 64);
      // NativeImport.ReadProcessMemory(_hProcess.Handle, (IntPtr)address, Buffer, 64, out ByteRead);

      tmp.M11 = BitConverter.ToSingle(Buffer, (0 * 4));
      tmp.M12 = BitConverter.ToSingle(Buffer, (1 * 4));
      tmp.M13 = BitConverter.ToSingle(Buffer, (2 * 4));
      tmp.M14 = BitConverter.ToSingle(Buffer, (3 * 4));

      tmp.M21 = BitConverter.ToSingle(Buffer, (4 * 4));
      tmp.M22 = BitConverter.ToSingle(Buffer, (5 * 4));
      tmp.M23 = BitConverter.ToSingle(Buffer, (6 * 4));
      tmp.M24 = BitConverter.ToSingle(Buffer, (7 * 4));

      tmp.M31 = BitConverter.ToSingle(Buffer, (8 * 4));
      tmp.M32 = BitConverter.ToSingle(Buffer, (9 * 4));
      tmp.M33 = BitConverter.ToSingle(Buffer, (10 * 4));
      tmp.M34 = BitConverter.ToSingle(Buffer, (11 * 4));

      tmp.M41 = BitConverter.ToSingle(Buffer, (12 * 4));
      tmp.M42 = BitConverter.ToSingle(Buffer, (13 * 4));
      tmp.M43 = BitConverter.ToSingle(Buffer, (14 * 4));
      tmp.M44 = BitConverter.ToSingle(Buffer, (15 * 4));

      return tmp;
    }

    public static Vector3 Read3DVector(int address)
    {
      Vector3 tmp = new Vector3();

      byte[] Buffer = new byte[12];
      IntPtr ByteRead;

      // NativeImport.ReadProcessMemory(_hProcess.Handle, (IntPtr)(address + Game.OffsetManager.Object.Pos), Buffer, 12, out ByteRead);

      tmp.X = BitConverter.ToSingle(Buffer, (0 * 4));
      tmp.Y = BitConverter.ToSingle(Buffer, (1 * 4));
      tmp.Z = BitConverter.ToSingle(Buffer, (2 * 4));

      return tmp;
    }
    public static void WriteFloat(IntPtr instance, float value)
    {
      Pubmemory.WriteFloat(instance, value);
    }


    public static string ReadString(IntPtr instance)
    {
      var readStringAscii = Pubmemory.ReadStringASCII(instance, 512);
      return readStringAscii;
      return Encoding.ASCII.GetString(ReadByteArray(instance, 512), 0, (int)512).Split('\0')[0];
    }
    public static byte[] ReadByteArray(IntPtr pOffset, uint pSize)
    {
      // (Add your implementation for reading a byte array from the process's memory)
      return new byte[pSize];
    }

    public static long ReadByPtr(IntPtr address)
    {
      return Pubmemory.ReadULong(address);
    }

    public static long ReadByPtr(long address)
    {
      return Pubmemory.ReadULong(new IntPtr(address));
    }

  }
}
