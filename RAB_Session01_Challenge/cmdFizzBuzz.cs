#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RAB_Session01_Challenge
{
    [Transaction(TransactionMode.Manual)]
    public class cmdFizzBuzz : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // set the variables
            int range = 100;
            XYZ insPoint = new XYZ(0, 0, 0);
            double offsetValue = 0.05;
            double calcOffset = offsetValue * doc.ActiveView.Scale;
            XYZ offset = new XYZ(0, calcOffset, 0);
            string result = "";

            // create filtered element collector
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            // create the transaction
            Transaction t = new Transaction(doc);
            t.Start("Fizz Buzz");

            // create the loop
            for(int i = 1; i <= range; i++)
            {
                // check for fizz buzz
                if (i % 3 == 0 && i % 5 == 0)
                    result = "FIZZBUZZ";
                else if (i % 3 == 0)
                    result = "FIZZ";
                else if (i % 5 == 0)
                    result = "BUZZ";
                else
                    result = i.ToString();

                // create the text note
                TextNote curNote = TextNote.Create(doc, doc.ActiveView.Id,
                    insPoint, result, collector.FirstElementId());

                // increment the insertion point
                insPoint = insPoint.Subtract(offset);
            }

            // commit the transaction
            t.Commit();
            t.Dispose();

            return Result.Succeeded;
        }
    }
}
