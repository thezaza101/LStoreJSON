using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LStoreJSON
{
    public class JSONStore
    {
        //For IO oprations
        private StreamReader sr;
        private StreamWriter sw;
        private string dbPath;

        //for internal oprations
        private Dictionary<string, List<object>> inMemoryDB;
        private HashSet<Type> changedObjects;
        public JSONStore(string reletiveDatabasePath = "database\\")
        {
            dbPath = reletiveDatabasePath;

            inMemoryDB = new Dictionary<string, List<object>>();
            changedObjects = new HashSet<Type>();

            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }
        }

        public void Add<T>(T o, bool skipValidation = true)
        {
            string fileName = o.GetType().ToString();
            List<T> existingElements = ReadObjects<T>();
            if(skipValidation)
            {
                inMemoryDB[fileName].Add(o);
                AddItemToSaveList(o);
            }
            else 
            {
                if (existingElements.FindIndex(e => e.GetKeyValeue<T>().Equals(o.GetKeyValeue<T>())) >= 0)
                {
                    throw new Exception("Key \"" + o.GetKeyValeue<T>().ToString() + "\" already exists");
                }
                else
                {
                    inMemoryDB[fileName].Add(o);
                    AddItemToSaveList(o);
                }
            }
            
        }

        public void Remove<T>(T o)
        {
            string fileName = o.GetType().ToString();
            List<T> existingElements = ReadObjects<T>();
            if (existingElements.FindIndex(e => e.GetKeyValeue<T>().Equals(o.GetKeyValeue<T>())) >= 0)
            {
                existingElements.RemoveAll(e => e.GetKeyValeue<T>().Equals(o.GetKeyValeue<T>()));
                AddItemToSaveList(o);
            }
            else
            {
                throw new Exception("Key \"" + o.GetKeyValeue<T>().ToString() + "\" does not exist");
            }
        }

        /// <summary>
        /// Commit the changes made to the database
        /// </summary>
        public void SaveChanges()
        {
            foreach (Type t in changedObjects)
            {
                SaveChanges(t);
            }
            changedObjects.Clear();
        }

        /// <summary>
        /// Commit the changes made to the database for the given type
        /// </summary>
        public void SaveChanges(Type t)
        {
            string fileName = t.ToString();
            string filePath = dbPath + fileName;

            if (!inMemoryDB.ContainsKey(fileName))
            {
                throw new KeyNotFoundException("No changes to key \"" + fileName + "\" found");
            }
            else
            {
                using (sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(inMemoryDB[fileName]));
                }
            }
        }

        /// <summary>
        /// Returns an object of the supplied type given the object contains a key
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam> 
        /// <returns>Object of the supplied type and key</returns>
        public T Single<T>(object Id)
        {
            return ReadObjects<T>().Find(e => e.GetKeyValeue<T>().ToString().Equals(Id));
        }



        /// <summary>
        /// Returns all the objects stored of the supplied type
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam> 
        /// <returns>List of the data for the given type</returns>
        public List<T> All<T>()
        {
            return ReadObjects<T>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IReadOnlyCollection<T> InnerList<T>()
        {
            return ReadObjects<T>().AsReadOnly();
        }

        private List<T> ReadObjects<T>()
        {
            string fileName = typeof(T).ToString();
            string filePath = dbPath + fileName;

            if (inMemoryDB.ContainsKey(fileName))
            {
                return inMemoryDB[fileName].ConvertObjectToGenericType<T>();
            }
            else
            {
                string data;
                try
                {
                    using (sr = new StreamReader(filePath, Encoding.UTF8))
                    {
                        data = sr.ReadToEnd();
                    }
                }
                catch (FileNotFoundException)
                {
                    data = "";
                }
                List<T> dataList = string.IsNullOrWhiteSpace(data) ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(data);
                inMemoryDB.Add(fileName, dataList.ConvertGenericTypeToObjectList<T>());
                return dataList;
            }
        }

        private void AddItemToSaveList(object inType)
        {
            changedObjects.Add(inType.GetType());
        }

        /// <summary>
        /// Determines if the type of object is saveable via the JSONStore class
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam> 
        /// <returns>Is the supplied type saveable</returns>
        public static bool IsTypeSaveable<T>()
        {
            bool output = false;
            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                if (Attribute.IsDefined(info, typeof(System.ComponentModel.DataAnnotations.KeyAttribute)))
                {
                    output = true;
                    break;
                }
            }
            return output;
        }
    }
}