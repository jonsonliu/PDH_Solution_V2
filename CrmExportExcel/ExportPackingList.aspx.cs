using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDH_CrmService.CrmInterface;
using PDH_Model.ExportCrmModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace CrmExportExcel
{
    public partial class ExportPackingList : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
            try
            {
                string orderIds = Request.Form["orderIds"].ToString();
                log.Info("orderIds:" + orderIds);
                this.ExportExcel(orderIds);
            }
            catch (Exception ex)
            {
                log.Info("Exception:"+ex);
            }
            
        }
        private void ExportExcel(string orderIds)
        {
            XSSFWorkbook book = new XSSFWorkbook();
            //HSSFWorkbook book = new HSSFWorkbook();
            

            OrderOperation op = new OrderOperation();
            
            string[] orderId_strs = orderIds.Split(',');
            //int row_count = 0;
            for (int i = 0; i < orderId_strs.Length; i++)
            {
                ISheet sheet = book.CreateSheet("Packing List "+(i+1));

                sheet.PrintSetup.Scale = 85;

                Guid orderId = new Guid(orderId_strs[i]);
                ExportCrmOrder crmOrder = op.getOrderInfoById(orderId);
                
                this.CreateDetailRow(0,book, sheet, crmOrder);

                sheet.ProtectSheet("password_123");
                
            }
            

            string excelName = "Packing List";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + excelName +"("+ DateTime.Now.ToString("yyyyMMdd")+")"+ ".xlsx"));
            HttpContext.Current.Response.AddHeader("Content-Length", ms.ToArray().Length.ToString());
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
            System.Web.HttpContext.Current.Response.End();



        }

        public int CreateDetailRow(int i, XSSFWorkbook book, ISheet sheet, ExportCrmOrder crmOrder)
        {
            sheet.SetColumnWidth(0, 17 * 256);
            sheet.SetColumnWidth(1, 17 * 256);
            sheet.SetColumnWidth(2, 17 * 256);
            sheet.SetColumnWidth(3, 17 * 256);
            sheet.SetColumnWidth(4, 17 * 256);
            sheet.SetColumnWidth(5, 17 * 256);
            float rowHight = 20;

            IFont Max18 = getFont(book, (short)FontBoldWeight.Bold, (short)18);
            IFont Max12 = getFont(book, (short)FontBoldWeight.Bold, (short)12);
            IFont Min12 = getFont(book, (short)FontBoldWeight.Normal, (short)12);

            IRow row1 = sheet.CreateRow(i+0);//index代表多少行
            row1.HeightInPoints = rowHight;//行高
            row1.CreateCell(0).SetCellValue("PDH Packing List");
            SetCellRangeAddress(sheet, i + 0, i + 1, 0, 3);
            row1.CreateCell(4).SetCellValue("DATE:");
            SetCellRangeAddress(sheet, i + 0, i + 1, 4, 4);
            row1.CreateCell(5).SetCellValue(DateTime.Now.ToString("dd/MM/yyyy"));
            SetCellRangeAddress(sheet, i + 0, i + 1, 5, 5);

            row1.GetCell(0).CellStyle = getCellStyleWithNoBorder(book, HorizontalAlignment.Center, Max18);
            row1.GetCell(4).CellStyle = getCellStyleWithNoBorder(book, HorizontalAlignment.Left, Max12);
            row1.GetCell(5).CellStyle = getCellStyleWithNoBorder(book, HorizontalAlignment.Right, Max12);

            


            //To 		From		REF NO.:	221
            IRow row2 = sheet.CreateRow(i + 2);//index代表多少行
            row2.HeightInPoints = rowHight;//行高
            row2.CreateCell(0).SetCellValue("To");
            row2.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 2, i + 2, 0, 1);
            row2.CreateCell(2).SetCellValue("From");
            row2.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 2, i + 2, 2, 3);
            row2.CreateCell(4).SetCellValue("REF NO.:");
            row2.CreateCell(5).SetCellValue(crmOrder.RefNo);

            ICellStyle cellStyle = getCellStyleWithColor(book);
            row2.GetCell(0).CellStyle = cellStyle;
            row2.GetCell(1).CellStyle = cellStyle; 
            row2.GetCell(2).CellStyle = cellStyle;
            row2.GetCell(3).CellStyle = cellStyle; 
            row2.GetCell(4).CellStyle = cellStyle; 
            row2.GetCell(5).CellStyle = cellStyle;
            
            IRow row3 = sheet.CreateRow(i + 3);//index代表多少行
            row3.HeightInPoints = rowHight;//行高
            row3.CreateCell(0).SetCellValue(crmOrder.BillToName);
            row3.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 3, i + 3, 0, 1);
            row3.CreateCell(2).SetCellValue("PetDreamHouse");
            row3.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 3, i + 3, 2, 3);
            row3.CreateCell(4).SetCellValue("ORDER NO.:");
            row3.CreateCell(5).SetCellValue(crmOrder.OrderNo);

            row3.GetCell(0).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row3.GetCell(1).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row3.GetCell(2).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row3.GetCell(3).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row3.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Max12);
            row3.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Right, Min12);


            IRow row4 = sheet.CreateRow(i + 4);//index代表多少行
            row4.HeightInPoints = rowHight;//行高
            row4.CreateCell(0).SetCellValue(crmOrder.Street1);
            row4.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 4, i + 4, 0, 1);
            row4.CreateCell(2).SetCellValue("Unit 106, The Enterprise Centre");
            row4.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 4, i + 4, 2, 3);
            row4.CreateCell(4).SetCellValue("");
            row4.CreateCell(5).SetCellValue("");
            SetCellRangeAddress(sheet, i + 4, i + 4, 4, 5);

            row4.GetCell(0).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row4.GetCell(1).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row4.GetCell(2).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row4.GetCell(3).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row4.GetCell(4).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row4.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);


            IRow row5 = sheet.CreateRow(i + 5);//index代表多少行
            row5.HeightInPoints = rowHight;//行高
            row5.CreateCell(0).SetCellValue(crmOrder.Street2);
            row5.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 5, i + 5, 0, 1);
            row5.CreateCell(2).SetCellValue("Cottingham Road");
            row5.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 5, i + 5, 2, 3);
            row5.CreateCell(4).SetCellValue("SHIP DATE:");
            row5.CreateCell(5).SetCellValue(crmOrder.ShipDate);

            row5.GetCell(0).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row5.GetCell(1).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row5.GetCell(2).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row5.GetCell(3).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row5.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Max12);
            row5.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Right, Min12);

            IRow row6 = sheet.CreateRow(i + 6);//index代表多少行
            row6.HeightInPoints = rowHight;//行高
            row6.CreateCell(0).SetCellValue(crmOrder.City);
            row6.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 6, i + 6, 0, 1);
            row6.CreateCell(2).SetCellValue("Hull");
            row6.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 6, i + 6, 2, 3);
            row6.CreateCell(4).SetCellValue("");
            row6.CreateCell(5).SetCellValue("");
            SetCellRangeAddress(sheet, i + 6, i + 6, 4, 5);

            row6.GetCell(0).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row6.GetCell(1).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row6.GetCell(2).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row6.GetCell(3).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.None, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row6.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row6.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);


            IRow row7 = sheet.CreateRow(i + 7);//index代表多少行
            row7.HeightInPoints = rowHight;//行高
            row7.CreateCell(0).SetCellValue(crmOrder.PostCode);
            row7.CreateCell(1).SetCellValue("");
            SetCellRangeAddress(sheet, i + 7, i + 7, 0, 1);
            row7.CreateCell(2).SetCellValue("HU6 7RX");
            row7.CreateCell(3).SetCellValue("");
            SetCellRangeAddress(sheet, i + 7, i + 7, 2, 3);
            row7.CreateCell(4).SetCellValue("SHIP VIA:");
            row7.CreateCell(5).SetCellValue(crmOrder.ShipVia);

            row7.GetCell(0).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row7.GetCell(1).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row7.GetCell(2).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row7.GetCell(3).CellStyle = getCellStyleWithSpBorder(book, HorizontalAlignment.Left, Min12, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.SS.UserModel.BorderStyle.None);
            row7.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Max12);
            row7.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Right, Min12);


            IRow row9 = sheet.CreateRow(i + 9);//index代表多少行
            row9.HeightInPoints = rowHight;//行高
            row9.CreateCell(0).SetCellValue("NOTE:");
            SetCellRangeAddress(sheet, i + 9, i + 11, 0, 0);
            row9.CreateCell(1).SetCellValue(crmOrder.Note);
            SetCellRangeAddress(sheet, i + 9, i + 11, 1, 5);

            row9.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Max12);
            row9.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

            row9.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row9.CreateCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row9.CreateCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row9.CreateCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

            IRow row91 = sheet.CreateRow(i + 10);
            row91.CreateCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row91.CreateCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

            IRow row92 = sheet.CreateRow(i + 11);
            row92.CreateCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row92.CreateCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row92.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row92.CreateCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row92.CreateCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
            row92.CreateCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);


            //Location	Product Code			QTY ORDERED	QTY SHIPPED
            IRow row13 = sheet.CreateRow(i + 13);//index代表多少行
            row13.HeightInPoints = rowHight;//行高
            row13.CreateCell(0).SetCellValue("Location");
            row13.CreateCell(1).SetCellValue("Product Code");
            SetCellRangeAddress(sheet, i + 13, i + 13, 1, 2);
            row13.CreateCell(3).SetCellValue("Value");
            row13.CreateCell(4).SetCellValue("QTY ORDERED");
            row13.CreateCell(5).SetCellValue("QTY SHIPPED");


            row13.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row13.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row13.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row13.GetCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row13.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row13.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);

            List<ExportCrmOrderProduct> products = crmOrder.products;
            List<string> isExistSupplierA = new List<string> {"INIT"};
            int allProductCount = 0;
            int rowCount = i + 14 ;
            decimal allCost = 0;
            for (int j = 0; j < products.Count; j++)
            {
                ExportCrmOrderProduct product = products[j];

                //Non Stock-->1,Purchased Stock-->2,Third Party Stock-->3
                string sockStatus = product.StockStatus;
                if (sockStatus == null || sockStatus == "")
                {
                    IRow row14 = sheet.CreateRow(rowCount);//index代表多少行
                    row14.HeightInPoints = rowHight;//行高
                    row14.CreateCell(0).SetCellValue(product.Location);
                    row14.CreateCell(1).SetCellValue(product.ProductName);
                    SetCellRangeAddress(sheet, rowCount, rowCount, 1, 2);
                    if (product.LandedCost != null && product.LandedCost != "")
                    {
                        row14.CreateCell(3).SetCellValue("£ " + product.LandedCost);
                    }
                    else{
                        row14.CreateCell(3).SetCellValue("");
                    }
                    
                    row14.CreateCell(4).SetCellValue(product.QtyOrdered);
                    row14.CreateCell(5).SetCellValue("");


                    row14.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

                    rowCount += 1;
                    string qty = product.QtyOrdered;
                    allProductCount += int.Parse(qty);

                    if (product.LandedCost != null && product.LandedCost != "")
                    {
                        allCost += decimal.Parse(product.LandedCost) * decimal.Parse(product.QtyOrdered);
                    }
                }
                else if (sockStatus.Equals("1") && !isExistSupplierA.Contains(product.SupplierA))
                {
                    IRow row14 = sheet.CreateRow(rowCount);//index代表多少行
                    row14.HeightInPoints = rowHight;//行高
                    row14.CreateCell(0).SetCellValue(product.Location);
                    row14.CreateCell(1).SetCellValue(crmOrder.OrderNo + " . " + product.SupplierA);
                    isExistSupplierA.Add(product.SupplierA);
                    SetCellRangeAddress(sheet, rowCount, rowCount, 1, 2);
                    //row14.CreateCell(4).SetCellValue(product.QtyOrdered);
                    row14.CreateCell(3).SetCellValue("");
                    row14.CreateCell(4).SetCellValue("");
                    row14.CreateCell(5).SetCellValue("");


                    row14.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

                    rowCount += 1;
                    //allProductCount += 1;
                }
                //Non Stock-->1,Purchased Stock-->2,Third Party Stock-->3
                else if (!sockStatus.Equals("1"))
                {
                    IRow row14 = sheet.CreateRow(rowCount);//index代表多少行
                    row14.HeightInPoints = rowHight;//行高
                    row14.CreateCell(0).SetCellValue(product.Location);
                    row14.CreateCell(1).SetCellValue(product.ProductName);
                    SetCellRangeAddress(sheet, rowCount, rowCount, 1, 2);
                    if (product.LandedCost != null && product.LandedCost != "")
                    {
                        row14.CreateCell(3).SetCellValue("£ " + product.LandedCost);
                    }
                    else
                    {
                        row14.CreateCell(3).SetCellValue("");
                    }
                    row14.CreateCell(4).SetCellValue(product.QtyOrdered);
                    row14.CreateCell(5).SetCellValue("");


                    row14.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.CreateCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);
                    row14.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Min12);

                    rowCount += 1;
                    string qty = product.QtyOrdered;
                    allProductCount += int.Parse(qty);
                    if (product.LandedCost != null && product.LandedCost != "")
                    {
                        allCost += decimal.Parse(product.LandedCost) * decimal.Parse(product.QtyOrdered);
                    }
                    
                }
                
                //row14.CreateCell(5).SetCellValue("QTY SHIPPED");
            }

            //IRow row15 = sheet.CreateRow(i + 14 + products.Count );//index代表多少行
            //row15.HeightInPoints = rowHight;//行高
            //row15.CreateCell(0).SetCellValue("");
            //row15.CreateCell(1).SetCellValue("");
            //SetCellRangeAddress(sheet, i + 14 + products.Count , i + 14 + products.Count , 1, 3);
            //row15.CreateCell(4).SetCellValue("");
            //row15.CreateCell(5).SetCellValue("");

            //row15.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row15.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row15.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row15.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);



            //IRow row16 = sheet.CreateRow(i + 14 + products.Count + 1);//index代表多少行
            //row16.HeightInPoints = rowHight;//行高
            //row16.CreateCell(0).SetCellValue("");
            //row16.CreateCell(1).SetCellValue("");
            //SetCellRangeAddress(sheet, i + 14 + products.Count + 1, i + 14 + products.Count + 1, 1, 3);
            //row16.CreateCell(4).SetCellValue("");
            //row16.CreateCell(5).SetCellValue("");

            //row16.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row16.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row16.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row16.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);



            //IRow row17 = sheet.CreateRow(i + 14 + products.Count + 2);//index代表多少行
            //row17.HeightInPoints = rowHight;//行高
            //row17.CreateCell(0).SetCellValue("");
            //row17.CreateCell(1).SetCellValue("");
            //SetCellRangeAddress(sheet, i + 14 + products.Count + 2, i + 14 + products.Count + 2, 1, 3);
            //row17.CreateCell(4).SetCellValue("");
            //row17.CreateCell(5).SetCellValue("");

            //row17.GetCell(0).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row17.GetCell(1).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row17.GetCell(4).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);
            //row17.GetCell(5).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, short.MinValue, (short)12);




            IRow row18 = sheet.CreateRow(i + rowCount );//index代表多少行
            row18.HeightInPoints = rowHight;//行高
            row18.CreateCell(2).SetCellValue("Total:");
            if (allCost != 0)
            {
                row18.CreateCell(3).SetCellValue("£ " + allCost.ToString("0.00"));
            }
            else
            {
                row18.CreateCell(3).SetCellValue("");
            }


            row18.GetCell(2).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Center, Max12);
            row18.GetCell(3).CellStyle = getCellStyleWithBorder(book, HorizontalAlignment.Left, Max12);
                    

            i = i + 14 + products.Count + 8;
            return i;

            
        }
        private ICellStyle getCellStyleWithColor(XSSFWorkbook book)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.WrapText = true;

            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;

            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            IFont font = book.CreateFont();
            font.FontName = "Arial";
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.FontHeightInPoints = (short)12;
            cellStyle.SetFont(font);

            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.IsLocked = true;//read only

            return cellStyle;
        }
        private IFont getFont(XSSFWorkbook book, short boldweight, short size)
        {
            IFont font = book.CreateFont();
            font.FontName = "Arial";
            font.Boldweight = boldweight;
            font.FontHeightInPoints = size;
            return font;

            
        }
        private ICellStyle getCellStyleWithBorder(XSSFWorkbook book, HorizontalAlignment alignment, IFont font)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.WrapText = true;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            
            cellStyle.SetFont(font);

            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.IsLocked = true;

            return cellStyle;
        }

        private ICellStyle getCellStyleWithNoBorder(XSSFWorkbook book, HorizontalAlignment alignment, IFont font)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.WrapText = true;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;

            cellStyle.SetFont(font);

            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.IsLocked = true;

            return cellStyle;
        }

        private ICellStyle getCellStyleWithSpBorder(XSSFWorkbook book, HorizontalAlignment alignment, IFont font, NPOI.SS.UserModel.BorderStyle bottom, NPOI.SS.UserModel.BorderStyle left, NPOI.SS.UserModel.BorderStyle right, NPOI.SS.UserModel.BorderStyle top)
        {
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.WrapText = true;
            cellStyle.BorderBottom = bottom;
            cellStyle.BorderLeft = left;
            cellStyle.BorderRight = right;
            cellStyle.BorderTop = top;

            cellStyle.SetFont(font);

            cellStyle.Alignment = alignment;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            cellStyle.IsLocked = true;

            return cellStyle;
        }



        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet">要合并单元格所在的sheet</param>
        /// <param name="rowstart">开始行的索引</param>
        /// <param name="rowend">结束行的索引</param>
        /// <param name="colstart">开始列的索引</param>
        /// <param name="colend">结束列的索引</param>
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);

            sheet.AddMergedRegion(cellRangeAddress);
            //((HSSFSheet)sheet).SetEnclosedBorderOfRegion(cellRangeAddress, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);

        }

        //public static void SetCellRangeAddress2(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        //{
        //    CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);

        //    sheet.AddMergedRegion(cellRangeAddress);
        //    //((HSSFSheet)sheet).SetEnclosedBorderOfRegion(cellRangeAddress, NPOI.SS.UserModel.BorderStyle.Thin, HSSFColor.Black.Index);

        //}




    }
}