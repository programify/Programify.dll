//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library                                              CLibLogFile.cs
//   Application Activity Log File
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   23-01-20  Added this module to the library.
 *   24-01-20  Installed constructor set and IExceptionSafe.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Programify
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
public class CLibLogFile : IExceptionSafe
{
//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

// Enums

// Objects

// Structures

// Locks

//-------------------------------------------------------- Volatile Properties

private   volatile  Boolean   mbfDate ;      // The "ccyymmss" date element will appear in the file name.
private   volatile  Boolean   mbfTime ;      // The "hhmmss" time element will appear in the file name.

private   volatile  char      mcSeparator ;  // String separating time stamp from filename and its elements.

private   volatile  Int32     miEntries ;    // Number of lines written to log file.

private   volatile  string    mstrExtension ;     // File name extension for log file.
private   volatile  string    mstrFilename ;      // File name prefix to log file.
private   volatile  string    mstrFilespec ;      // Fully qualified reference to log file.
private   volatile  string    mstrPath ;          // Path to log file.

private   volatile  CLibExceptions      moEx ;

//----------------------------------------------------------------- Properties

public    Boolean   DateInFilename { get => mbfDate ;   set { mbfDate = value ; NameChange () ; } }
public    Boolean   TimeInFilename { get => mbfTime ;   set { mbfTime = value ; NameChange () ; } }

public    char      Separator { get => mcSeparator ;    private set => mcSeparator   = value ; }

public    Int32     Entries   { get => miEntries ; }

public    string    Extension { get => mstrExtension ;  private set => mstrExtension = value ; }
public    string    Filename  { get => mstrFilename ;   private set => mstrFilename  = value ; }
public    string    Filespec  { get => mstrFilespec ;   private set => mstrFilespec  = value ; }
public    string    Path      { get => mstrPath ;       set => mstrPath = value ; }

// IExceptionSafe...

public    CLibExceptions      ExceptionInfo  { get => moEx ; }


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                                 CLibLogFile
//----------------------------------------------------------------------------
public CLibLogFile ()
{
     Constructor (null, null, "log", '_') ;
}

public CLibLogFile (char cSeparator)
{
     Constructor (null, null, "log", cSeparator) ;
}

public CLibLogFile (string strPath)
{
     Constructor (strPath, null, "log", '_') ;
}

public CLibLogFile (string strPath, char cSeparator)
{
     Constructor (strPath, null, "log", cSeparator) ;
}

public CLibLogFile (string strPath, string strFilename)
{
     Constructor (strPath, strFilename, "log", '_') ;
}

public CLibLogFile (string strPath, string strFilename, char cSeparator)
{
     Constructor (strPath, strFilename, "log", cSeparator) ;
}

public CLibLogFile (string strPath, string strFilename, string strExtension)
{
     Constructor (strPath, strFilename, strExtension, '_') ;
}

public CLibLogFile (string strPath, string strFilename, string strExtension, char cSeparator)
{
     Constructor (strPath, strFilename, strExtension, cSeparator) ;
}

//----------------------------------------------------------------------------
//                                                                 Constructor
//----------------------------------------------------------------------------
private void Constructor (string strPath, string strFilename, string strExtension, char cSeparator)
{
// Create exception handler
     moEx = new CLibExceptions () ;

// Init
     Filename  = strFilename ;
     Extension = strExtension ;
     Path      = strPath ;
     Separator = cSeparator ;
// Construct time stamped file specification
     DateInFilename = true ;
     TimeInFilename = true ;
}


//----------------------------------------------------------------------------
//                                                                  NameChange
//----------------------------------------------------------------------------
public void NameChange ()
{
     string    strTmStamp ;

// Construct time stamped file specification
     strTmStamp = GetTimeStamp (Separator) ;

     if (strTmStamp == null)
          Filespec   = $"{Path}{Filename}.{Extension}" ;
     else
          Filespec   = $"{Path}{Filename}{Separator}{strTmStamp}.{Extension}" ;

// Reset the number of entries written to the file
     miEntries = 0 ;
}


//============================================================================
//                                                                        Open
//----------------------------------------------------------------------------
public Boolean Open ()
{
     return true ;
}


//============================================================================
//                                                                       Write
//----------------------------------------------------------------------------
public Boolean Write (string strTitle, CLibExceptions oEx)
{
     if (! Write ($"{strTitle} Exception Event"))
          goto exit_method ;
     if (! Write ($"Cause     : {oEx.Cause}"))
          goto exit_method ;
     if (! Write ($"Title     : {oEx.Title}"))
          goto exit_method ;
     if (! Write ($"Detail    : {oEx.Detail}"))
          goto exit_method ;
     if (! Write ($"Exception : {oEx.Exception}"))
          goto exit_method ;
     if (! Write ($"Message   : {oEx.Message}"))
          goto exit_method ;
     Write ($"Count     : {oEx.Count}") ;

exit_method:

     return moEx.IsClear ;
}


//============================================================================
//                                                                       Write
//----------------------------------------------------------------------------
/*
 *   Write() commits a message to the application's current day log. This
 *   is a re-entrant function compatible with multiple concurrent threads.
 */
public Boolean Write (string strMessage)
{
     byte []   abData ;
     int       iLength ;
     string    strPreamble ;
     string    strThread ;
     string    strTime ;

     DateTime       dtNow ;
     FileStream     fsLog ;
     Thread         thCurr ;

// Exclusive access to the event log from multiple threads
     lock (this)
     {
     // Init
          fsLog  = null ;
          thCurr = Thread.CurrentThread ;

     // Fix if message is null
          if (String.IsNullOrEmpty (strMessage))
               strMessage = "" ;

     // Note time log was appended to
          dtNow     = DateTime.Now ;
          strTime   = $"{dtNow.Hour:D02}:{dtNow.Minute:D02}:{dtNow.Second:D02}" ;
          strThread = $"{thCurr.ManagedThreadId:X04}" ;

     // Create/Open-Append application's log file
          try
          {
               moEx.Cause = Series.Cause.FileOpen ;
               fsLog = File.Open (Filespec, FileMode.Append, FileAccess.Write, FileShare.Read) ;

          // Check if committing first entry
               if (miEntries == 0)
                    strPreamble = "\r\n" ;
               else
                    strPreamble = "" ;

          // Convert string to byte array
               abData  = Encoding.ASCII.GetBytes ($"{strPreamble}{strTime}  {strThread}  {strMessage}\r\n") ;
               iLength = abData.Length ;

          // Check if logging is enabled from user preferences
               moEx.Cause = Series.Cause.FileWrite ;
               fsLog.Write (abData, 0, iLength) ;

               moEx.Cause = Series.Cause.FileFlush ;
               fsLog.Flush () ;

               moEx.Cause = Series.Cause.FileClose ;
               fsLog.Close () ;

               miEntries ++ ;
               moEx.Clear () ;
          }
          catch (ArgumentOutOfRangeException ex)  { moEx.Handler (ex, Series.Exception.ArgumentRange) ; }
          catch (ArgumentException           ex)  { moEx.Handler (ex, Series.Exception.Argument) ;      }
          catch (PathTooLongException        ex)  { moEx.Handler (ex, Series.Exception.PathTooLong) ;   }
          catch (DirectoryNotFoundException  ex)  { moEx.Handler (ex, Series.Exception.DirNotFound) ;   }
          catch (IOException                 ex)  { moEx.Handler (ex, Series.Exception.IO) ;            }
          catch (UnauthorizedAccessException ex)  { moEx.Handler (ex, Series.Exception.Access) ;        }
          catch (NotSupportedException       ex)  { moEx.Handler (ex, Series.Exception.NotSupported) ;  }

          finally
          {
               fsLog?.Dispose () ;
          }

          if (moEx.CaughtException)
          {
          // Report error in detail
               moEx.Message = $"Cause: {moEx.Cause}.\r\nException: {moEx.Exception}\r\nDetail: {moEx.Detail}" ;
               Console.WriteLine ($"*** {strTime:8}  {moEx.Message}") ;
          }

     // Always send log entries to console output (immediate debugging)
          Console.WriteLine ($"*** {strTime:8}  {strMessage}") ;
     }

     return moEx.IsClear ;
}


//============================================================================
//                                                                GetTimeStamp
//----------------------------------------------------------------------------
public string GetTimeStamp (char cSeparator)
{
     string    strDate ;
     string    strTime ;
     string    strTmStamp ;

     DateTime  dtNow ;

// Init
     dtNow      = DateTime.Now ;
     strTmStamp = null ;

// Construct "CCYYMMDD_HHMMSS" time stamp string
     strDate = $"{dtNow.Year:d04}{dtNow.Month:d02}{dtNow.Day:d02}" ;
     strTime = $"{dtNow.Hour:d02}{dtNow.Minute:d02}{dtNow.Second:d02}" ;

// Construct stamp variations
     if (DateInFilename && TimeInFilename)
          strTmStamp = $"{strDate}{cSeparator}{strTime}" ;
     if (DateInFilename && ! TimeInFilename)
          strTmStamp = strDate ;
     if (! DateInFilename && TimeInFilename)
          strTmStamp = strTime ;

     return strTmStamp ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
