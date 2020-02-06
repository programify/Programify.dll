﻿//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Extension Methods                             CLibForm.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   30-12-19  Started development.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
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
public static class CLibExtension
{


//============================================================================
//                                                                         int
//----------------------------------------------------------------------------
public static Boolean IsEven (this int iValue) => ((iValue & 1) == 0) ;
public static Boolean IsOdd  (this int iValue) => ((iValue & 1) == 1) ;


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
