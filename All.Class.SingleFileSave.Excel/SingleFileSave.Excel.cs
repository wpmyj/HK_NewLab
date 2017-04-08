using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace All.Class
{
    public class SingleFileSave
    {
        public static class Excel
        {
            /// <summary>
            /// 读取指定Excel数据
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <returns></returns>
            public static System.Data.DataTable Read(string fileName)
            {
                return Read(fileName, 0);
            }
            /// <summary>
            /// 读取指定Excel指定表数据
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <param name="sheet">表格名称</param>
            /// <returns></returns>
            public static System.Data.DataTable Read(string fileName, string sheet)
            {
                return Read(fileName, sheet, true);
            }
            /// <summary>
            /// 读取指定Excel指定表数据
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <param name="sheet">表格序号</param>
            /// <returns></returns>
            public static System.Data.DataTable Read(string fileName, int sheet)
            {
                return Read(fileName, sheet, true);
            }
            /// <summary>
            /// 读取指定Excel指定表数据
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <param name="sheet">表格名称</param>
            /// <param name="firstTitle">表格第一行是否做列名</param>
            /// <returns></returns>
            public static System.Data.DataTable Read(string fileName, string sheet, bool firstTitle)
            {
                System.Data.DataTable result = new System.Data.DataTable();
                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        NPOI.SS.UserModel.IWorkbook work = null;
                        if (fileName.EndsWith("xlsx"))
                        {
                            work = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                        }
                        else
                        {
                            work = new NPOI.HSSF.UserModel.HSSFWorkbook(fs, true);
                        }
                        NPOI.SS.UserModel.ISheet hs = work.GetSheet(sheet);
                        System.Collections.IEnumerator rows = hs.GetEnumerator();
                        NPOI.SS.UserModel.ICell cell;
                        while (rows.MoveNext())
                        {
                            NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                            if (result.Columns.Count <= 0)
                            {
                                if (firstTitle)
                                {
                                    for (int i = 0; i < row.LastCellNum; i++)
                                    {
                                        cell = row.GetCell(i);
                                        result.Columns.Add(string.Format("column{0}", i));
                                        if (cell != null)
                                        {
                                            result.Columns[i].Caption = cell.ToString();
                                        }
                                    }
                                    continue;
                                }
                                else
                                {
                                    for (int i = 0; i < hs.GetRow(0).LastCellNum; i++)
                                    {
                                        result.Columns.Add(string.Format("column{0}", i));
                                    }
                                }
                            }
                            System.Data.DataRow dr = result.NewRow();
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                cell = row.GetCell(i);
                                if (cell != null)
                                {
                                    dr[i] = cell.ToString();
                                }
                                else
                                {
                                    dr[i] = null;
                                }
                            }
                            result.Rows.Add(dr);
                        }
                        work = null;
                    }
                }
                catch { }
                return result;
            }
            /// <summary>
            /// 读取指定Excel指定表数据
            /// </summary>
            /// <param name="fileName">文件名称</param>
            /// <param name="sheet">表格序号</param>
            /// <param name="firstTitle">表格第一行是否做列名</param>
            /// <returns></returns>
            public static System.Data.DataTable Read(string fileName, int sheet, bool firstTitle)
            {
                System.Data.DataTable result = new System.Data.DataTable();
                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        NPOI.SS.UserModel.IWorkbook work = null;
                        if (fileName.EndsWith("xlsx"))
                        {
                            work = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
                        }
                        else
                        {
                            work = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                        }
                        NPOI.SS.UserModel.ISheet hs = work.GetSheetAt(sheet);
                        System.Collections.IEnumerator rows = hs.GetEnumerator();
                        NPOI.SS.UserModel.ICell cell;
                        while (rows.MoveNext())
                        {
                            NPOI.SS.UserModel.IRow row = (NPOI.SS.UserModel.IRow)rows.Current;
                            if (result.Columns.Count <= 0)
                            {
                                if (firstTitle)
                                {
                                    for (int i = 0; i < row.LastCellNum; i++)
                                    {
                                        cell = row.GetCell(i);
                                        result.Columns.Add(string.Format("column{0}", i));
                                        if (cell != null)
                                        {
                                            result.Columns[i].Caption = cell.ToString();
                                        }
                                    }
                                    continue;
                                }
                                else
                                {
                                    for (int i = 0; i < hs.GetRow(0).LastCellNum; i++)
                                    {
                                        result.Columns.Add(string.Format("column{0}", i));
                                    }
                                }
                            }
                            System.Data.DataRow dr = result.NewRow();
                            for (int i = 0; i < row.LastCellNum; i++)
                            {
                                cell = row.GetCell(i);
                                if (cell != null)
                                {
                                    dr[i] = cell.ToString();
                                }
                                else
                                {
                                    dr[i] = null;
                                }
                            }
                            result.Rows.Add(dr);
                        }
                        work = null;
                    }
                }
                catch { }
                return result;
            }
        }
    }
}
