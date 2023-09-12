using Aspose.Cells.Charts;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCommands
{
    //2023 newfloow method is not available
    [TransactionAttribute(TransactionMode.Manual)]
    class PlaceLoopElement : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //creating a varaible for typeid
            ForgeTypeId ft = null;
            //List for curve
            

            var view = uidoc.ActiveView;
            ElementId floor_id = null;

            //Get Document
            Document doc = uidoc.Document;

            //Creating Points
            XYZ p1 = new XYZ(-10,-10,0);
            XYZ p2 = new XYZ(10,-10,0);
            XYZ p3 = new XYZ(15,0,0);
            XYZ p4 = new XYZ(10,10,0);
            XYZ p5 = new XYZ(-10,10,0);

            //Create Curves
            List<Curve> curves = new List<Curve>();
            Line l1 = Line.CreateBound(p1, p2);
            Arc l2 = Arc.Create(p2, p4, p3);
            Line l3 = Line.CreateBound(p4, p5);
            Line l4 = Line.CreateBound(p5, p1);

            //Appending them to the curves list using add method
            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            //Create Curve Loop
            CurveLoop crvLoop = CurveLoop.Create(curves);
            CurveLoop offsetcrv = CurveLoop.CreateViaOffset(crvLoop, 0.442913, new XYZ(0, 0, 1));

            CurveArray cArray = new CurveArray();
            List<CurveLoop> curve = new List<CurveLoop>();
            foreach(Curve c in offsetcrv)
            {
                curve.Append(offsetcrv);
            }

            FilteredElementCollector filter_floor = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Floors)
                .WhereElementIsElementType();

            foreach (Element element in filter_floor)
            {
                if (element.Name == "Wood Joist 10\" - Wood Finish")
                {
                    floor_id = element.Id;
                    break;
                }
            }

            //Creating a levelid
            Level level = view.GenLevel;
            ElementId level_id = level.Id;

            try
            {
                using(Transaction trans = new Transaction(doc,"Place Family "))
                {
                    trans.Start();

                    Autodesk.Revit.DB.Floor.Create(doc, curve, floor_id, level_id);


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
