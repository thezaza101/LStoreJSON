# LStoreJSON

Provide a lightweight local JSON object store. Any class can be stored as long as there is one property with the [System.ComponentModel.DataAnnotations.Key] attribute.

## Dependencies
* Newtonsoft.Json
* System.ComponentModel.Annotations
 
 ## How to use
 ### Install the package

Package Manager:
```cmd
Install-Package LStoreJSON
```

.NET CLI
```cmd
dotnet add package LStoreJSON
```

Packet CLI
```cmd
paket add LStoreJSON
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

### Methods

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

## Making contributions
To propose a change, you first need to [create a GitHub account](https://github.com/join).

Once you're signed in, you can browse through the folders above and choose the content you're looking for. You should then see the content in Markdown form. Click the Edit icon in the top-right corner to start editing the content.

The content is written in the Markdown format. [There's a guide here on how to get started with it](https://guides.github.com/features/mastering-markdown/).

You can preview your changes using the tabs at the top of the editor.

When you're happy with your change, make sure to create a pull request for it using the options at the bottom of the page. You'll need to write a short description of the changes you've made.

A pull request is a proposal for a change to the content. Other people can comment on the change and make suggestions. When your change has been reviewed, it will be "merged" - and it will appear immediately in the published content.

Take a look at [this guide on GitHub about pull requests](https://help.github.com/articles/using-pull-requests/).




