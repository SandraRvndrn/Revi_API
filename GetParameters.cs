using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitCommands
{
    


    [TransactionAttribute(TransactionMode.Manual)]
    class GetParameters : IExternalCommand
    {
        // In this I Learned a different way to place an element such as using lines by creating walls directly in Revit command
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document 
            Document doc = uidoc.Document;

            try
            {
                //Pick object
                Reference pickObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                 //Retrive Element
                    ElementId eleid = pickObj.ElementId;
                    Element ele = doc.GetElement(eleid);

                    //Get Parameter
                    Parameter param = ele.LookupParameter("Head Height");     
                    InternalDefinition paramDef = param.Definition as InternalDefinition;

                //Display Element ID
                if (pickObj != null)
                {
                   

                    TaskDialog.Show("Parameters", string.Format("{0}  with builtinparameter {1}",
                        paramDef.Name,
                        //paramDef.UnitType, parameter of type {1}
                        paramDef.BuiltInParameter));
                }
                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
