//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library
//   Exceptions Handling Class                               CLibExceptions.cs
//
//****************************************************************************

//****************************************************************************
//                                                                 Development
//****************************************************************************
/*
 *   24-01-20  Added to library.
 *   31-01-20  Installed ShowMessageBox().
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Programify
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
public class CLibExceptions
{

// Objects

// Structures

// Volatile Properties

private   volatile  Int32     iCount ;            // Number of times Handler has been called since object was created.

private   volatile  string    mstrErrDetail ;
private   volatile  string    mstrErrMessage ;
private   volatile  string    mstrErrTitle ;

private   volatile  Series.Cause        meErrCause ;
private   volatile  Series.Exception    meErrException ;

//----------------------------------------------------------------- Properties

public    Boolean   CaughtException     { get => (Exception != Series.Exception.None) ; }
public    Boolean   IsClear             { get => (Exception == Series.Exception.None) ; }

public    Int32     Count          { get => iCount ; }

public    string    Detail         { get => mstrErrDetail ;  set => mstrErrDetail  = value ; }
public    string    Message        { get => mstrErrMessage ; set => mstrErrMessage = value ; }
public    string    Title          { get => mstrErrTitle ;   set => mstrErrTitle   = value ; }

public    Series.Cause        Cause       { get => meErrCause ;     set => meErrCause     = value ; }
public    Series.Exception    Exception   { get => meErrException ; set => meErrException = value ; }


//============================================================================
//                                                              CLibExceptions
//----------------------------------------------------------------------------
public CLibExceptions ()
{
// Init
     Clear () ;

     iCount = 0 ;
}


//============================================================================
//                                                                       Clear
//----------------------------------------------------------------------------
public Boolean Clear ()
{
// Reset indicators
     Cause     = Series.Cause.None ;
     Exception = Series.Exception.None ;
// Commentary fields
     Detail    = "" ;
     Message   = "" ;
     Title     = "" ;

     return true ;
}


//============================================================================
//                                                                     Handler
//----------------------------------------------------------------------------
/*
 *   Handler() provides a generic exception handler.
 *
 *   The calling block of code must call Cause() to indicate the action 
 *   which may give rise to an exception event. The calling try/catch blocks 
 *   must also use the 'finally' mechanism to call Clear().
 */
public void Handler (Exception except, Series.Exception eExcept)
{
     Type      exception ;

// Ignore if no exception object
     if (except == null)
          return ;

     exception = except.GetType () ;

// Pass information to the executive level for reporting
     Exception = eExcept ;
     Title     = exception.FullName ;
     Message   = except.Message ;
     Detail    = $"{Cause.ToString ()} : {eExcept.ToString ()}" ;

// Count times called (with an non-null exception
     iCount ++ ;
}


//============================================================================
//                                                              ShowMessageBox
//----------------------------------------------------------------------------
public void ShowMessageBox (string strContext)
{
     string    strText ;

// Construct message text based on exception
     strText = $"Cause : {Cause}\r\n" +
               $"Exception : {Exception}\r\n" +
               $"Message : {Message}\r\n" +
               $"Title : {Title}" ;

     MessageBox.Show (strText, strContext, MessageBoxButtons.OK, MessageBoxIcon.Error) ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
