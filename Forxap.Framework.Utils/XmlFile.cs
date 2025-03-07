﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using System.Windows.Forms;
using Forxap.Framework.DI;

namespace Forxap.Framework.Utils
{
   public   class XmlFile : Base
    {

       /// <summary>
       ///  Lee las tablas de usuario desde un archivo xml
       /// </summary>
       /// <param name="xmlFile"></param>
       public static void LoadUserTablesFromXmlFile(string xmlFile)
       {
           XmlDocument xmlDocument = new XmlDocument();
           UserTables userTables = new UserTables();


           // verificar que el archivo exista
           if (File.Exists(xmlFile))
           {
               // si el archivo existe entonces lo intenta leer
               xmlDocument.Load(xmlFile);


               /// cargo el nodo con toda la lista de las tablas
               XmlNodeList nodeListTables = xmlDocument.GetElementsByTagName("UserTables");

               ///obtengo una de las tablas
               XmlNodeList nodeListTable = ((XmlElement)nodeListTables[0]).GetElementsByTagName("UserTablesMD");

               /// recorro los nodos de una tabla
               foreach (XmlElement nodo in nodeListTable)
               {

                   int i = 0;

                   XmlNodeList nTableName = nodo.GetElementsByTagName("TableName");

                   XmlNodeList nTableDescription = nodo.GetElementsByTagName("TableDescription");

                   XmlNodeList nTableType = nodo.GetElementsByTagName("TableType");

                   XmlNodeList nArchivable = nodo.GetElementsByTagName("Archivable");
                   XmlNodeList nArchiveDateField = nodo.GetElementsByTagName("ArchiveDateField"); 


                   Errors.Sb1Error sb1Error = userTables.CreateUserTable
                       (
                       nTableName[i].InnerText.Trim(), 
                       nTableDescription[i].InnerText.Trim(), 
                       (SAPbobsCOM.BoUTBTableType)Enum.Parse(typeof(SAPbobsCOM.BoUTBTableType),nTableType[i].InnerText),
                       (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum),nArchivable[i].InnerText),
                        nArchiveDateField[i].InnerText.Trim()
                       );

                   if (sb1Error.Code == 0)
                       Forxap.Framework.UI.Messages.ShowSuccess("Tabla : " + sb1Error.Message);
                   else
                       Forxap.Framework.UI.Messages.ShowWarning("Error: " + sb1Error.Code + sb1Error.Message);
               }

           }

           else
           {
               Forxap.Framework.UI.Messages.ShowError("Error: " + xmlFile +  " Archivo no existe");
           }


       }


