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
    class ChangeLocation : IExternalCommand
    {
        // In this I Learned a different way to place an element such as using lines by creating walls directly in Revit command
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument and Document 
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                //Pick object
                Reference pickObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Display Element ID
                if (pickObj != null)
                {
                    //Retrieve Element
                    ElementId eleid = pickObj.ElementId;
                    Element ele = doc.GetElement(eleid);

                    using (Transaction trans = new Transaction(doc, "Change Location"))
                    {
                        trans.Start();

                        //Set Location
                      
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
