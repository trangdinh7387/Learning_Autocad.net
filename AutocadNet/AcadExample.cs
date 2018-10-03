using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Add thư viện tham khảo của Autocad
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using acadFunSpace;
#endregion
namespace AcadUtilities
{
	public class AcadExample
	{
        private static string LAYER_NAME = "MYLAYER";
		#region Vẽ đường Line
		[CommandMethod("DrawLine")]
		public void DrawLine()
		{
            //define 2 points
            Point3d spoint = new Point3d(0, 0, 0);
            Point3d epoint = new Point3d(100, 100, 0);
            acadFuncs.AddNewEnt(new Line(spoint, epoint));
        }
		#endregion
		//#region Vẽ đường tròn
		//[CommandMethod("DrawCircle")uh?
		//public void DrawCirlce()
		//{
		//	//Get curren drawing and database
		//	using (Transaction acTrans = GetCurDb().TransactionManager.StartTransaction())
		//	{
		//		BlockTableRecord acModel = GetModelSpace(acTrans);
		//		if (null == acModel)
		//			return;
		//		Point3d CenterPoint = new Point3d(0, 0, 0);
		//		double Radius = 5;

		//		Circle objCircle = new Circle();
		//		objCircle.Center = CenterPoint;
		//		objCircle.Radius = Radius;
		//		{
		//			//get layer here
		//		}
		//		ObjectId layer_id = GetLayerByName(LAYER_NAME);
		//		if(ObjectId.Null == layer_id)
		//		{
		//			acTrans.Dispose();
		//			return;
		//		}
		//		objCircle.SetLayerId(layer_id, true);
		//		acModel.UpgradeOpen();
		//		acModel.AppendEntity(objCircle);

		//		acTrans.AddNewlyCreatedDBObject(objCircle, true);
		//		acTrans.Commit();
		//	}
		//}

		//private ObjectId GetLayerByName(string strLayerName)
		//{
		//	using (Transaction acTrans = GetCurDb().TransactionManager.StartTransaction())
		//	{
		//		//Open the Layer Table for read
		//		LayerTable acLayerTbl = acTrans.GetObject(GetCurDb().LayerTableId, OpenMode.ForRead) as LayerTable;
		//		if (null == acLayerTbl)
		//			return ObjectId.Null;

		//		//ktra xem Layer có tồn tại chưa
		//		if (false == acLayerTbl.Has(strLayerName))
		//		{
		//			using (LayerTableRecord acLayerRec = new LayerTableRecord())
		//			{
		//				acLayerRec.Name = strLayerName;
		//				acLayerRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);

		//				acLayerTbl.UpgradeOpen();
		//				acLayerTbl.Add(acLayerRec);
		//				acTrans.AddNewlyCreatedDBObject(acLayerRec, true);
		//				acTrans.Commit();

		//				return acLayerRec.Id;
		//			}
		//		}
		//		else
		//		{
		//			LayerTableRecord layer_tbl_rcd = acTrans.GetObject(acLayerTbl[strLayerName], OpenMode.ForRead) as LayerTableRecord;
		//			if (null != layer_tbl_rcd)
		//				return layer_tbl_rcd.Id;
		//			else
		//				return ObjectId.Null;
		//		}

		//	}
		//}

		//#endregion
		//#region Tạo Layer
		//[CommandMethod("MyLayer")]
		//public void NewLayer()
		//{
		//	//Get curren drawing and database
		//	using (Transaction acTrans = GetCurDb().TransactionManager.StartTransaction())
		//	{
		//		//Open the Layer Table for read
		//		LayerTable acLayerTbl = acTrans.GetObject(GetCurDb().LayerTableId, OpenMode.ForRead) as LayerTable;
		//		if (null == acLayerTbl)
		//			return;

		//		string strLayerName = "MyLayer";

		//		//ktra xem Layer có tồn tại chưa
		//		if (false == acLayerTbl.Has(strLayerName))
		//		{
		//			using (LayerTableRecord acLayerRec = new LayerTableRecord())
		//			{
		//				acLayerRec.Name = strLayerName;
		//				acLayerRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);

		//				acLayerTbl.UpgradeOpen();
		//				acLayerTbl.Add(acLayerRec);
		//				acTrans.AddNewlyCreatedDBObject(acLayerRec, true);
		//			}
		//		}
		//		else
		//		{
		//			LayerTableRecord layer_tbl_rcd = acTrans.GetObject(acLayerTbl["MyLayer"], OpenMode.ForRead) as LayerTableRecord;
		//		}

