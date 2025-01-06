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

### Creating new save
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
eventBus.SendRequest(new CreateSaveRequest(saveParams))
```  

### Loading Data  
```csharp
var data = SaveManager.Instance.Load<YourType>("key");
```  

### Deleting Save Data  
```csharp
SaveManager.Instance.Delete("key");
```  

---

## 📚 Documentation  

For more information, refer to:  
- [Code Examples](docs/examples.md)  
- [System Architecture](docs/architecture.md)  

---

## 📖 Roadmap  

- [ ] Add cloud save support.  
- [ ] Integrate with game achievements.  
- [ ] Optimize performance for handling large datasets.  

---

## 🤝 Contributing  

We welcome your ideas and contributions! To contribute:  
1. Fork the repository.  
2. Create a new branch:  
   ```bash
   git checkout -b feature/your-feature
   ```  
3. Commit your changes and open a pull request.  

---

## 💬 Contact  

If you have any questions or suggestions, feel free to reach out:  
- Email: [example@email.com](mailto:example@email.com)  
- Telegram: [@your-handle](https://t.me/your-handle)  

---

## 📜 License  

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.  

---

## 🖼️ Screenshots  

Include a few screenshots showcasing your system in action.  
