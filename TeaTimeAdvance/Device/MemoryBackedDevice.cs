using System;
using System.Buffers.Binary;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common;

namespace TeaTimeAdvance.Device
{
    public class MemoryBackedDevice : IBusDevice
    {
        protected byte[] _data;

        public virtual uint MappedSize => (uint)_data.Length;

        public MemoryBackedDevice(int size)
        {
            _data = new byte[size];
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data)
        {
            // Enforce a full copy here.
            _data = data.ToArray();
        }

        public MemoryBackedDevice(ReadOnlySpan<byte> data, int size) : this(size)
        {
            data.CopyTo(_data);
        }

        protected Span<byte> GetSpan(uint baseAddress, uint address, int size)
        {
            return _data.AsSpan().Slice((int)((address - baseAddress) % MappedSize), size);
        }

        public virtual byte Read8(uint baseAddress, uint address)
        {
            return GetSpan(baseAddress, address, sizeof(byte))[0];
        }

        public virtual ushort Read16(uint baseAddress, uint address)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(GetSpan(baseAddress, address, sizeof(ushort)));
        }

        public virtual uint Read32(uint baseAddress, uint address)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(GetSpan(baseAddress, address, sizeof(uint)));
        }

        public virtual void Write8(uint baseAddress, uint address, byte value)
        {
            GetSpan(baseAddress, address, sizeof(byte))[0] = value;
        }

        public virtual void Write16(uint baseAddress, uint address, ushort value)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(GetSpan(baseAddress, address, sizeof(ushort)), value);
        }

        public virtual void Write32(uint baseAddress, uint address, uint value)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(GetSpan(baseAddress, address, sizeof(uint)), value);
        }
    }
}
