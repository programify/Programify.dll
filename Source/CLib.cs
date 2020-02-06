//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library
//   Versioning & Release Notes                                        CLib.cs
//
//****************************************************************************

//****************************************************************************
//                                                          Installed Packages
//****************************************************************************
/*
 *   Microsoft.CodeAnalysis.FxCopAnalyzers
 *   v2.9.8
 *
 *   Microsoft recommended code quality rules and .NET API usage rules, 
 *   including the most important FxCop rules, implemented as analyzers using 
 *   the .NET Compiler Platform (Roslyn). These analyzers check your code for 
 *   security, performance, and design issues, among others. The documentation 
 *   for FxCop analyzers can be found at:
 *
 *   https://docs.microsoft.com/visualstudio/code-quality/install-fxcop-analyzers
 */

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   VERSIONS
 *   ========
 *
 *        Product Name : Programify.dll
 *
 *        Release      Product     Size
 *        Date         Version    Bytes
 *        ===========  =======  =======
 *        20-JAN-2020  1.0       44,032
 *        31-JAN-2020  1.1
 *
 *
 *   RELEASE NOTES
 *   =============
 *
 *   20-01-20  v1.0.
 *             Initial live release.
 *
 *   31-01-20  v1.1.
 *             Forward compatible to using applications.
 *             CLibExceptions : Installed ShowMessageBox() method.
 *
 *   04-02-20  v1.2.
 *             Added software ReleaseDate and VersionCode properties.
 *
 *   06-02-20  v1.3.
 *             Installed FxCopAnalyzers package.
 *             Began modifying code to bring up to FxCop standard.
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
public static class CLib
{


//----------------------------------------------------------------- Properties

public static  string    ReleaseDate    { get => "200204" ; }
public static  string    VersionCode    { get => "1.2" ; }


//****************************************************************************
//                                                                     Methods
//****************************************************************************



//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
