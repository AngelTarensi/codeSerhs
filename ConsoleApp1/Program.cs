using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using System.Configuration;
using Lib;

namespace ConsoleApp1
{
    internal class Program
    {
        static string pathD = ConfigurationManager.AppSettings["pathD"];
        static string pathO = ConfigurationManager.AppSettings["pathO"];
        static string folderPathThumbnails = ConfigurationManager.AppSettings["folderPathThumbnails"];
        static string folderPathMagento = ConfigurationManager.AppSettings["folderPathMagento"];

        static ILog log = LogManager.GetLogger(typeof(Program));
        static Lib.Helper helper = new Lib.Helper();
     
       
        static void Main(string[] args)
        {            
            bool salir = false;
            string file = string.Empty;
            string newFile = string.Empty;
            List<string> listImg2 = new List<string>();

            while (!salir)
            {

                Console.WriteLine("Pulse 1 para mostrar las imagenes de la carpeta ProvasC#");
                Console.WriteLine("Pulse 2 para copiar una imagen y cambiar el nombre");
                Console.WriteLine("Pulse 3 para renombrar una imagen");
                Console.WriteLine("Pulse 4 para comparar carpetas");
                Console.WriteLine("Pulse 5 para salir de la aplicación");
                Console.WriteLine("");
                Console.WriteLine("Elige una de las opciones");
                Console.WriteLine("");
                int opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        //1. Devolver lista de nombres, extensiones y ruta de las imagenes dentro de la carpeta
                        List<Image> listImg  = new List<Image>();

                        if (helper.Validatepath(pathD))
                        {
                            listImg = helper.GetFilesFromFolder(pathD);
                            if (listImg.Count == 0)
                            {
                                Console.WriteLine("No hay ficheros");
                                log.Info("En la carpeta no hay archivos");
                            }
                            else
                            {
                                Console.WriteLine("Las imagenes existentes son las sigueintes");
                                foreach (Image img in listImg)
                                {
                                    Console.WriteLine(string.Format("{0}-{1}-{2}", img.path, img.name, img.extesion));

                                }
                            }
                        }
                        else
                        {
                            log.Error("No existeix la ruta");
                            Console.WriteLine("No existeix la ruta");
                        }
                        break;




                    case 2:
                        //2. Copiar una imagen y ponera a otra carpeta o en la misma con un nombre diferente
                        Console.WriteLine("Que imagen desea copiar?");
                        file = Console.ReadLine();

                        Console.WriteLine("Que nombre quiere poner a la imagen copiada? ej. hola.jpg/.png");
                        newFile = Console.ReadLine();


                        if(helper.Validatepath(pathD))
                        {
                            if (helper.ValidateExistFile(file))
                            {
                                if (helper.CopyFileName(file, newFile))
                                    //File.Copy(fileName, newfile);
                                    Console.WriteLine("OK");
                                else
                                {
                                    Console.WriteLine("KO");
                                    log.Error("Imagen no encontrada o mal escrita");
                                }
                            }
                            else
                            {
                                log.Error("No existe el fichero seleccionado");
                                Console.WriteLine("No existe el fichero seleccionado");
                            }
                        }
                        else
                        {
                            log.Error("No existeix la ruta");
                            Console.WriteLine("No existeix la ruta");
                        }
                        break;


                    case 3:
                        //3. Renombrar fichero seleccionado
                        Console.WriteLine("Que imagen desea renombrar?");
                        file = Console.ReadLine();

                        Console.WriteLine("nuevo nombre");
                        newFile = Console.ReadLine();


                        if (helper.Validatepath(pathD))
                        {
                            if(helper.Validatepath(folderPathMagento))
                            {
                                if (helper.ValidateExistFile(file))
                                {

                                    //renombra fichero
                                    if (helper.RenameFile(file, pathD, newFile, folderPathMagento, folderPathThumbnails))
                                        Console.WriteLine("OK");
                                    else
                                    {
                                        Console.WriteLine("KO");
                                        log.Error("Imagen no encontrada o mal escrita");
                                    }
                                }
                                else
                                {
                                    log.Error("No existe el fichero seleccionado");
                                    Console.WriteLine("No existe el fichero seleccionado");
                                }
                            }
                        }
                        else
                        {
                            log.Error("No existeix la ruta");
                            Console.WriteLine("No existeix la ruta");
                        }
                        break;

                    case 4:
                        //4. Comparar carpetas, si falta una imagen en la carpeta Magento se detectara y se reproducira un nombre

                        if(helper.Validatepath(folderPathThumbnails))
                        {
                            if(helper.Validatepath(folderPathMagento))
                            {
                                listImg2 = helper.CompareFolders(folderPathThumbnails, folderPathMagento);
                                {
                                    foreach (string img in listImg2)
                                    {
                                        Console.WriteLine("Imagenes faltantes:");
                                        Console.WriteLine("{0}", img);
                                    }
                                }
                            }
                        }
                        break;



                    case 5:
                        Console.WriteLine("Dentro de poco saldra de la aplicación");
                        salir = true;
                        break;
                }
            }
        }
    }
}
