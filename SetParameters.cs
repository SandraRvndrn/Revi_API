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
    class SetParameters : IExternalCommand
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

                //Display Element ID
                if (pickObj != null)
                {
                    //Retrive Element
                    ElementId eleid = pickObj.ElementId;
                    Element ele = doc.GetElement(eleid);

                    //Get Parameter Value
                    Parameter param = ele.get_Parameter(BuiltInParameter.INSTANCE_HEAD_HEIGHT_PARAM);

                    TaskDialog.Show("Parameter Value", string.Format("Parameter storage type {0} and value {1}",
                        param.StorageType.ToString(),
                        param.AsDouble()));


                    //Set Parameter Value
                    using (Transaction trans = new Transaction(doc, "Set Parameter"))
                    {
                        trans.Start();

                        param.Set(7.5);

                        trans.Commit();
                    }
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
