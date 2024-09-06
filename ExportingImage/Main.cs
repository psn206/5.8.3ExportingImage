using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ExportingImage
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            var views = new FilteredElementCollector(doc)
                   .OfClass(typeof(View))
                   .Where(p => p.Name == "План 1 этажа")
                   .Cast<View>()
                   .ToList();

            List<ElementId> listID = new List<ElementId>();

            foreach (View view in views)
            {
                listID.Add(view.Id);
            }

            ImageExportOptions options = new ImageExportOptions();
            options.ExportRange = ExportRange.SetOfViews;
            options.PixelSize = 2400;
            options.SetViewsAndSheets(listID);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Файл NWC (*.png)|*.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                options.FilePath = saveFileDialog.FileName;
                options.ViewName = saveFileDialog.SafeFileName;
                doc.ExportImage(options);
                TaskDialog.Show("Сообщение", "Файл записан!");
            }
            return Result.Succeeded;
        }
    }
}