       /// <summary>
       /// lee campos de usuarios desde un archivo xml 
       /// </summary>
       /// <param name="xmlFile"></param>
       public static void LoadUserFieldsFromXmlFile(string xmlFile)
       {
           XmlDocument xmlDocument = new XmlDocument();
           UserFields userFields = new UserFields();

           string fieldName = string.Empty;
           string fieldType = string.Empty;
           int size = 0;
           string fieldDescription = string.Empty;

           string fieldSubType = string.Empty;
           string linkedTable = string.Empty;
           string defaultValue = string.Empty;
           string tableName = string.Empty;
           int fieldID = 0;
           string mandatory = string.Empty;
           int editSize = 0;

           string linkedUDO = string.Empty;
           IDictionary<string, string> validValues = null; 


           // verificar que el archivo exista
           if (File.Exists(xmlFile))
           {
               try
               {
                   // si el archivo existe entonces lo intenta leer
                   xmlDocument.Load(xmlFile);

                   /// cargo el nodo con toda la lista de los campos
                   XmlNodeList nodeListFields = xmlDocument.GetElementsByTagName("UserFields");

                   ///obtengo uno de los campos
                   XmlNodeList nodeListField = ((XmlElement)nodeListFields[0]).GetElementsByTagName("Field");

                   
                   /// recorro los nodos de un campo
                   foreach (XmlElement nodo in nodeListField)
                   {

                       
                       int i = 0;

                       XmlNodeList nUserFieldsMD = nodo.GetElementsByTagName("UserFieldsMD");

                       fieldName = string.Empty;
                       fieldType = string.Empty;
                       size = 0;
                       fieldDescription = string.Empty;
                       fieldSubType = string.Empty;
                       linkedTable = string.Empty;
                       defaultValue = string.Empty;
                       tableName = string.Empty;
                       fieldID = 0;
                       mandatory = string.Empty;
                       editSize = 0;

                       linkedUDO = string.Empty;
                       validValues= null ; 

                       foreach (XmlElement nodo1 in nUserFieldsMD)
                       {
                         XmlNodeList nFieldName = nodo1.GetElementsByTagName("FieldName");
                         XmlNodeList nType = nodo1.GetElementsByTagName("Type");
                         XmlNodeList nSize = nodo1.GetElementsByTagName("Size");
                         XmlNodeList nDescription = nodo1.GetElementsByTagName("Description");
                         XmlNodeList nSubType = nodo1.GetElementsByTagName("SubType");
                         XmlNodeList nLinkedTable = nodo1.GetElementsByTagName("LinkedTable");
                         XmlNodeList nDefaultValue = nodo1.GetElementsByTagName("DefaultValue");
                         XmlNodeList nTableName = nodo1.GetElementsByTagName("TableName");
                         XmlNodeList nFieldID = nodo1.GetElementsByTagName("FieldID");
                         
                         XmlNodeList nEditSize = nodo1.GetElementsByTagName("EditSize");
                         XmlNodeList nMandatory = nodo1.GetElementsByTagName("Mandatory");

                         

                         fieldName = nFieldName[i].InnerText.Trim();
                         fieldType = nType[i].InnerText.Trim();
                         size = Convert.ToInt32( nSize[i].InnerText.Trim());
                         fieldDescription = nDescription[i].InnerText.Trim();
                         fieldSubType = nSubType[i].InnerText.Trim();
                         linkedTable = nLinkedTable[i].InnerText.Trim()?? string.Empty;
                         defaultValue = nDefaultValue[i].InnerText.Trim();
                         tableName = nTableName[i].InnerText.Trim();
                         fieldID = Convert.ToInt32( nFieldID[i].InnerText.Trim());
                         mandatory = nMandatory[i].InnerText.Trim(); 
                         editSize = Convert.ToInt32( nEditSize[i].InnerText.Trim());                       }

                       
                       
                       nUserFieldsMD.Item(0).InnerText.Trim().ToString();

                       string a = nodo.GetElementsByTagName("UserFieldsMD").Item(0).InnerText;
                       
                       XmlNodeList nValidValuesMD = nodo.GetElementsByTagName("ValidValuesMD");

                       foreach (XmlElement nodo2 in nValidValuesMD)
                       {
                           XmlNodeList nRows = nodo2.GetElementsByTagName("row");

                           
                           string valueRow = string.Empty;
                           string descriptionRow = string.Empty;
                           validValues = new Dictionary<string, string>();

                           foreach (XmlElement nRow in nRows)
                           {
                               XmlNodeList nValue = nRow.GetElementsByTagName("Value");
                               XmlNodeList nDescription = nRow.GetElementsByTagName("Description");

                               valueRow = nValue[i].InnerText.Trim();
                               descriptionRow = nDescription[i].InnerText.Trim();
               
                               validValues.Add(valueRow,descriptionRow);
                           }
                       }


                       Errors.Sb1Error sb1Error = userFields.CreateUserField
                           (
                           tableName, 
                           fieldName, 
                           fieldDescription, 
                           (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), mandatory), 
                           ((SAPbobsCOM.BoFieldTypes)Enum.Parse(typeof(SAPbobsCOM.BoFieldTypes), fieldType)), 
                           size, 
                           ((SAPbobsCOM.BoFldSubTypes)Enum.Parse(typeof(SAPbobsCOM.BoFldSubTypes), fieldSubType)), 
                           validValues, 
                           defaultValue, 
                           linkedTable, 
                           linkedUDO
                           );


                       if (sb1Error.Code == 0)
                           Forxap.Framework.UI.Messages.ShowSuccess("Campo : " + sb1Error.Message);
                       else if ((sb1Error.Code != -2035) && (sb1Error.Code != -1109)) 
                           Forxap.Framework.UI.Messages.ShowWarning("Error: " + sb1Error.Code + sb1Error.Message);
                   }



               }
               catch (Exception exception)
               {
                   Errors.Sb1Error sb1Error = Errors.GetLastErrorFromHRException(fieldName, exception); 
               }
               
           }
           else
           {
               Forxap.Framework.UI.Messages.ShowError("Error: " + xmlFile + " Archivo no existe");
           }

       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="xmlFile"></param>
       public static void LoadModuleMenuFromXmlFile(string xmlFile)
       {
           try
           {
               //  
               XmlDocument xmlDocument = new System.Xml.XmlDocument();
               string stringXML = string.Empty;

               xmlDocument.Load(xmlFile);
               stringXML = xmlDocument.InnerXml;

               oApplication.LoadBatchActions(ref stringXML);

               


               // seteo la imagen para el menu del modulo Factoring 
            //   SAPbouiCOM.MenuItem oMenuItem = Application.SBO_Application.Menus.Item("mix_factoring_module");
               // oMenuItem.Image = "factoring.png";

           }
           catch (Exception ex)
           {  
             oApplication.SetStatusBarMessage(ex.Message.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, true);
           }
       }


       
       /// <summary>
       /// lee campos de usuarios desde un archivo excel 
       /// </summary>
       /// <param name="xmlFile"></param>
       public static void LoadUserObjectFromXmlFile(string xmlFile)
       {

           XmlDocument xmlDocument = new XmlDocument();
           UserTables userTables = new UserTables();

           UserObjects userObjects = new UserObjects();

           List<string> childTables = null;

           // verificar que el archivo exista
           if (File.Exists(xmlFile))
           {
               // si el archivo existe entonces lo intenta leer
               xmlDocument.Load(xmlFile);


               /// cargo el nodo con toda la lista de los objetos
               XmlNodeList nodeListObjects = xmlDocument.GetElementsByTagName("UserObjects");

               ///obtengo una lista solos de los objetos
               XmlNodeList nodeListUserObjectMD = ((XmlElement)nodeListObjects[0]).GetElementsByTagName("UserObjectMD");

               /// recorro los nodos de una tabla
               foreach (XmlElement nodo in nodeListUserObjectMD)
               {

                   int i = 0;

                   XmlNodeList nCanCancel = nodo.GetElementsByTagName("CanCancel");
                   string canCancel = nCanCancel[i].InnerText.Trim();

                   XmlNodeList nCanClose = nodo.GetElementsByTagName("CanClose");
                   string canClose = nCanClose[i].InnerText.Trim();

                   XmlNodeList nCanDelete = nodo.GetElementsByTagName("CanDelete");
                   string canDelete = nCanDelete[i].InnerText.Trim();

                   XmlNodeList nCanFind = nodo.GetElementsByTagName("CanFind");
                   string canFind = nCanFind[i].InnerText.Trim();

                   XmlNodeList nCanYearTransfer = nodo.GetElementsByTagName("CanYearTransfer");
                   string canYearTransfer = nCanYearTransfer[i].InnerText.Trim();

                   XmlNodeList nCanApprove = nodo.GetElementsByTagName("CanApprove");
                   string canApprove = nCanApprove[i].InnerText.Trim();

                   XmlNodeList nCanArchive = nodo.GetElementsByTagName("CanArchive");
                   string canArchive = nCanArchive[i].InnerText.Trim();

                   XmlNodeList nCanLog = nodo.GetElementsByTagName("CanLog");
                   string canLog = nCanLog[i].InnerText.Trim();

                   XmlNodeList nObjectType = nodo.GetElementsByTagName("ObjectType");
                   string objectType = nObjectType[i].InnerText.Trim();

                   


                   XmlNodeList nTableName = nodo.GetElementsByTagName("TableName");
                   string tableName = nTableName[i].InnerText.Trim();

                   XmlNodeList nChildTables = nodo.GetElementsByTagName("ChildTables");
                   string childTable = nChildTables[i].InnerText.Trim();

                   

                   //XmlNodeList nChildTable2 = nodo.GetElementsByTagName("ChildTable2");
                   //string childTable2 = nChildTable1[i].InnerText.Trim();


                   

                   XmlNodeList nObjectCode = nodo.GetElementsByTagName("ObjectCode");
                   string objectCode = nObjectCode[i].InnerText.Trim();

                   XmlNodeList nObjectName = nodo.GetElementsByTagName("ObjectName");
                   string objectName = nObjectName[i].InnerText.Trim();

                   XmlNodeList nManageSeries = nodo.GetElementsByTagName("ManageSeries");
                   string manageSeries = nManageSeries[i].InnerText.Trim();

                   XmlNodeList nCanCreateDefaultForm = nodo.GetElementsByTagName("CanCreateDefaultForm");
                   string canCreateDefaultForm = nCanCreateDefaultForm[i].InnerText.Trim();

                   XmlNodeList nFatherMenuID = nodo.GetElementsByTagName("FatherMenuID");
                   int  fatherMenuID = Convert.ToInt32( nFatherMenuID[i].InnerText.Trim());

                   XmlNodeList nMenuID = nodo.GetElementsByTagName("MenuID");
                   string menuID = nMenuID[i].InnerText.Trim();

                   //XmlNodeList nMenuCaption = nodo.GetElementsByTagName("MenuCaption");
                   //string menuCaption = nMenuCaption[i].InnerText.Trim();


                   //XmlNodeList nChildTables = nodo.GetElementsByTagName("ChildTables");

                   

                   foreach (XmlElement nodo2 in nChildTables)
                   {
                       XmlNodeList nRows = nodo2.GetElementsByTagName("TableChildName");



                       
                       childTables = new List<string>();

                       foreach (XmlElement nRow in nRows)
                       {
                           string tableNameChild = string.Empty;

                           XmlNodeList nValue = nRow.GetElementsByTagName("TableName");


                           tableNameChild = nValue[i].InnerText.Trim();


                           childTables.Add(tableNameChild);
                       }
                   }



                   SAPbobsCOM.BoUDOObjType boUDOObjType = (SAPbobsCOM.BoUDOObjType)Enum.Parse(typeof(SAPbobsCOM.BoUDOObjType), objectType);
                   SAPbobsCOM.BoYesNoEnum  CanCancel = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), canCancel);
                   SAPbobsCOM.BoYesNoEnum  CanClose = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), canClose);
                   SAPbobsCOM.BoYesNoEnum  CanCreateDefaultForm = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), canCreateDefaultForm);
                   SAPbobsCOM.BoYesNoEnum  CanDelete = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), canDelete);
                   SAPbobsCOM.BoYesNoEnum  CanFind = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), canFind);
                   SAPbobsCOM.BoYesNoEnum  ManageSeries = (SAPbobsCOM.BoYesNoEnum)Enum.Parse(typeof(SAPbobsCOM.BoYesNoEnum), manageSeries); 
                   
                

                    userObjects.AddUDO
                       (
                       CanCancel,
                       CanClose,
                       CanCreateDefaultForm,
                       CanDelete,
                       CanFind,
                       ManageSeries,
                       objectCode,
                       objectName,
                       tableName,
                       "",
                       "",
                       childTables,
                       boUDOObjType

                       
                       );

          
               }

           }

           else
           {
               Forxap.Framework.UI.Messages.ShowError("Error: " + xmlFile + " Archivo no existe");
           }


       }


    }// fin de la clase

}// fin del namespace
