// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;

namespace Microsoft.AspNetCore.Server.Kestrel.Core.Internal
{
    internal class FakeMemory : IDisposable
    {
        private IMemoryOwner<byte>? _memoryOwner;
        private byte[]? _array;

        public Memory<byte> AvailableMemory { get; private set; }

        public void SetOwnedMemory(IMemoryOwner<byte> memoryOwner)
        {
            _memoryOwner = memoryOwner;
            AvailableMemory = memoryOwner.Memory;
        }

        public void SetOwnedMemory(byte[] arrayPoolBuffer)
        {
            _array = arrayPoolBuffer;
            AvailableMemory = arrayPoolBuffer;
        }

        public void ResetMemory()
        {
            if (_memoryOwner != null)
            {
                _memoryOwner.Dispose();
                _memoryOwner = null;
            }
            else if (_array != null)
            {
                ArrayPool<byte>.Shared.Return(_array);
                _array = null;
            }

            AvailableMemory = default;
        }

        public void Dispose()
        {
            ResetMemory();
        }
    }
}
