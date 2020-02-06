//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library
//   Active structures                                          CLibStructs.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   26-01-20  Added this module to the project.
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
public class CLibStructs
{
}


//****************************************************************************
//                                                                     Structs
//****************************************************************************


//============================================================================
//                                                                    Bitmap32
//----------------------------------------------------------------------------
/*
 *   Bitmap32 defines a DWORD data type which operates as an array of 32 bits
 *   exposed as Boolean flags each accessible via a bit index. E.g.
 *
 *        Bitmap32 bmCode = new Bitmap32 (0) ;
 *        bmCode [5]   = false ;
 *        bmCode [21] ^= true ;
 *        strReport = $"Bitmap is 0b{bmCode}" ;
 */
struct Bitmap32
{
     private   Int32     siMap ;

// Constructor
     public Bitmap32 (Int32 iInitValue) => siMap = iInitValue ;

// Indexer access to bits inside Int32
     public Boolean this [ Int32 iIndexer ]
     {
          get
          {
               return (siMap & (1 << iIndexer)) != 0 ;
          }

          set
          {
               if (value)
                    siMap |= (1 << iIndexer) ;
               else
                    siMap &= ~(1 << iIndexer) ;
          }
     }

// Report field as a series of bits
     public override string ToString ()
     {
          return (Convert.ToString (siMap, 2)) ;
     }
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
