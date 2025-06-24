using Dynamic.Tekla.Structures;
using Dynamic.Tekla.Structures.Drawing;
using Dynamic.Tekla.Structures.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Ncs
{
   
    public static class NcParaDxf
    {
        /**** Modify these strings to suit local environment and user preferences ****/
        private static string defFile = "tekla_dstv2dxf_metric_tnk.def";
        private static string exeFile = "tekla_dstv2dxf.exe";
        private static string dxfOutputFolder = "NC_dxf";
        /*****************************************************************************/

        private static string FindDstvFolder()
        {
            var binDir = string.Empty;
            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref binDir);

            if (Directory.Exists(Path.Combine(binDir, "applications", "Tekla", "Tools", "dstv2dxf")))
            {
                return Path.Combine(binDir, "applications", "Tekla", "Tools", "dstv2dxf");
            }

            var xsDir = string.Empty;
            TeklaStructuresSettings.GetAdvancedOption("XS_DIR", ref xsDir);

            if (Directory.Exists(Path.Combine(xsDir, "nt", "dstv2dxf")))
            {
                return Path.Combine(xsDir, "nt", "dstv2dxf");
            }

            return string.Empty;
        }

        public static void Run()
        {
            string modelDir;
            var dstvDir = FindDstvFolder();

            try
            {
                TSM.Model Model = new TSM.Model();

                /** Get model directory **/
                modelDir = Model.GetInfo().ModelPath;// new System.IO.DirectoryInfo("./").FullName;

                /** Mudar os dstv"**/
                if (System.IO.Directory.Exists(@modelDir + "\\" + "DSTV_CHAPAS"))
                {
                    var files = System.IO.Directory.EnumerateFiles(@modelDir + "\\" + "DSTV_CHAPAS");
                    foreach ( var file in files)
                    {
                        foreach (var modelObject in Model.GetModelObjectSelector())
                        {
                            if(modelObject is TSM.ContourPlate part)
                            {
                                var peca = modelObject as TSM.ContourPlate;
                                var mark = file.Split('\\').Last().Replace(".nc1", "");
                                var posicao = peca.GetPartMark();//.Replace(" ", "").Replace(",", "").Replace(".", "").Replace("-", "").Replace(":", "");
                                if (posicao.Equals(mark, StringComparison.OrdinalIgnoreCase))
                                {
                                    var perfil = peca.Profile.ProfileString.Replace("CH","");
                                    lineChanger(file, perfil);
                                    break;
                                }
                                
                            }
                        }


                           
                    }

                    

                }
                



                if (!File.Exists(Path.Combine(dstvDir, defFile)))
               {
                   System.Windows.MessageBox.Show("The conversion definition file " + defFile + " and/or the dstv2dxf directory could not be found.\n\npleases modify the macro script to point to the correct directory.");
                   return;
               }

               /** Copy dstv2dxf.exe to the model folder **/
                if (!System.IO.File.Exists(@modelDir + "\\" + exeFile))
                    new System.IO.FileInfo(Path.Combine(dstvDir, "tekla_dstv2dxf.exe")).CopyTo(@modelDir + "tekla_dstv2dxf.exe", true);

                /** Generate the "model local" version of the def file **/
                System.IO.StreamReader sr = new System.IO.StreamReader(Path.Combine(dstvDir, defFile), System.Text.Encoding.Default);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Path.Combine(modelDir, defFile), false, System.Text.Encoding.Default);
                string line = " ";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim().IndexOf("INPUT_FILE_DIR") >= 0)
                        sw.WriteLine("INPUT_FILE_DIR=");
                    else if (line.Trim().IndexOf("OUTPUT_FILE_DIR") >= 0)
                        sw.WriteLine("OUTPUT_FILE_DIR=" + @modelDir + "\\" + dxfOutputFolder);
                    else
                        sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();

                /** Check for dxfOutputFolder. Creat it if it doesn't exist **/
                if (!System.IO.Directory.Exists(@modelDir + "\\" + dxfOutputFolder))
                    System.IO.Directory.CreateDirectory(@modelDir + dxfOutputFolder);

                /** Launch the local dstv2dxf **/
                System.Diagnostics.Process NCDXFConv = new System.Diagnostics.Process();
                NCDXFConv.EnableRaisingEvents = false;
                NCDXFConv.StartInfo.CreateNoWindow = true;
                NCDXFConv.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                NCDXFConv.StartInfo.FileName = @modelDir + "\\" + "tekla_dstv2dxf.exe";
                NCDXFConv.StartInfo.Arguments = " -cfg " + defFile + " -m batch";
                NCDXFConv.Start();
                NCDXFConv.WaitForExit();
                NCDXFConv.Close();

                //if (System.Windows.MessageBox.Show("Do you want to open the folder, which contains the DXF files?", "Tekla Structures", MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                //{
                //    /** Open a file explorer window in the output folder **/
                //    System.Diagnostics.Process Explorer = new System.Diagnostics.Process();
                //    Explorer.EnableRaisingEvents = false;
                //    Explorer.StartInfo.FileName = "explorer";
                //    Explorer.StartInfo.Arguments = "\"" + @modelDir + dxfOutputFolder + "\"";
                //    Explorer.Start();
                //}

            }
            /** This little section will report any errors that may happen during run-time, and even tell you what line of code the error happened at **/
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show("Problem runing process\n" + e.Message + "\n" + e.StackTrace);
            }
        }

        static void lineChanger(string sourceFile, string perfil)
        {
            string destinationFile = sourceFile.Replace(".nc1",".nc2");
            string tempFile = sourceFile.Replace(".nc1", ".nc3");

            if (!File.Exists(destinationFile))
                File.Create(destinationFile).Close();

            if (!File.Exists(tempFile))
                File.Copy(sourceFile, tempFile, true);

            int line_number = 0;
            string line = null;
            int line_to_edit = 4; // The line number to edit (0-based index)
            string marca = "";
            using (StreamReader reader = new StreamReader(tempFile))
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if(line_number == line_to_edit - 1)
                    {
                        marca = line;
                    }

                    if (line_number == line_to_edit)
                    {
                        writer.WriteLine(marca);
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                    line_number++;
                }
            }

            // Delete the old file.
            File.Delete(sourceFile);

            // Rename the new file to the old file name.
            File.Move(destinationFile, sourceFile);
            File.Delete(tempFile);

        }

    }
}
