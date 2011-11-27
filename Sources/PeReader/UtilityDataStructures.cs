//-----------------------------------------------------------------------------
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the Microsoft Public License.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Cci.MetadataReader;

//^ using Microsoft.Contracts;

namespace Microsoft.Cci.UtilityDataStructures
{
	public sealed class EnumerableArrayWrapper<T, U> : IEnumerable<U> where T : class, U where U : class
	{
		public readonly T[] RawArray;
		public readonly U DummyValue;
		public EnumerableArrayWrapper(T[] rawArray, U dummyValue)
		{
			this.RawArray = rawArray;
			this.DummyValue = dummyValue;
		}

		public struct ArrayEnumerator : IEnumerator<U>
		{
			public T[] RawArray;
			public int CurrentIndex;
			public U DummyValue;

			public ArrayEnumerator(T[] rawArray, U dummyValue)
			{
				this.RawArray = rawArray;
				this.CurrentIndex = -1;
				this.DummyValue = dummyValue;
			}

			#region IEnumerator<U> Members

			public U Current {
				get {
					U retValue = this.RawArray[this.CurrentIndex];
					return retValue == null ? this.DummyValue : retValue;
				}
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
			}

			#endregion

			#region IEnumerator Members

			//^ [Confined]
			object 			/*?*/System.Collections.IEnumerator.Current {
				get {
					U retValue = this.RawArray[this.CurrentIndex];
					return retValue == null ? this.DummyValue : retValue;
				}
			}

			public bool MoveNext()
			{
				this.CurrentIndex++;
				return this.CurrentIndex < this.RawArray.Length;
			}

			public void Reset()
			{
				this.CurrentIndex = -1;
			}

			#endregion
		}

		#region IEnumerable<U> Members

		//^ [Pure]
		public IEnumerator<U> GetEnumerator()
		{
			return new ArrayEnumerator(this.RawArray, this.DummyValue);
		}

		#endregion

		#region IEnumerable Members

		//^ [Pure]
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new ArrayEnumerator(this.RawArray, this.DummyValue);
		}

		#endregion
	}

	public sealed class EnumerableMemoryBlockWrapper : IEnumerable<byte>
	{
		public readonly MemoryBlock MemBlock;
		public EnumerableMemoryBlockWrapper(MemoryBlock memBlock)
		{
			this.MemBlock = memBlock;
		}

		unsafe public struct MemoryBlockEnumerator : IEnumerator<byte>
		{
			public MemoryBlock MemBlock;
			public int CurrentOffset;
			public MemoryBlockEnumerator(MemoryBlock memBlock)
			{
				this.MemBlock = memBlock;
				this.CurrentOffset = -1;
			}

			#region IEnumerator<byte> Members

			public byte Current {
				get { return *(this.MemBlock.Buffer + this.CurrentOffset); }
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
			}

			#endregion

			#region IEnumerator Members

			//^ [Confined]
			object 			/*?*/System.Collections.IEnumerator.Current {
				get { return *(this.MemBlock.Buffer + this.CurrentOffset); }
			}

			public bool MoveNext()
			{
				this.CurrentOffset++;
				return this.CurrentOffset < this.MemBlock.Length;
			}

			public void Reset()
			{
				this.CurrentOffset = -1;
			}

			#endregion
		}

		#region IEnumerable<byte> Members

		//^ [Pure]
		public IEnumerator<byte> GetEnumerator()
		{
			return new MemoryBlockEnumerator(this.MemBlock);
		}

		#endregion

		#region IEnumerable Members

		//^ [Pure]
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new MemoryBlockEnumerator(this.MemBlock);
		}

		#endregion
	}

	public sealed class EnumerableBinaryDocumentMemoryBlockWrapper : IEnumerable<byte>
	{
		public readonly IBinaryDocumentMemoryBlock BinaryDocumentMemoryBlock;
		public EnumerableBinaryDocumentMemoryBlockWrapper(IBinaryDocumentMemoryBlock binaryDocumentMemoryBlock)
		{
			this.BinaryDocumentMemoryBlock = binaryDocumentMemoryBlock;
		}

		unsafe public struct MemoryBlockEnumerator : IEnumerator<byte>
		{
			public IBinaryDocumentMemoryBlock BinaryDocumentMemoryBlock;
			public byte* pointer;
			public int length;
			public int currentOffset;
			public MemoryBlockEnumerator(IBinaryDocumentMemoryBlock binaryDocumentMemoryBlock)
			{
				this.BinaryDocumentMemoryBlock = binaryDocumentMemoryBlock;
				this.pointer = binaryDocumentMemoryBlock.Pointer;
				this.length = (int)binaryDocumentMemoryBlock.Length;
				this.currentOffset = -1;
			}

			#region IEnumerator<byte> Members

			public byte Current {
				get { return *(pointer + this.currentOffset); }
			}

			#endregion

			#region IDisposable Members

			public void Dispose()
			{
			}

			#endregion

			#region IEnumerator Members

			//^ [Confined]
			object 			/*?*/System.Collections.IEnumerator.Current {
				get { return *(pointer + this.currentOffset); }
			}

			public bool MoveNext()
			{
				this.currentOffset++;
				return this.currentOffset < length;
			}

			public void Reset()
			{
				this.currentOffset = -1;
			}

			#endregion
		}

		#region IEnumerable<byte> Members

		//^ [Pure]
		public IEnumerator<byte> GetEnumerator()
		{
			return new MemoryBlockEnumerator(this.BinaryDocumentMemoryBlock);
		}

		#endregion

		#region IEnumerable Members

		//^ [Pure]
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return new MemoryBlockEnumerator(this.BinaryDocumentMemoryBlock);
		}

		#endregion
	}
}
