using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchmarkExample
{
    public class FixedStack<T>
    {
        private T[] items;
        private int count;
        const int n = 10000;
        public FixedStack()
        {
            items = new T[n];
        }
        public FixedStack(int length)
        {
            items = new T[length];
        }

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public int Count
        {
            get { return count; }
        }

        public void Push(T item)
        {
            if (count == items.Length)
                throw new InvalidOperationException("Переполнение стека");
            items[count++] = item;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            T item = items[--count];
            items[count] = default(T);
            return item;
        }

        public T Peek()
        {
            return items[count - 1];
        }
    }

    public class StackWithDynamicArray<T>
    {
        private T[] items;
        private int count;
        const int n = 10;
        public StackWithDynamicArray()
        {
            items = new T[n];
        }
        public StackWithDynamicArray(int length)
        {
            items = new T[length];
        }

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public int Count
        {
            get { return count; }
        }

        public void Push(T item)
        {
            if (count == items.Length)
                Resize(items.Length + 10);

            items[count++] = item;
        }
        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            T item = items[--count];
            items[count] = default(T);

            if (count > 0 && count < items.Length - 10)
                Resize(items.Length - 10);

            return item;
        }
        public T Peek()
        {
            return items[count - 1];
        }

        private void Resize(int max)
        {
            T[] tempItems = new T[max];
            for (int i = 0; i < count; i++)
                tempItems[i] = items[i];
            items = tempItems;
        }
    }

    public class QueueWithDynamicArray<T>
    {
        private T[] _array;
        private int size;
        private const int defaultCapacity = 10;
        private int capacity;
        private int head;
        private int tail;

        public QueueWithDynamicArray()
        {
            capacity = defaultCapacity;
            this._array = new T[defaultCapacity];
            this.size = 0;
            this.head = -1;
            this.tail = 0;
        }

        public bool isEmpty()
        {
            return size == 0;
        }

        public void Enqueue(T newElement)
        {
            if (this.size == this.capacity)
            {
                T[] newQueue = new T[2 * capacity];
                Array.Copy(_array, 0, newQueue, 0, _array.Length);
                _array = newQueue;
                capacity *= 2;
            }
            size++;
            _array[tail++ % capacity] = newElement;
        }

        public T Dequeue()
        {
            if (this.size == 0)
            {
                throw new InvalidOperationException();
            }
            size--;
            return _array[++head % capacity];
        }


        public int Count
        {
            get
            {
                return this.size;
            }
        }
    }

    public class StackBenchmark
    {
        [Benchmark(Description = "StackFixedArray")]
        public void TestStackWithFixedArray()
        {
            var random = new Random();
            var fixedArray = new FixedStack<double>();
            for (var i = 0; i < 1000; i++)
            {
                fixedArray.Push(random.Next(10000) + random.NextDouble());
            }
        }

        [Benchmark(Description = "StackDynamicArray")]
        public void TestStackWithDynamicArray()
        {
            var random = new Random();
            var dynamicArray = new StackWithDynamicArray<double>();
            for (var i = 0; i < 1000; i++)
            {
                dynamicArray.Push(random.Next(10000) + random.NextDouble());
            }
        }

        [Benchmark(Description = "DefaultStack")]
        public void TestDefaultStack()
        {
            var random = new Random();
            var stack = new Stack<double>();
            for (var i = 0; i < 1000; i++)
            {
                stack.Push(random.Next(10000) + random.NextDouble());
            }
        }

        [Benchmark(Description = "QueueWithArray")]
        public void TestQueueWithArray()
        {
            var random = new Random();
            var queueWithDynamicArray = new Queue<double>();
            for (var i = 0; i < 1000; i++)
            {
                queueWithDynamicArray.Enqueue(random.Next(10000) + random.NextDouble());
            }
        }

        [Benchmark(Description = "DefaultQueue")]
        public void TestDefaultQueue()
        {
            var random = new Random();
            var queue = new Queue<double>();
            for (var i = 0; i < 1000; i++)
            {
                queue.Enqueue(random.Next(10000) + random.NextDouble());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            BenchmarkRunner.Run<StackBenchmark>();
            Console.ReadLine();
        }
    }
}