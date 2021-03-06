﻿using System;
using System.Collections.Generic;
using System.IO;

namespace EBusCopyApp.Utilities
{
    public class Helper
    {
        public Helper()
        {
        }

        public bool IsFileLocked(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        public List<string> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            if (string.IsNullOrEmpty(sDir)) return files;
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return files;
        }


        public void MoveFile(string sourcepath, string destinationPath, string newFileName)
        {
            try
            {
                string file = Path.GetFileName(sourcepath);
                string destinationFilePath = destinationPath + "//" + file;

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                //Checks if files exists and deletes if they do
                if (File.Exists(destinationFilePath))
                {
                    destinationFilePath = destinationPath + "//" + newFileName;
                }
                File.Copy(sourcepath, destinationFilePath);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
