using GenericRepositoryScaffold.UI;
using Microsoft.AspNet.Scaffolding;
using System.Collections.Generic;

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
            // Get the selected code type
            //var codeType = _viewModel.SelectedModelType.CodeType;

            string baseClassName = "BaseGenericRepository";
            string childClassName = "GenericRepository";
            string interfaceName = "IGenericRepository";
            string projectNameSpace = Context.ActiveProject.Name;
            string classNameSpace = projectNameSpace + ".Models";
            string interfaceNameSpace = projectNameSpace + ".Interfaces";

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

            this.AddFolder(Context.ActiveProject, "Models");
            this.AddFolder(Context.ActiveProject, "Interfaces");

            // Add the custom scaffolding item from T4 template.
            this.AddFileFromTemplate(Context.ActiveProject,
                "Models",
                "GenericRepositoryTemplate",
                childParameters,
                skipIfExists: false);

            this.AddFileFromTemplate(Context.ActiveProject,
                "Models",
                "BaseGenericRepositoryTemplate",
                baseParameters,
                skipIfExists: false);

            this.AddFileFromTemplate(Context.ActiveProject,
                "Interfaces",
                "IGenericRepositoryTemplate",
                interfaceParameters,
                skipIfExists: false);
        }


    }
}