		//		acTrans.Commit();
		//	}
		//}
  //      #endregion
        #region Get integer or Keyword from User
        [CommandMethod("GetIntegerFromUser")]
        public void Getinteger_keywordfromuser()
        {
            Editor acEditor = acadFuncs.GetEditor();
            PromptIntegerOptions prIntOpt = new PromptIntegerOptions("");
            prIntOpt.Message = "\nEnter integer Number:";

            //Người dùng không được phép nhập giá trị âm hoặc =0
            prIntOpt.AllowNegative = false;
            prIntOpt.AllowZero = false;

            //define valid keyword and allow enter
            prIntOpt.Keywords.Add("Big");
            prIntOpt.Keywords.Add("Small");
            prIntOpt.Keywords.Add("Regular");
            prIntOpt.Keywords.Default = "Regular";
            prIntOpt.AllowNone = true;

            PromptIntegerResult prIntegerRes = acEditor.GetInteger(prIntOpt);
            if (prIntegerRes.Status == PromptStatus.Keyword)
            {
                AcadApp.Application.ShowAlertDialog("Bạn đã chọn từ khóa: " + prIntegerRes.StringResult);
            }
            else
            {
                AcadApp.Application.ShowAlertDialog("Bạn đã nhập giá trị: " + prIntegerRes.Value.ToString());
            }

        }
        #endregion
        [CommandMethod("CheckforPickfirstSelection",CommandFlags.UsePickSet)]
        public void CheckforPickFirstSelection()
        {
            //Get the Editor
            Editor acEdior = acadFuncs.GetEditor();
            //Get the pickfirst selectionset
            PromptSelectionResult prSSRes = acEdior.SelectImplied();
            SelectionSet acSSet;
            //if the prompt status is Okie, objects were selected before
            //the command was started
            if (prSSRes.Status == PromptStatus.OK)
            {
                acSSet = prSSRes.Value;
                AcadApp.Application.ShowAlertDialog("Number objects in pickfirst selection: " +
                                            acSSet.Count.ToString());
            }
            else
            {
                AcadApp.Application.ShowAlertDialog("No objects in Pickfirst selection");
            }
            //Clear the pickfirst Selection
            ObjectId[] idarrayEmpty = new ObjectId[0];
            acEdior.SetImpliedSelection(idarrayEmpty);
            //Request object to be selected in drawing
            prSSRes = acEdior.GetSelection();
            if (prSSRes.Status == PromptStatus.OK)
            {
                acSSet = prSSRes.Value;
                AcadApp.Application.ShowAlertDialog("Number objects in pickfirst selection: " +
                                            acSSet.Count.ToString());
            }
            else
            {
                AcadApp.Application.ShowAlertDialog("No objects in Pickfirst selection");
            }
        }
        #region Get String from User
        [CommandMethod("GetStringFromUser")]
        public void GetStringfromUser()
        {
            Editor acEditor = acadFuncs.GetEditor();
            PromptStringOptions prStrOpt = new PromptStringOptions("\n");
            prStrOpt.Message = "\nEnter your name: ";
            prStrOpt.AllowSpaces = true;

            PromptResult prRes = acEditor.GetString(prStrOpt);
            AcadApp.Application.ShowAlertDialog("Chuỗi bạn nhập vào là: " + prRes.StringResult);
        }
        #endregion
        
    }

    public class acadSelectionset
    {
        [CommandMethod("filterblocks")]
        public void FilterBlocks()
        {
            Database db = acadFuncs.GetActiveDb();
            Editor ed = acadFuncs.GetEditor();

            PromptEntityOptions opts = new PromptEntityOptions("\nSelect the target block: ");
            opts.SetRejectMessage("Only a block.");
            opts.AddAllowedClass(typeof(BlockReference), true);
            PromptEntityResult per = ed.GetEntity(opts);
            if (per.Status != PromptStatus.OK)
                return;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockReference br = (BlockReference)tr.GetObject(per.ObjectId, OpenMode.ForRead);
                TypedValue[] filterList = new TypedValue[3]
                {
                    new TypedValue(0, "INSERT"),
                    new TypedValue(2, br.Name),
                    new TypedValue(8, br.Layer)
                };
                PromptSelectionResult psr = ed.SelectAll(new SelectionFilter(filterList));
                ed.SetImpliedSelection(psr.Value);
                ed.WriteMessage("\nNumber of selected objects: {0}", psr.Value.Count);
                tr.Commit();
            }
        }
        [CommandMethod("DVT")]
        public void ChangeColor_Objects()
        {
            Editor acEditor = acadFuncs.GetEditor();
            Database acDb = acadFuncs.GetActiveDb();

            SelectionSet objSS = acadFuncs.getSelectionset("LWPOLYLINE");
            if (null == objSS) return;
            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                ObjectId[] objIDs = objSS.GetObjectIds();

                foreach (ObjectId entID in objIDs)
                {
                    //Entity acEnt = acTrans.GetObject(entID, OpenMode.ForWrite) as Entity;

                    Polyline acPolyline = acTrans.GetObject(entID, OpenMode.ForWrite) as Polyline;
                    if (null != acPolyline)
                    {
                        acPolyline.ColorIndex = 2;
                        using (BlockTableRecord acModel = acadFuncs.getModelSpace(acTrans))
                        {
                            for (int i = 0; i < acPolyline.NumberOfVertices; i++)
                            {
                                double segLen = 0;double txtAngle = 0;
                                Point3d midpoint =  new Point3d();

                                if (acPolyline.GetSegmentType(i) == SegmentType.Line)
                                {
                                    LineSegment2d LineSeg = acPolyline.GetLineSegment2dAt(i);
                                    midpoint = new Point3d(LineSeg.MidPoint.X, LineSeg.MidPoint.Y, 0);
                                    segLen = LineSeg.Length;
                                    txtAngle = LineSeg.Direction.Angle;

                                }
                                else if (acPolyline.GetSegmentType(i) == SegmentType.Arc)
                                {
                                    CircularArc2d ArcSeg = acPolyline.GetArcSegment2dAt(i);
                                    segLen = ArcSeg.GetLength(ArcSeg.GetParameterOf(ArcSeg.StartPoint), ArcSeg.GetParameterOf(ArcSeg.EndPoint));
                                }
                                DBText objText = AddNewEnt.addText(midpoint, Math.Round(segLen, 3).ToString(), 1,0,AttachmentPoint.MiddleCenter);
                                acModel.UpgradeOpen();
                                acModel.AppendEntity(objText);
                                acTrans.AddNewlyCreatedDBObject(objText, true);

                            }
                        }
                    }
                }
                acTrans.Commit();
            }
        }
    }
}
