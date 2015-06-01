using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace TextAdornmentTest1
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    class TextViewCreation : IWpfTextViewCreationListener
    {
        private DTE dte;


        private Queue<Tuple<int, string>> FunctionQ = new Queue<Tuple<int, string>>();
        public void TextViewCreated(IWpfTextView textView)
        {
            CreateFunctionsQ();
        }
        public void CreateFunctionsQ()
        {
            foreach (CodeElement codeElement in dte.ActiveDocument.ProjectItem.FileCodeModel.CodeElements)// search for the function to be opened
            {
                // get the namespace elements
                if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                {
                    foreach (CodeElement namespaceElement in codeElement.Children)
                    {
                        // get the class elements
                        if (namespaceElement.Kind == vsCMElement.vsCMElementClass || namespaceElement.Kind == vsCMElement.vsCMElementInterface)
                        {
                            FunctionQ = new Queue<Tuple<int, string>>();
                            foreach (CodeElement classElement in namespaceElement.Children)
                            {
                                try
                                {

                                    // get the function elements to highlight methods in code window
                                    if (classElement.Kind == vsCMElement.vsCMElementFunction)
                                    {

                                        FunctionQ.Enqueue(new Tuple<int, string>(classElement.StartPoint.Line, classElement.Name));
                                        //str += "FunctionName: " + classElement.Name + " LineNumber: " + classElement.StartPoint.Line + Environment.NewLine;
                                        //File.WriteAllText("C:\\debug.txt", str);
                                        //Console.WriteLine(classElement.Name + "=>Line No: " + classElement.StartPoint.Line);
                                        //this.CreateVisuals(_view.TextViewLines[classElement.StartPoint.Line]);



                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }


        }
    }
}
