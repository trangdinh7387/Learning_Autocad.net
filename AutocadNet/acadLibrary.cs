using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using AcadGeo = Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Geometry;

namespace acadFunSpace
{
	public class acadFuncs
	{
		static public Database GetActiveDb()
		{
			return AcadApp.Application.DocumentManager.MdiActiveDocument.Database;
		}

		static public Editor GetEditor()
		{
			return AcadApp.Application.DocumentManager.MdiActiveDocument.Editor;
		}

		static BlockTable GetBlkTbl(Transaction acTrans)
		{
			return acTrans.GetObject(GetActiveDb().BlockTableId, OpenMode.ForRead) as BlockTable;
		}

		static public BlockTableRecord getModelSpace(Transaction acTrans)
		{
			return acTrans.GetObject(GetBlkTbl(acTrans)[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
		}

		static public void AddNewEnt(Entity ent)
		{
			Database db = GetActiveDb();

			using (Transaction acTrans = db.TransactionManager.StartTransaction())
			{
				BlockTableRecord model_space = getModelSpace(acTrans);
				if (null == model_space)
					return;

				model_space.UpgradeOpen();
				model_space.AppendEntity(ent);
				acTrans.AddNewlyCreatedDBObject(ent, true);

				acTrans.Commit();
			}
		}

        static public SelectionSet getSelectionset(string strObjName)
        {
            Editor acEditor = GetEditor();
            TypedValue[] acTypValAr = new TypedValue[1];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, strObjName), 0);
            SelectionFilter acFilters = new SelectionFilter(acTypValAr);
            PromptSelectionResult acSSPrompt = acEditor.GetSelection(acFilters);
            if (acSSPrompt.Status == PromptStatus.OK)
            {
                return acSSPrompt.Value;
            }
            else return null;
        }
	}
}
