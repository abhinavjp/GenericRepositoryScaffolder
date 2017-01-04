# Unit Of Work Pattern with Generic Repository

A scaffolder which generates generic repository for entity framework with Unit Of Work pattern
Note: Unfortunately scaffolder only works with ASP.Net projects.

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
Perform the steps mentioned below on the service you need to use the repository.  
Include the namespace of structuremap as below:
```
using static Your_StructureMap_Namespace_Created_By_Scaffolder.StructureMapConfigurator;
```

Also there is a need for `StructureMap.Pipeline` namespace used when a parameterized constructor is in use and hence also include that namespace:
```
using StructureMap.Pipeline;
```

You can now use your dbset of entity framework in the manner described below.  
For Example there is an entity named `Employee` and the context name is `EmployeeSystemEntities`:

```
private EmployeeSystemEntities _context = new EmployeeSystemEntities();
var args = new ExplicitArguments();
args.Set(_context);
private readonly IUnitOfWork<EmployeeSystemEntities> _unitOfWork = GetInstance<IUnitOfWork<EmployeeSystemEntities>>(args);
private readonly IGenericRepository<Employee, EmployeeSystemEntities> _entityRepository = _unitOfWork.GetRepository<Employee>();
```
Unit of work pattern needs to be declared only once for all repositories in a specific class.

## Authors

* **Abhinav Pisharody** - *Initial work* - [GithubProfile](https://github.com/abhinavjp)
* **Salim Tamimi** - *Unit Of Work Pattern Guidance* - [LinkedInProfile](https://www.linkedin.com/in/salim-tamimi-a-40761837)
* **Milan Vadaliya** - *Helping Hand* - [LinkedInProfile](https://in.linkedin.com/in/milan-vadaliya-6271535a)

## Acknowledgments

* Understanding of unit of work pattern from [CodeProject](https://www.codeproject.com/articles/581487/unit-of-work-design-pattern)
* Dependency Injection using [StructureMap](http://structuremap.github.io/)
* Learnt to make scaffolder with the help of a [MSDN Blog](https://blogs.msdn.microsoft.com/webdev/2014/04/03/creating-a-custom-scaffolder-for-visual-studio/) By [Joost de Nijs](https://social.msdn.microsoft.com/profile/Joost+de+Nijs)
