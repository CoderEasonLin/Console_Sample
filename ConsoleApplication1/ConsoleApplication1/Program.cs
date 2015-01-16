using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace ApplicantTestin
{
    /// The DataObject class stored with a key
    class DataObject : IComparable<DataObject>
    {
        // Populate
        public string key;
        public int value;
        public DataObject reference;

        
        public DataObject(string str)
        {
            key = str;
            value = 0;
        }

        public void Ref(ref DataObject r)
        {
            reference = r;
        }

        public void UnRef()
        {
            reference = null;
        }

        public void Increase()
        {
            value++;
        }

        public void Decrease()
        {
            value--;
        }

        // Swap the value and refKey.
        public void Swap(ref DataObject o)
        {
            if (key == o.reference.key || reference.key == o.key)
                return;

            int tempValue = o.value;
            DataObject tempRef = o.reference;
            o.value = value;
            o.reference = reference;
            value = tempValue;
            reference = tempRef;
        }

        public bool IsRef()
        {
            return reference != null;
        }

        int Compare(DataObject other)
        {
            if (IsRef() && other.IsRef())
                return 0;
            else if (IsRef())
                return -1;
            else if (other.IsRef())
                return 1;
            else
                return value.CompareTo(other.value);
        }

        int IComparable<DataObject>.CompareTo(DataObject other)
        {
            return Compare(other);
        }
    }

    class Program
    {
        static Hashtable Data = new Hashtable();
        static string[] StaticData = new string[] {"X-Ray", "Echo", "Alpha", "Yankee", "Bravo", "Charlie", 
			"Delta", "Hotel", "India", "Juliet", "Foxtrot", "Sierra",
			"Mike", "Kilo", "Lima", "November", "Oscar", "Papa", "Qubec", 
			"Romeo", "Tango", "Golf", "Uniform", "Victor", "Whisky", "Zulu"};

        static void Main(string[] args)
        {
            for (int i = 0; i < StaticData.Length; i++)
                Data.Add(StaticData[i].ToLower(), new DataObject(StaticData[i]));
            while (true)
            {
                PrintSortedData();
                Console.WriteLine();
                Console.Write("> ");
                string str = Console.ReadLine();
                string[] strs = str.Split(' ');

                if (strs[0] == "q")
                    break;
                else if (strs[0] == "printv")
                    PrintSortedDataByValue();
                else if (strs[0] == "print")
                    PrintSortedData();
                else if (strs[0] == "inc")
                    Increase(strs[1]);
                else if (strs[0] == "dec")
                    Decrease(strs[1]);
                else if (strs[0] == "swap")
                    Swap(strs[1], strs[2]);
                else if (strs[0] == "ref")
                    Ref(strs[1], strs[2]);
                else if (strs[0] == "unref")
                    UnRef(strs[1]);
            }
        }

        /// <summary>
        /// Create a reference from one data object to another.
        /// </summary>
        /// <param name="key1">The object to create the reference on</param>
        /// <param name="key2">The reference object</param>
        static void Ref(string key1, string key2)
        {
            // Populate
            if (key1 == key2)
                return;
            if (!Data.ContainsKey(key1) || !Data.ContainsKey(key2))
                return;

            DataObject o = (DataObject)Data[key1];
            DataObject o2 = (DataObject)Data[key2];
            o.Ref(ref o2);
        }

        /// <summary>
        /// Removes an object reference on the object specified.
        /// </summary>
        /// <param name="key">The object to remove the reference from</param>
        static void UnRef(string key)
        {
            // Populate
            if (!Data.ContainsKey(key))
                return;

            DataObject o = (DataObject)Data[key];
            o.UnRef();
        }

        /// <summary>
        /// Swap the data objects stored in the keys specified
        /// </summary>
        static void Swap(string key1, string key2)
        {
            // Populate
            if (!Data.ContainsKey(key1) || !Data.ContainsKey(key2))
                return;

            DataObject o1 = (DataObject)Data[key1];
            DataObject o2 = (DataObject)Data[key2];
            o1.Swap(ref o2);
        }

        /// <summary>
        /// Decrease the Value field by 1 of the 
        /// data object stored with the key specified
        /// </summary>
        static void Decrease(string key)
        {
            // Populate
            if (!Data.ContainsKey(key))
                return;
            
            DataObject o = (DataObject)Data[key];
            o.Decrease();
        }

        /// <summary>
        /// Increase the Value field by 1 of the 
        /// data object stored with the key specified
        /// </summary>
        static void Increase(string key)
        {
            // Populate
            if (!Data.ContainsKey(key))
                return;
            
            DataObject o = (DataObject)Data[key];
            o.Increase();
        }


        /// <summary>
        /// Prints the information in the Data hashtable to the console.
        /// Output should be sorted by key 
        /// References should be printed between '<' and '>'
        /// The output should look like the following : 
        /// 
        /// 
        /// Alpha...... -3
        /// Bravo...... 2
        /// Charlie.... <Zulu>
        /// Delta...... 1
        /// Echo....... <Alpha>
        /// --etc---
        /// 
        /// </summary>
        static void PrintSortedData()
        {
            // Populate
            ArrayList keys = new ArrayList(Data.Keys);
            keys.Sort();
            foreach (string key in keys)
            {
                DataObject o = (DataObject)Data[key];
                string valueOrRef = o.IsRef() ? string.Format("<{0}>", o.reference.key) : o.value.ToString();
                string output = string.Format("{0} {1}", o.key.PadRight(11, '.'), valueOrRef);

                Console.Write(output);
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        /// <summary>
        /// Prints the information in the Data hashtable to the console.
        /// Output should be sorted by stored value
        /// References should be printed between '<' and '>'
        /// Sorting order start from max to min, larger value takes priority.
        /// The output should look like the following : 
        /// 
        /// 
        /// Bravo...... 100
        /// Echo...... 99
        /// Zulu...... 98
        /// Charlie.... <Zulu>
        /// Delta...... 34
        /// Echo....... 33
        /// Alpha...... <Echo>
        /// --etc---
        /// 
        /// </summary>
        static void PrintSortedDataByValue()
        {
            // Populate
            List<DataObject> values = new List<DataObject>(Data.Values.Cast<DataObject>());
            values.Sort();
            values.Reverse();
            
            for (int i = 0; i < values.Count; i++)
            {
                DataObject o = values[i];
                if (!o.IsRef())
                    continue;

                int index = values.IndexOf(o.reference);
                if (index > i)
                    i--;

                values.Remove(o);
                values.Insert(index + 1, o);
            }

            foreach (DataObject o in values)
            {
                string valueOrRef = o.IsRef() ? string.Format("<{0}>", o.reference.key) : o.value.ToString();
                string output = string.Format("{0} {1}", o.key.PadRight(11, '.'), valueOrRef);

                Console.Write(output);
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
