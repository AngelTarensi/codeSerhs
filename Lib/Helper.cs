using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
namespace Lib
{
    public class Helper
    {
        static ILog log = LogManager.GetLogger(typeof(Helper));

        /// <summary>
        /// Objetivo, Que devuelva todos los ficheros que haya en la carpeta seleccionada
        /// </summary>
        /// <param name="pathO"> variable string, basicamente una ruta hacia una carpeta</param>
        /// Todos los img. son variables imagen que dividen el nombre principal en partes, en este caso extension, nombre y la ruta
        /// <returns></returns>
        public List<Image> GetFilesFromFolder(string pathD)
        {

            List<Image> listImg = new List<Image>();
            try
            {
                    string[] files = System.IO.Directory.GetFiles(pathD);
                    int pathSize = pathD.Length;
                    foreach (string s in files)
                    {
                        Image img = new Image();
                        img.extesion = Path.GetExtension(s);
                        img.name = s.Substring(pathSize, s.Substring(pathSize).LastIndexOf("."));
                        img.path = pathD;
                        listImg.Add(img);
                    }
            }

            catch (InvalidCastException e)
            {
                log.Error("Error: " + e);
            }
            return listImg;

        }

        /// <summary>
        /// Objetivo, copiar un archivo y poner diferente nombre
        /// </summary>
        /// <param name="fileName"> Recojer el fichero que el usuario ponga ex. 1.txt </param>
        /// <param name="path"> Es una variable string que lo que hace es seleccionar la ruta en la que estara el usuario para elejir el fileName </param>
        /// <returns></returns>
        public bool CopyFileName(string fileName, string newFile)
        {
            bool ok = false;
            try
            {
                        //FICHERO A COPIAR
                        File.Copy(fileName, newFile);
                        ok = true;
            }
            catch (InvalidCastException e)
            {
                log.Error("Error: " + e);
            }
            return ok;
        }

        /// <summary>
        /// Objetivo, Cambiar el nombre de una imagen, el codigo se ha complicado ya que lo que hacemos es mover el fichero que queremos renombrar a otra carpeta, cuando esta en esa carpeta se cambia el nombrey despues se devuelve a la carpeta destino, y validar si existe la ruta donde esta el fichero y si cuando es copiado existe
        /// </summary>
        /// <param name="path"> Ruta donde el usuario cambiar el nombre de el fichero </param>
        /// <param name="nameFile"> Fichero que el usuario elegira para cambiar el nombre </param>
        /// <returns></returns>
        public bool RenameFile(string file, string pathD, string newFile, string folderPathMagento, string folderPathThumbnails)
        {
            bool ok = false;
            try
            {

                        //FICHERO A RENOMBRAR
                        //File.Move(file, newfile);
                        //Hacer que el "file" se vaya a otra carpeta externa y se renombre y vuelva a la carpeta
                        string fullPathFile = Path.Combine(folderPathMagento, file);
                        string fullPathFileDest = Path.Combine(pathD, file);

                        string fullPathNewFile = Path.Combine(pathD, newFile);
                        string fullPathNewFileOri = Path.Combine(folderPathMagento, newFile);
                        //De la carpeta principal a una carpeta de fuera
                        File.Move(fullPathFile, fullPathFileDest);
                        //En la carpeta de fuera cambiar nombre
                        File.Move(fullPathFileDest, fullPathNewFile);
                        //Mover la imagen a la carpeta principal
                        File.Move(fullPathNewFile, fullPathNewFileOri);
                        ok = true;
            }
            catch (InvalidCastException e)
            {
                log.Error("Error: " + e);
            }
            return ok;
        }

        //Estas funciones son basicamente para validar la ruta y el fichero
        public bool Validatepath(string pathO)
        {
            Directory.SetCurrentDirectory(pathO);
            return Directory.Exists(pathO);
        }
        public bool ValidateExistFile(string file)
        {
            return File.Exists(file);
        }
        /// <summary>
        /// Objetivo, Comparar las imagenes en estas 2 carpetas, si no hay una imagen en 1 carpeta y en otra si, salta el nombre
        /// </summary>
        /// <param name="folderPathThumbnails">Carpeta Principal con todas las imagenes</param>
        /// <param name="folderPathMagento">Carpeta que se compara con alguna que falta(puede no faltar ninguna)</param>
        /// <returns></returns>
        public List<string> CompareFolders(string folderPathThumbnails, string folderPathMagento)
        {
            // Comprobar que existan las mismas imagenes en thumbnails que en Magento

            List<string> listImg = new List<string>();



                string[] files = System.IO.Directory.GetFiles(folderPathThumbnails);

                // que te devuelva extension
                // que te devuelve ruta
                // que te devuelva path

                int pathSize = folderPathThumbnails.Length;
                foreach (string img in files)
                {
                    var nameWithExtension = img.Substring(pathSize);
                    log.Info(nameWithExtension + " Nombre del archivo que se esta comparando");
                    if (!File.Exists(string.Format("{0}\\{1}", folderPathMagento, nameWithExtension)))
                    {
                        // Añadir imagenees no existentes a una lista
                        listImg.Add(nameWithExtension);
                    }
                    log.Info("Se cambia de archivo para comparar a " + img);
                }
                return listImg;
        }
    }
}