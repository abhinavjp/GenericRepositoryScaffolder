using GenericRepositoryScaffold.Helper;
using GenericRepositoryScaffold.UI;
using Microsoft.AspNet.Scaffolding;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.AspNet.Scaffolding.NuGet;

namespace GenericRepositoryScaffold
{
    public class CustomCodeGenerator : CodeGenerator
    {
        CustomViewModel _viewModel;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public CustomCodeGenerator(
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
                    NuGetPackage demoPackage = new NuGetPackage("EntityFramework",
                                            "6.1.3",
                                            new NuGetSourceRepository("https://packages.nuget.org/api/v2"));

                    var nugetService = (INuGetService)Context.ServiceProvider.GetService(typeof(INuGetService));
                    nugetService.InstallPackage(Context.ActiveProject, demoPackage);
                    t.Add(demoPackage);
                    return (IEnumerable<NuGetPackage>)t;

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

                string baseClassName = "BaseGenericRepository";
                string childClassName = "GenericRepository";
                string interfaceName = "IGenericRepository";
                string baseClassTemplateName = "BaseGenericRepository";
                string childClassTemplateName = "GenericRepository";
                string interfaceTemplateName = "IGenericRepository";
                string projectNameSpace = Context.ActiveProject.Name;
                string classNameSpace = projectNameSpace + ".Models";
                string interfaceNameSpace = projectNameSpace + ".Interfaces";
                var selectionRelativePath = GetSelectedRelativePath();
                var project = Context.ActiveProject;

                // Setup the scaffolding item creation parameters to be passed into the T4 template.
                var baseParameters = new Dictionary<string, object>()
            {
                { "ClassName", baseClassName },
                { "NameSpace", classNameSpace }
            };

                var childParameters = new Dictionary<string, object>()
            {
                { "ClassName", childClassName },
                { "ParentClassName", baseClassName },
                { "InterfaceName", interfaceName },
                { "NameSpace", classNameSpace }
            };

                var interfaceParameters = new Dictionary<string, object>()
            {
                { "InterfaceName", interfaceName },
                { "NameSpace", interfaceNameSpace }
            };

                var classTemplatesPath = "Repository\\Models";
                var interfaceTemplatesPath = "Repository\\Interfaces";

                var baseClassTemplatePath = Path.Combine(classTemplatesPath, baseClassTemplateName);
                var childClassTemplatePath = Path.Combine(classTemplatesPath, childClassTemplateName);
                var interfaceTemplatePath = Path.Combine(interfaceTemplatesPath, interfaceTemplateName);

                //var path = Path.Combine(Path.GetDirectoryName(),);

                AddFolderIfNotExist(project, classTemplatesPath);
                AddFolderIfNotExist(project, interfaceTemplatesPath);

                // Add the custom scaffolding item from T4 template.
                AddFileFromTemplate(
                    project: project,
                    outputPath: baseClassTemplatePath,
                    templateName: baseClassTemplateName,
                    templateParameters: baseParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: childClassTemplatePath,
                    templateName: childClassTemplateName,
                    templateParameters: childParameters,
                skipIfExists: false);

                AddFileFromTemplate(
                    project: project,
                    outputPath: interfaceTemplatePath,
                    templateName: interfaceTemplateName,
                    templateParameters: interfaceParameters,
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
