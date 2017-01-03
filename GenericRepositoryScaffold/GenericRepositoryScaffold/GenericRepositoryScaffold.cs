using GenericRepositoryScaffold.Helper;
using GenericRepositoryScaffold.UI;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.AspNet.Scaffolding.NuGet;
using NuGet;

namespace GenericRepositoryScaffold
{
    public class GenericRepositoryScaffold : CodeGenerator
    {
        CustomViewModel _viewModel;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public GenericRepositoryScaffold(
            CodeGenerationContext context,
            CodeGeneratorInformation information)
            : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }

        public override IEnumerable<NuGetPackage> Dependencies
        {
            get
            {
                try
                {
                    List<NuGetPackage> t = new List<NuGetPackage>();
                    string[] packageIds = new string[] { "EntityFramework", "structuremap" };
                    IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
                    foreach (var packageId in packageIds)
                    {
                        var packageVersion = repo.FindPackagesById(packageId).Where(p => p.IsLatestVersion).Select(p => p.Version.ToString()).Max();
                        NuGetPackage package = new NuGetPackage(packageId,
                                                packageVersion,
                                                new NuGetSourceRepository("https://packages.nuget.org/api/v2"));
                        t.Add(package);
                    }
                    return t;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Any UI to be displayed after the scaffolder has been selected from the Add Scaffold dialog.
        /// Any validation on the input for values in the UI should be completed before returning from this method.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            // Bring up the selection dialog and allow user to select a model type
            //SelectModelWindow window = new SelectModelWindow(_viewModel);
            //bool? showDialog = window.ShowDialog();
            //return showDialog ?? false;
            return true;
        }

        /// <summary>
        /// This method is executed after the ShowUIAndValidate method, and this is where the actual code generation should occur.
        /// In this example, we are generating a new file from t4 template based on the ModelType selected in our UI.
        /// </summary>
        public override void GenerateCode()
        {
            try
            {
                // Get the selected code type
                //var codeType = _viewModel.SelectedModelType.CodeType;

                string unitOfWorkClassName = "UnitOfWork";
                string unitOfWorkInterfaceName = "IUnitOfWork";
                string genericRepositoryClassName = "GenericRepository";
                string genericRepositoryInterfaceName = "IGenericRepository";
                string structureMapClassName = "StructureMapConfigurator";
                string unitOfWorkTemplateName = "UnitOfWorkPattern";
                string unitOfWorkInterfaceTemplateName = "IUnitOfWorkPattern";
                string genericRepositoryTemplateName = "GenericRepository";
                string genericRepositoryInterfaceTemplateName = "IGenericRepository";
                string structureMapTemplateName = "StructureMapConfigurator";
                string projectNameSpace = Context.ActiveProject.Name;
                string classNameSpace = projectNameSpace + ".Repository.Models";
                string interfaceNameSpace = projectNameSpace + ".Repository.Interfaces";
                string structureMapNameSpace = projectNameSpace + ".Infrastructure";
                var selectionRelativePath = GetSelectedRelativePath();
                var project = Context.ActiveProject;

                // Setup the scaffolding item creation parameters to be passed into the T4 template.
                var unitOfWorkParameters = new Dictionary<string, object>()
            {
                { "ClassName", unitOfWorkClassName },
                { "InterfaceName", unitOfWorkInterfaceName },
                { "RepositoryInterfaceName", genericRepositoryInterfaceName },
                { "RepositoryInterfaceNamespace", interfaceNameSpace },
                { "NameSpace", classNameSpace },
                { "StructureMapNameSpace", structureMapNameSpace },
                { "StructureMapClassName", structureMapClassName }
            };

                var unitOfWorkInterfaceParameters = new Dictionary<string, object>()
            {
                { "InterfaceName", unitOfWorkInterfaceName },
                { "RepositoryInterfaceName", genericRepositoryInterfaceName },
                { "RepositoryInterfaceNamespace", interfaceNameSpace },
                { "NameSpace", classNameSpace }
            };

                var genericRepositoryParameters = new Dictionary<string, object>()
            {
                { "ClassName", genericRepositoryClassName },
                { "InterfaceName", genericRepositoryInterfaceName },
                { "InterfaceNameSpace", interfaceNameSpace },
                { "NameSpace", classNameSpace }
            };

                var genericRepositoryInterfaceParameters = new Dictionary<string, object>()
            {
                { "InterfaceName", genericRepositoryInterfaceName },
                { "NameSpace", interfaceNameSpace }
            };

                var structureMapParameters = new Dictionary<string, object>()
            {
                { "ClassName", structureMapClassName },
                { "UnitOfWorkClassName", unitOfWorkClassName },
                { "UnitOfWorkInterfaceName", unitOfWorkInterfaceName },
                { "RepositoryClassName", genericRepositoryClassName },
                { "RepositoryInterfaceName", genericRepositoryInterfaceName },
                { "RepositoryInterfaceNamespace", interfaceNameSpace },
                { "RepositoryClassNamespace", classNameSpace },
                { "NameSpace", structureMapNameSpace }
            };

                var classTemplatesPath = "Repository\\Models";
                var interfaceTemplatesPath = "Repository\\Interfaces";
                var structureMapTemplatesPath = "Infrastructure";

                var unitOfWorkTemplatePath = Path.Combine(classTemplatesPath, unitOfWorkTemplateName);
                var unitOfWorkInterfaceTemplatePath = Path.Combine(interfaceTemplatesPath, unitOfWorkInterfaceTemplateName);
                var genericRepositoryTemplatePath = Path.Combine(classTemplatesPath, genericRepositoryTemplateName);
                var genericRepositoryInterfaceTemplatePath = Path.Combine(interfaceTemplatesPath, genericRepositoryInterfaceTemplateName);
                var structureMapTemplatePath = Path.Combine(structureMapTemplatesPath, structureMapTemplateName);

                //var path = Path.Combine(Path.GetDirectoryName(),);

                AddFolderIfNotExist(project, classTemplatesPath);
                AddFolderIfNotExist(project, interfaceTemplatesPath);

                // Add the custom scaffolding item from T4 template.
                AddFileFromTemplate(
                    project: project,
                    outputPath: unitOfWorkTemplatePath,
                    templateName: unitOfWorkTemplateName,
                    templateParameters: unitOfWorkParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: unitOfWorkInterfaceTemplatePath,
                    templateName: unitOfWorkInterfaceTemplateName,
                    templateParameters: unitOfWorkInterfaceParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: genericRepositoryTemplatePath,
                    templateName: genericRepositoryTemplateName,
                    templateParameters: genericRepositoryParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: genericRepositoryInterfaceTemplatePath,
                    templateName: genericRepositoryInterfaceTemplateName,
                    templateParameters: genericRepositoryInterfaceParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: structureMapTemplatePath,
                    templateName: structureMapTemplateName,
                    templateParameters: structureMapParameters,
                skipIfExists: false);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string GetSelectedRelativePath()
        {
            return Context.ActiveProjectItem == null ? string.Empty : ProjectItemHelper.GetProjectRelativePath(Context.ActiveProjectItem);
        }

        private string GetDefaultNamespace()
        {
            return Context.ActiveProjectItem == null
                ? Context.ActiveProject.GetDefaultNamespace()
                : Context.ActiveProjectItem.GetDefaultNamespace();
        }

        private void AddFolderIfNotExist(EnvDTE.Project project, string projectRelativePath)
        {
            try
            {
                var projectPath = project.GetFullPath();
                if (!Directory.Exists(Path.Combine(projectPath, @projectRelativePath)))
                {
                    AddFolder(project, projectRelativePath);
                }
                var pCollection = new ProjectCollection();
                var p = pCollection.LoadProject(@project.FullName);
                p.AddItem("Folder", @projectRelativePath);
                p.Save();
                pCollection.UnloadProject(p);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
