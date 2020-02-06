//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Extension Methods                             CLibForm.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   02-01-20  Started development.
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
public static class CLibTime
{


//============================================================================
//                                                                   IsTimeout
//----------------------------------------------------------------------------
public static Boolean IsTimeout (DateTime dtStart, int iMillisecs)
{
     long      lDiff ;

     DateTime       dtNow ;
     TimeSpan       tsSpan ;

// Calculate how many milliseconds have elapsed since start
     dtNow  = DateTime.Now ;
     lDiff  = dtNow.Ticks - dtStart.Ticks ;
     tsSpan = new TimeSpan (lDiff) ;
// Return true if duration exceeds timeout limit
     return (tsSpan.TotalMilliseconds > (double) iMillisecs) ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
