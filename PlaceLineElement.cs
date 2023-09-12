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
    class PlaceLineElement : IExternalCommand
    {
        // In this I Learned a different way to place an element such as using lines by creating walls directly in Revit command
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document 
            Document doc = uidoc.Document;

            //Creating the level
            Level level = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .First(x => x.Name == "Level 1");


            //Create Points
            XYZ p1 = new XYZ(-10,-10,0);
            XYZ p2 = new XYZ(10,-10,0);
            XYZ p3 = new XYZ(15,0,0);
            XYZ p4 = new XYZ(10,10,0);
            XYZ p5 = new XYZ(-10,10,0);

            //Create Curves
            List<Curve> curves = new List<Curve>();
            Line l1 = Line.CreateBound(p1,p2);
            Arc l2 = Arc.Create(p2, p4,p3);
            Line l3 = Line.CreateBound(p4,p5);
            Line l4 = Line.CreateBound(p5,p1);

            //Appending them to the curves list using add method
            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            // using WallCreate method to create the walls

            try
            {
                using (Transaction trans = new Transaction(doc, "Place Family"))
                {
                    trans.Start();
                    
                    foreach(Curve c in curves)
                    {
                        Wall.Create(doc, c, level.Id, false);
                    }

                    trans.Commit();
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
