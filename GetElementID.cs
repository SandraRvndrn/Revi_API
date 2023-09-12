using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace RevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class GetElementID: IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
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

                //Get Element Type
                ElementId eTypeId = ele.GetTypeId();
                ElementType eType = doc.GetElement(eTypeId) as ElementType;

                //Display Element ID
                if (pickObj != null)
                {
                    TaskDialog.Show("Element Classification", eleid.ToString()+Environment.NewLine
                        + " Category : " + ele.Category.Name +Environment.NewLine 
                        + " Instance : "+ ele.Name +Environment.NewLine 
                        + " Family symbol : "+ eType.Name +Environment.NewLine
                        + " Family : "+ eType.FamilyName);
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
