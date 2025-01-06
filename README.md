# SaveSystemForUnity  

## 🌟 About the Project  
This is a modular save system for Unity that supports:  
- Creating save slots.  
- Saving and loading data in JSON or Binary format.  
- Data encryption.  
- Backup file creation for added security.  

---

## 🔧 Features  
- Easy integration via interfaces and an event-driven system.  
- Simple customization with an editor extension for any project needs.  

---

## 📦 Installation  

1. Clone the repository or download the ZIP archive:  
   ```bash
   git clone https://github.com/your-username/Unity-Save-System.git
   ```  
2. Import the folder into your Unity project.  

---

## 🛠️ Setup  

1. Add the `SaveSystem` to your scenes.
2. Bind the `Event Bus` to Zenject container or simply create the its instance.
3. Call the `Contract` method of `SaveSystem`.

---
## 🚀 Usage  

### Creating New Save
```csharp
//you need an instance of EventBus
var eventBus = new EventBus();
//create params of new save
//only name of save is required param, other is optional
var saveParams = new SaveParams(
   "name of new save",
   "name of player",
   "some description"
   );
//send the request using event bus
//response returns bool value
var response = await eventBus.SendRequest(new CreateSaveRequest(saveParams));
```
### Creating Data Entity
To save data you need inherit data class from `SavableObject` or `MonoSavableObject` for MonoBehaviour objects.
```csharp
//inherit data class from `SavableObject`
public class FooData: SavableObject
{
   //to define which data to save, you need to add `[save]` attribute
   //you can save both fields and properties
   //fields can be public or private
   [save]
   private float _health;
   [save]
   private int _score;
   [save]
   private Vector3 _pos;
   //this field won't be saved
   private int _wontBeSaved;
}
```
### Getting Meta Data Of Save
After you creating new saves, you can get list of all meta data
```csharp
var response = await eventBus.SendRequest(new LoadSaveMetaRequest());
var metas = response.metas;
```
### Saving Data
After you've created data class you can save its data
```csharp
var saveMetas = await eventBus.sendRequest();
//get the guid of data save
var id = metas[0].id;
//send the requets using event bus
//response returns bool value
var response = await eventBus.SendRequest(new SaveDataToSaveRequest(id));
//data was saved
```
### Loading Data  
```csharp
var id = metas[0].id;
var response = await eventBus.SendRequest(new LoadDataFromSaveRequest(id));
``` 
---

## 📚 Documentation  

For more information, refer to:  
- [Code Examples](docs/examples.md)  
- [System Architecture](docs/architecture.md)  

---

## 📖 Roadmap  

- [ ] Add cloud and server save support.  

---

## 💬 Contact  

If you have any questions or suggestions, contact me:  
- Email: [erzumashev96+dev@gmail.com](erzumashev96+dev@gmail.com)  
- Telegram: [@your-handle](https://t.me/your-handle)  

---

## 📜 License  

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.  

---

## 🖼️ Screenshots  

Include a few screenshots showcasing your system in action.  
