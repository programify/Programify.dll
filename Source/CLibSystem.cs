//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : System-level Functions                      CLibSystem.cs
//
//****************************************************************************

//****************************************************************************
//                                                                 Development
//****************************************************************************
/*
 *   18-01-20  Added to project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Programify
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
public class CLibSystem : IExceptionSafe
{

// Objects

// Structures

// Volatile Properties

private   volatile  CLibExceptions      moEx ;

//----------------------------------------------------------------- Properties

public    CLibExceptions      ExceptionInfo  { get => moEx ; }


//============================================================================
//                                                                  CLibSystem
//----------------------------------------------------------------------------
public CLibSystem ()
{
// Init exception handler
     moEx = new CLibExceptions () ;
}


//============================================================================
//                                                                  FileDelete
//----------------------------------------------------------------------------
public Boolean FileDelete (string strFilespec)
{
     Boolean   bfSuccess ;

// Ignore empties
     if (string.IsNullOrEmpty (strFilespec))
          return false ;

// Init
     bfSuccess = false ;
// Attempt to delete the CSV file
     try
     {
     // Indicate cause of exceptions
          moEx.Cause = Series.Cause.FileDelete ;
     // Attempt file system action
          File.Delete (strFilespec) ;
     // Success
          bfSuccess = moEx.Clear () ;
     }
     catch (ArgumentException ex)            { moEx.Handler (ex, Series.Exception.Argument) ;     }
     catch (DirectoryNotFoundException ex)   { moEx.Handler (ex, Series.Exception.DirNotFound) ;  }
     catch (PathTooLongException ex)         { moEx.Handler (ex, Series.Exception.PathTooLong) ;  }
     catch (IOException ex)                  { moEx.Handler (ex, Series.Exception.IO) ;           }
     catch (NotSupportedException ex)        { moEx.Handler (ex, Series.Exception.NotSupported) ; }
     catch (UnauthorizedAccessException ex)  { moEx.Handler (ex, Series.Exception.Access) ;       }

     return bfSuccess ;
}


//============================================================================
//                                                                FileInfoOpen
//----------------------------------------------------------------------------
public FileStream FileInfoOpen (string strFilespec, FileMode mode, FileAccess access, FileShare share)
{
     FileInfo       fileinfo ;
     FileStream     fstream ;

// Init
     fileinfo = null ;
     fstream  = null ;
// Attempt to locate the file
     try
     {
     // Indicate cause of exceptions
          moEx.Cause = Series.Cause.FileInfoNew ;
     // Attempt file system action
          fileinfo = new FileInfo (strFilespec) ;
     // Success
          moEx.Clear () ;
     }
     catch (ArgumentNullException ex)        { moEx.Handler (ex, Series.Exception.ArgumentNull) ; }
     catch (ArgumentException ex)            { moEx.Handler (ex, Series.Exception.Argument) ;     }
     catch (SecurityException ex)            { moEx.Handler (ex, Series.Exception.Security) ;     }
     catch (UnauthorizedAccessException ex)  { moEx.Handler (ex, Series.Exception.Access) ;       }
     catch (PathTooLongException ex)         { moEx.Handler (ex, Series.Exception.PathTooLong) ;  }
     catch (NotSupportedException ex)        { moEx.Handler (ex, Series.Exception.NotSupported) ; }
// Return if failed to locate file
     if (fileinfo == null)
          return null ;

// Attempt to gain access to the file
     try
     {
     // Indicate cause of exceptions
          moEx.Cause = Series.Cause.FileInfoOpen ;
     // Attempt file system action
          fstream = fileinfo.Open (mode, access, share) ;
     // Success
          moEx.Clear () ;
     }
     catch (FileNotFoundException ex)        { moEx.Handler (ex, Series.Exception.FileNotFound) ; }
     catch (UnauthorizedAccessException ex)  { moEx.Handler (ex, Series.Exception.Access) ;       }
     catch (DirectoryNotFoundException ex)   { moEx.Handler (ex, Series.Exception.DirNotFound) ;  }
     catch (IOException ex)                  { moEx.Handler (ex, Series.Exception.IO) ;           }

     return fstream ;
}


//============================================================================
//                                                             FileReadAllText
//----------------------------------------------------------------------------
public string FileReadAllText (string strFilespec, Encoding encoding)
{
     string    strText ;

// Init
     strText = null ;
// Attempt to read all text from the specified file
     try
     {
     // Indicate cause of exceptions
          moEx.Cause = Series.Cause.FileReadAllText ;
     // Attempt file system action
          strText  = File.ReadAllText (strFilespec, encoding) ;
     // Success
          moEx.Clear () ;
     }
     catch (ArgumentNullException ex)        { moEx.Handler (ex, Series.Exception.ArgumentNull) ; }
     catch (ArgumentException ex)            { moEx.Handler (ex, Series.Exception.Argument) ;     }
     catch (PathTooLongException ex)         { moEx.Handler (ex, Series.Exception.PathTooLong) ;  }
     catch (DirectoryNotFoundException ex)   { moEx.Handler (ex, Series.Exception.DirNotFound) ;  }
     catch (FileNotFoundException ex)        { moEx.Handler (ex, Series.Exception.FileNotFound) ; }
     catch (IOException ex)                  { moEx.Handler (ex, Series.Exception.IO) ;           }
     catch (UnauthorizedAccessException ex)  { moEx.Handler (ex, Series.Exception.Access) ;       }
     catch (NotSupportedException ex)        { moEx.Handler (ex, Series.Exception.NotSupported) ; }
     catch (SecurityException ex)            { moEx.Handler (ex, Series.Exception.Security) ;     }

     return strText ;
}


//============================================================================
//                                                             FileStreamWrite
//----------------------------------------------------------------------------
public Boolean FileStreamWrite (FileStream fstream, Byte[] abBuffer, Int32 iOffset, Int32 iLength)
{
// Guard against null pointers
     if (fstream == null)
          return false ;

// Attempt to write byte array to filestream
     try
     {
     // Indicate cause of exceptions
          moEx.Cause = Series.Cause.FileStreamWrite ;
     // Attempt file system action
          fstream.Write (abBuffer, iOffset, iLength) ;
     // Success
          moEx.Clear () ;
     }
     catch (ArgumentNullException ex)        { moEx.Handler (ex, Series.Exception.ArgumentNull) ;   }
     catch (ArgumentOutOfRangeException ex)  { moEx.Handler (ex, Series.Exception.ArgumentRange) ;  }
     catch (ArgumentException ex)            { moEx.Handler (ex, Series.Exception.Argument) ;       }
     catch (IOException ex)                  { moEx.Handler (ex, Series.Exception.IO) ;             }
     catch (NotSupportedException ex)        { moEx.Handler (ex, Series.Exception.NotSupported) ;   }
     catch (ObjectDisposedException ex)      { moEx.Handler (ex, Series.Exception.ObjectDisposed) ; }

     return true ;
}


//============================================================================
//                                                             AssemblyVersion
//----------------------------------------------------------------------------
public static string AssemblyVersion ()
{
     Assembly assem = Assembly.GetCallingAssembly () ;
     FileVersionInfo fvi = FileVersionInfo.GetVersionInfo (assem.Location) ;
     return $"{fvi.FileMajorPart}.{fvi.FileMinorPart}" ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
