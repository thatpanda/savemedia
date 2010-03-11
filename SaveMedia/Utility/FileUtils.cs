﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Utility
{
    static class FileUtils
    {
        public static String FilenameCheck( String aFilename )
        {
            aFilename = aFilename.Replace( "\\", "" );
            aFilename = aFilename.Replace( "/", "" );
            aFilename = aFilename.Replace( ":", "" );
            aFilename = aFilename.Replace( "*", "" );
            aFilename = aFilename.Replace( "?", "" );
            aFilename = aFilename.Replace( "\"", "'" );
            aFilename = aFilename.Replace( "<", "" );
            aFilename = aFilename.Replace( ">", "" );
            aFilename = aFilename.Replace( "|", "" );

            foreach( char theInvalidChar in System.IO.Path.GetInvalidFileNameChars() )
            {
                aFilename = aFilename.Replace( theInvalidChar.ToString(), "" );
            }

            if( String.IsNullOrEmpty( aFilename ) )
            {
                return "untitled";
            }

            return aFilename;
        }

        public static void DeleteFile( String aFilePath )
        {
            if( !String.IsNullOrEmpty( aFilePath ) &&
                System.IO.File.Exists( aFilePath ) )
            {
                try
                {
                    System.IO.File.Delete( aFilePath );
                }
                catch( System.Exception /*ex*/ )
                {
                    // TODO: write error to log
                }
            }
        }

        public static String SaveFile( String aFilename, String aFilter, System.Windows.Forms.Form aOwner )
        {
            aFilename = FilenameCheck( aFilename );

            SaveFileDialog theSaveFileDialog = new SaveFileDialog();
            theSaveFileDialog.AddExtension = true;
            theSaveFileDialog.CheckFileExists = false;
            theSaveFileDialog.CheckPathExists = true;
            theSaveFileDialog.CreatePrompt = false;
            theSaveFileDialog.FileName = aFilename;
            theSaveFileDialog.Filter = aFilter;
            theSaveFileDialog.FilterIndex = 1;
            theSaveFileDialog.OverwritePrompt = true;
            theSaveFileDialog.RestoreDirectory = true;
            theSaveFileDialog.SupportMultiDottedExtensions = true;
            theSaveFileDialog.ValidateNames = true;

            if( theSaveFileDialog.ShowDialog( aOwner ) == DialogResult.OK )
            {
                return theSaveFileDialog.FileName;
            }

            return String.Empty;
        }

        public static System.Collections.Generic.Dictionary<String, String>
        ReadXml( String aFilePath )
        {
            System.Collections.Generic.Dictionary< String, String > theNodes = new System.Collections.Generic.Dictionary<String, String>(); ;

            if( System.IO.File.Exists( aFilePath ) )
            {
                System.Xml.XmlTextReader theReader = null;

                try
                {
                    theReader = new System.Xml.XmlTextReader( aFilePath );
                    String theElement = String.Empty;

                    while( theReader.Read() )
                    {
                        switch( theReader.NodeType )
                        {
                            case System.Xml.XmlNodeType.Element:
                                theElement = theReader.Name;
                                break;
                            case System.Xml.XmlNodeType.Text:
                                theNodes.Add( theElement, theReader.Value );
                                break;
                            case System.Xml.XmlNodeType.EndElement:
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if( theReader != null )
                    {
                        theReader.Close();
                    }
                }
            }

            return theNodes;
        }

        public static void WriteXml( String aFilePath, ref System.Collections.Generic.Dictionary<String, String> aNodes )
        {
            System.Xml.XmlTextWriter theWriter = null;

            try
            {
                theWriter = new System.Xml.XmlTextWriter( aFilePath, System.Text.Encoding.UTF8 );

                theWriter.Formatting = System.Xml.Formatting.Indented;

                theWriter.WriteStartDocument();
                theWriter.WriteStartElement( "SaveMedia" );

                foreach( System.Collections.Generic.KeyValuePair< String, String > thePair in aNodes )
                {
                    theWriter.WriteStartElement( thePair.Key );
                    theWriter.WriteString( thePair.Value );
                    theWriter.WriteEndElement();
                }

                theWriter.WriteEndElement();
                theWriter.WriteEndDocument();
            }
            catch
            {
            }
            finally
            {
                if( theWriter != null )
                {
                    theWriter.Close();
                }
            }
        }
    }
}
