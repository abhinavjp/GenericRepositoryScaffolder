# Unit Of Work Pattern with Generic Repository

A scaffolder which generates generic repository for entity framework with Unit Of Work pattern

## Getting Started

The Project and the vsix are both available here.
As this is an open source project, the project can be modified according to custom requirements.

### Installing
Note:	This project also uses latest version of structuremap and entity framework.
		So a file for structuremap is automatically created which maps the classes with interfaces for dependency injection.

 1. Just download the vsix extension and then install it. (Works only with Visual Studio 2015. Untested in other versions.)
 2. Right click on any project of an existing solution then click on Add, and click on New Scaffolded Item.
 3. Select Generic Scaffolder 
 4. Thats it! All files are automatically generated!

## How to start after installing

You can now use your dbset of entity framework in the manner described below:

```
private readonly IUnitOfWork<Entity_Context> _unitOfWork = GetInstance<IUnitOfWork<Entity_Context>>(args);;
private readonly IGenericRepository<Entity_Name, Entity_Context> _entityRepository = _unitOfWork.GetRepository<Entity_Name, Entity_Context>();;
```
Entity_Context is the Entity Framework Context and Entity_Name is the name of class created when adding an EDMX.

## Authors

* **Abhinav Pisharody** - *Initial work* - [abhinavjp](https://github.com/abhinavjp)

## Acknowledgments

* Understanding from [CodeProject](https://www.codeproject.com/articles/581487/unit-of-work-design-pattern)
* Dependency Injection using [StructureMap](http://structuremap.github.io/)
* Learnt to make scaffolder with the help of [MSDN Blog](https://blogs.msdn.microsoft.com/webdev/2014/04/03/creating-a-custom-scaffolder-for-visual-studio/) By [Joost de Nijs](https://social.msdn.microsoft.com/profile/Joost+de+Nijs)
