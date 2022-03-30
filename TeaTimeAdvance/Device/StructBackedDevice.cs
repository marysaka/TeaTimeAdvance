using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Common;

namespace TeaTimeAdvance.Device
{
    public class StructBackedDevice<T> : IBusDevice where T: unmanaged
    {
        public delegate void WriteCallbackDelegate(ref T data);

        private T _backingData;
        private WriteCallbackDelegate[] _writeCallbacks;

        public ref T Device => ref GetBackingDataRef();

        public StructBackedDevice()
        {
            _backingData = default;
            _writeCallbacks = new WriteCallbackDelegate[Unsafe.SizeOf<T>()];
        }

        public uint MappedSize => (uint)Unsafe.SizeOf<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref T GetBackingDataRef()
        {
            return ref _backingData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint GetIndex<U>(uint baseAddress, uint address) where U: unmanaged
        {
            uint offset = address - baseAddress;

            Debug.Assert(BitUtils.AlignUp(offset, Unsafe.SizeOf<U>()) == offset);

            return offset / (uint)Unsafe.SizeOf<U>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ref U GetAtAddress<U>(uint baseAddress, uint address) where U: unmanaged
        {
            return ref Unsafe.Add(ref Unsafe.As<T, U>(ref GetBackingDataRef()), GetIndex<U>(baseAddress, address));
        }

        public ushort Read16(uint baseAddress, uint address)
        {
            return GetAtAddress<ushort>(baseAddress, address);
        }

        public uint Read32(uint baseAddress, uint address)
        {
            return GetAtAddress<uint>(baseAddress, address);
        }

        public byte Read8(uint baseAddress, uint address)
        {
            return GetAtAddress<byte>(baseAddress, address);
        }

        public void Write16(uint baseAddress, uint address, ushort value)
        {
            GetAtAddress<ushort>(baseAddress, address) = value;

            SignalWriteAtOffset(address - baseAddress);
        }

        public void Write32(uint baseAddress, uint address, uint value)
        {
            GetAtAddress<uint>(baseAddress, address) = value;

            SignalWriteAtOffset(address - baseAddress);
        }

        public void Write8(uint baseAddress, uint address, byte value)
        {
            GetAtAddress<byte>(baseAddress, address) = value;

            SignalWriteAtOffset(address - baseAddress);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SignalWriteAtOffset(uint offset)
        {
            _writeCallbacks[offset]?.Invoke(ref GetBackingDataRef());
        }

        public void RegisterWriteCallback(string fieldName, WriteCallbackDelegate callback)
        {
            int offset = UnsafeHelper.OffsetOf<T>(fieldName);

            if (offset == -1)
            {
                throw new InvalidOperationException($"{fieldName} not found in {typeof(T).Name}");
            }

            _writeCallbacks[offset] = callback;
        }
    }
}
