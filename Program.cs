using System;

namespace KT12
{
    interface IList<T>
    {
        void Add(T item);
        void Remove(T item);
        T Get(int index);
        void Set(int index, T item);
        int Count { get; }
    }

    class ArrayList<T> : IList<T>
    {
        private T[] items;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public ArrayList()
        {
            items = new T[4];
            count = 0;
        }

        public void Add(T item)
        {
            if (count == items.Length)
            {
                T[] newItems = new T[items.Length * 2];

                for (int i = 0; i < items.Length; i++)
                {
                    newItems[i] = items[i];
                }

                items = newItems;
            }

            items[count] = item;
            count++;
        }

        public void Remove(T item)
        {
            int index = -1;

            for (int i = 0; i < count; i++)
            {
                if (object.Equals(items[i], item))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return;

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            count--;
            items[count] = default(T);
        }

        public T Get(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            return items[index];
        }

        public void Set(int index, T item)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            items[index] = item;
        }
    }

    class Node<T>
    {
        public T Data;
        public Node<T> Next;

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    class LinkedList<T> : IList<T>
    {
        private Node<T> head;
        private int count;

        public int Count
        {
            get { return count; }
        }

        public void Add(T item)
        {
            Node<T> newNode = new Node<T>(item);

            if (head == null)
            {
                head = newNode;
                count++;
                return;
            }

            Node<T> current = head;

            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = newNode;
            count++;
        }

        public void Remove(T item)
        {
            if (head == null)
                return;

            if (object.Equals(head.Data, item))
            {
                head = head.Next;
                count--;
                return;
            }

            Node<T> current = head;

            while (current.Next != null)
            {
                if (object.Equals(current.Next.Data, item))
                {
                    current.Next = current.Next.Next;
                    count--;
                    return;
                }

                current = current.Next;
            }
        }

        public T Get(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            Node<T> current = head;
            int i = 0;

            while (current != null)
            {
                if (i == index)
                    return current.Data;

                current = current.Next;
                i++;
            }

            throw new IndexOutOfRangeException();
        }

        public void Set(int index, T item)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            Node<T> current = head;
            int i = 0;

            while (current != null)
            {
                if (i == index)
                {
                    current.Data = item;
                    return;
                }

                current = current.Next;
                i++;
            }
        }
    }

    interface IComparer<T>
    {
        int Compare(T x, T y);
    }

    class StringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return x.Length.CompareTo(y.Length);
        }
    }

    class BookComparer : IComparer<Book>
    {
        public int Compare(Book x, Book y)
        {
            return x.Price.CompareTo(y.Price);
        }
    }

    interface IFactory<T>
    {
        T Create();
    }

    class RandomNumberFactory : IFactory<int>
    {
        private Random random = new Random();

        public int Create()
        {
            return random.Next(1, 101);
        }
    }

    class PersonFactory : IFactory<Person>
    {
        public Person Create()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());

            return new Person(name, age);
        }
    }

    class Person
    {
        public string Name;
        public int Age;

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return Name + " (" + Age + ")";
        }
    }

    class Book
    {
        public string Title;
        public decimal Price;

        public Book(string title, decimal price)
        {
            Title = title;
            Price = price;
        }

        public override string ToString()
        {
            return Title + " - " + Price;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("1. IList<T>");
            ArrayList<int> intList = new ArrayList<int>();
            intList.Add(10);
            intList.Add(20);
            intList.Add(30);
            Console.WriteLine("ArrayList<int>:");
            PrintList(intList);

            LinkedList<string> stringList = new LinkedList<string>();
            stringList.Add("one");
            stringList.Add("two");
            stringList.Add("three");
            Console.WriteLine("LinkedList<string>:");
            PrintList(stringList);

            ArrayList<Person> personList = new ArrayList<Person>();
            personList.Add(new Person("Иван", 18));
            personList.Add(new Person("Анна", 20));
            Console.WriteLine("ArrayList<Person>:");
            PrintList(personList);

            Console.WriteLine();
            Console.WriteLine("2. IComparer<T>");

            string[] words = { "cat", "elephant", "book", "hi" };
            SortArray(words, new StringComparer());
            Console.WriteLine("Строки после сортировки по длине:");
            PrintArray(words);

            Book[] books = {
                new Book("Book A", 500),
                new Book("Book B", 300),
                new Book("Book C", 700)
            };
            SortArray(books, new BookComparer());
            Console.WriteLine("Книги после сортировки по цене:");
            PrintArray(books);

            Console.WriteLine();
            Console.WriteLine("3. IFactory<T>");

            int[] numbers = CreateArray(new RandomNumberFactory(), 5);
            Console.WriteLine("Массив случайных чисел:");
            PrintArray(numbers);

            Console.WriteLine("Создание массива людей:");
            Person[] people = CreateArray(new PersonFactory(), 2);
            Console.WriteLine("Массив людей:");
            PrintArray(people);

            Console.ReadLine();
        }

        static void SortArray<T>(T[] array, IComparer<T> comparer)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (comparer.Compare(array[j], array[j + 1]) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }

        static T[] CreateArray<T>(IFactory<T> factory, int n)
        {
            T[] array = new T[n];

            for (int i = 0; i < n; i++)
            {
                array[i] = factory.Create();
            }

            return array;
        }

        static void PrintArray<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine(array[i]);
            }
        }

        static void PrintList<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list.Get(i));
            }
        }
    }
}