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
    [TransactionAttribute(TransactionMode.ReadOnly)]
    class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //Creat Filtered Element Collector 
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            //Create Filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

            //Apply Filter
            IList<Element> windows = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            TaskDialog.Show("Winsows", string.Format("{0} windows counted!",windows.Count));

            return Result.Succeeded;
        }
    }
}
