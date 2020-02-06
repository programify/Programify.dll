//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Common Functions                            CLibCommon.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   17-01-20  Added this module to the project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
public static class CLibCommon
{
//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

// Static

private static string mstrErrMessage ;

// Enums

// Objects

// Locks

//-------------------------------------------------------- Volatile Properties

// Static

// Enums

// Objects

//----------------------------------------------------------------- Properties
/*
 *   ErrorMessage        Error message string generated during exception event.
 */

public static  string    ErrorMessage  { get => mstrErrMessage ; }


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                              SetCultureInfo
//----------------------------------------------------------------------------
public static Boolean SetCultureInfo (out CultureInfo oCulture, string strCulture)
{
     Boolean   bfSuccess ;

// Init
     bfSuccess = false ;
     oCulture  = null ;
// Create object on heap
     try
     {
          oCulture  = new CultureInfo (strCulture) ;
          bfSuccess = true ;
     }
     catch (ArgumentNullException ex)     { mstrErrMessage = ex.Message ; }
     catch (CultureNotFoundException ex)  { mstrErrMessage = ex.Message ; }

     return bfSuccess ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
