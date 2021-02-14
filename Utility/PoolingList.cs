using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using engenious.Helper;

namespace engenious.Utility
{
    /// <summary>
    /// A dynamically growing list that uses memory pooling for its buffers.
    /// </summary>
    /// <typeparam name="T">The generic type to contain in this list.</typeparam>
    public class PoolingList<T> : IList<T>
    {
        private const int DefaultCapacity = 4;
        private int _size;
        private T[] _pooledArray;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PoolingList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity to initialize the <see cref="PoolingList{T}"/> with.</param>
        public PoolingList(int capacity = DefaultCapacity)
        {
            _pooledArray = ArrayPool<T>.Shared.Rent(capacity);
            _size = 0;
        }

        /// <summary>
        /// Gets a struct enumerator for this <see cref="PoolingList{T}"/>.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public Enumerator GetEnumerator()
        {
            return new(this);
        }
        
        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc see="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            var array = _pooledArray;
            var size = _size;
            if ((uint)size < (uint)array.Length)
            {
                _size = size + 1;
                array[size] = item;
            }
            else
            {
                AddWithResize(item);
            }
        }

        // Non-inline from List.Add to improve its code quality as uncommon path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddWithResize(T item)
        {
            int size = _size;
            EnsureCapacity(size + 1);
            _size = size + 1;
            _pooledArray[size] = item;
        }
        private void EnsureCapacity(int min)
        {
            if (_pooledArray.Length >= min)
                return;
            var newCapacity = _pooledArray.Length == 0 ? DefaultCapacity : _pooledArray.Length * 2;
            Capacity = newCapacity;
        }
        /// <summary>
        /// Gets or sets the capacity of this <see cref="PoolingList{T}"/>.
        /// </summary>
        public int Capacity
        {
            get => _pooledArray.Length;
            set
            {
                if (value != (_pooledArray?.Length ?? 0))
                {
                    if (value > 0)
                    {
                        var newItems = ArrayPool<T>.Shared.Rent(value);
                        if (_size > 0)
                        {
                            if (_pooledArray != null)
                                Array.Copy(_pooledArray, newItems, _size);
                        }
                        if (_pooledArray != null)
                            ArrayPool<T>.Shared.Return(_pooledArray);
                        _pooledArray = newItems;
                        
                        _size = Math.Min(_size, _pooledArray.Length);
                    }
                    else
                    {
                        _size = Math.Min(_size, value);
                    }
                }
            }
        }
        /// <inheritdoc />
        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                var size = _size;
                _size = 0;
                if (size > 0)
                {
                    Array.Clear(_pooledArray, 0, size); // Clear the elements so that the gc can reclaim the references.
                }
            }
            else
            {
                _size = 0;
            }
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _size != 0 && IndexOf(item) != -1;
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_pooledArray, 0, array, arrayIndex, _size);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;

        }

        /// <inheritdoc />
        public int Count => _size;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public int IndexOf(T item) => Array.IndexOf(_pooledArray, item, 0, _size);

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_size)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            }
            if (_size == _pooledArray.Length)
                EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_pooledArray, index, _pooledArray, index + 1, _size - index);
            }
            _pooledArray[index] = item;
            _size++;
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            }
            _size--;
            if (index < _size)
            {
                Array.Copy(_pooledArray, index + 1, _pooledArray, index, _size - index);
            }
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                _pooledArray[_size] = default!;
            }
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_size)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
                }
                return _pooledArray[index];
            }
            set
            {
                if ((uint)index >= (uint)_size)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
                }
                _pooledArray[index] = value;
            }
        }

        /// <summary>
        /// A struct enumerator for this <see cref="PoolingList{T}"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly PoolingList<T> _list;
            private int _index;
            private T? _current;

            internal Enumerator(PoolingList<T> list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            /// <inheritdoc />
            public void Dispose()
            {
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                PoolingList<T> localList = _list;

                if (((uint)_index < (uint)localList._size))
                {
                    _current = localList._pooledArray[_index];
                    _index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = _list._size + 1;
                _current = default;
                return false;
            }

            /// <inheritdoc />
            public T Current => _current!;

            object? IEnumerator.Current => Current;

            void IEnumerator.Reset()
            {
                _index = 0;
                _current = default;
            }
        }
        
        /// <summary>
        /// Steals the pooled memory buffer of this list and resets this list.
        /// <remarks>This buffer needs to be returned to the memory pool using <seealso cref="ReturnBuffer"/></remarks>
        /// </summary>
        /// <returns>The stolen memory buffer.</returns>
        public T[] StealAndClearBuffer()
        {
            var buf = _pooledArray;
            _size = 0;
            _pooledArray = ArrayPool<T>.Shared.Rent(DefaultCapacity);
            return buf;
        }

        /// <summary>
        /// Returns a buffer into the memory pool.
        /// </summary>
        /// <param name="buffer">The buffer to return to the memory pool.</param>
        public void ReturnBuffer(T[] buffer)
        {
            ArrayPool<T>.Shared.Return(buffer);
        }
    }
}