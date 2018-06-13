# LStoreJSON

Provide a lightweight local JSON object store. Any class can be stored as long as there is one property with the [System.ComponentModel.DataAnnotations.Key] attribute.

## Dependencies
* Newtonsoft.Json
* System.ComponentModel.Annotations
 
 ## How to use
 ### Install the package

Package Manager:
```cmd
Install-Package LStoreJSON -Version 1.0.0 
```

.NET CLI
```cmd
dotnet add package LStoreJSON --version 1.0.0 
```

Packet CLI
```cmd
paket add LStoreJSON --version 1.0.0 
```

### Using

```cmd
class MyClass
{
    [System.ComponentModel.DataAnnotations.Key]
    public string key {get; set;} 
    public int someVar
}
...
using LStoreJSON;
…
//Saving to file
JSONStore js = new JSONStore();
MyClass myObject = new MyClass(){key = “Some Key”};
js.Add(myObject);
js.SaveChanges();

//Retrieving from file
MyClass objectFromFile = js.Single<MyClass>(“Some Key”);
```

### More info

```cmd
//add item of type to in memory store
jsonStoreObject.Add<T>(T o)

//remove item of type to in memory store
jsonStoreObject.Remove<T>(T o)

//Save changes present from in memory store to files
jsonStoreObject.SaveChanges()

//Returns all the objects of a type
jsonStoreObject.All<T>()

//Returns the object of a given type with a matching ID
jsonStoreObject.Single<T>(object Id)

//Determines if the supplied type can be saved using a JSONStore object
JSONStore.IsTypeSaveable<T>()

```


