using System;
using System.Collections;
using System.Collections.Generic;

namespace IListExtension.Tests.List
{
    public class CustomList<T> : IList<T>
    {
        private const int DefaultCapacity = 4;
        private const int MaxArrayLength = 0X7FEFFFFF;
        
        private T[] _items;
        private int _size;
        private int _version;
        
        static readonly T[]  _emptyArray = new T[0];

        public int Count => _items.Length;

        public bool IsReadOnly => false;
        
        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "");
                }
 
                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public CustomList()
        {
            _items = _emptyArray;
        }

        public CustomList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity,"capacity can't be less than 0");
            }
            if (capacity == 0)
            {
                _items = _emptyArray;
            }
            else
            {
                _items = new T[capacity];
            }
        }

        public CustomList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if( collection is ICollection<T> c) 
            {
                int count = c.Count;
                if (count == 0)
                {
                    _items = _emptyArray;
                }
                else {
                    _items = new T[count];
                    c.CopyTo(_items, 0);
                    _size = count;
                }
            }    
            else
            {                
                _size = 0;
                _items = _emptyArray;

                using(IEnumerator<T> en = collection.GetEnumerator()) 
                {
                    while(en.MoveNext()) 
                    {
                        Add(en.Current);                                    
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (_size == _items.Length)
            {
                EnsureCapacity(_size + 1);
            }
            _items[_size++] = item;
            _version++;
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }

        public bool Contains(T item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                {
                    if (_items[i] == null)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i=0; i<_size; i++)
                {
                    if (c.Equals(_items[i], item)) return true;
                }
                return false;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array != null && array.Rank != 1)
            {
                throw new ArgumentException("multidimensional array not supported", nameof(array));
            }
 
            try
            {                
                Array.Copy(_items, 0, array, arrayIndex, _size);
            }
            catch(ArrayTypeMismatchException)
            {
                throw new ArgumentException("array type not supported", nameof(array));
            }
        }
        
        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        public void Insert(int index, T item)
        {
            if ((uint) index > (uint) _size)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size) {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;            
            _version++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
 
            return false;
        }

        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }
            _size--;
            if (index < _size) {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default;
            _version++;
        }

        public T this[int index]
        {
            get 
            {
                if ((uint) index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _items[index]; 
            }
 
            set {
                if ((uint) index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _items[index] = value;
                _version++;
            }
        }
        
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0? DefaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > MaxArrayLength) newCapacity = MaxArrayLength;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
        
        [Serializable]
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private CustomList<T> list;
            private int index;
            private int version;
            private T current;
 
            internal Enumerator(CustomList<T> list)
            {
                this.list = list;
                index = 0;
                version = list._version;
                current = default;
            }
 
            public void Dispose()
            {
            }
 
            public bool MoveNext()
            {
 
                CustomList<T> localList = list;
 
                if (version == localList._version && ((uint)index < (uint)localList._size)) 
                {                                                     
                    current = localList._items[index];                    
                    index++;
                    return true;
                }
                return MoveNextRare();
            }
 
            private bool MoveNextRare()
            {                
                if (version != list._version)
                {
                    throw new InvalidOperationException("enum version failed");
                }
 
                index = list._size + 1;
                current = default;
                return false;                
            }
 
            public T Current
            {
                get
                {
                    return current;
                }
            }
 
            Object IEnumerator.Current
            {
                get
                {
                    if( index == 0 || index == list._size + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }
    
            void IEnumerator.Reset()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException("enum version failed");
                }
                
                index = 0;
                current = default;
            }
        }
    }
}