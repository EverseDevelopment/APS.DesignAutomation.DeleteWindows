using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.IO;

namespace DeleteWindows
{
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   public class DeleteWindowsApp : IExternalCommand
   {

      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         var app = commandData.Application.Application;
         var doc = commandData.Application.ActiveUIDocument?.Document;
         DeleteAllWindows(app, doc);
         return Result.Succeeded;
      }

      public static void DeleteAllWindows(Application rvtApp, Document doc)
      {
         if (rvtApp == null) throw new InvalidDataException(nameof(rvtApp));

         if (doc == null) throw new InvalidOperationException("Could not open document.");

         using (Transaction transaction = new Transaction(doc))
         {
            var windows = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Windows)
                .WhereElementIsNotElementType();

                transaction.Start("Delete All Windows");
            doc.Delete(windows.ToElementIds());
            transaction.Commit();
         }
      }
   }
}
