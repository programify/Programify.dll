//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Class Interfaces                        CLibInterfaces.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   24-01-20  Added this module to the project.
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
//                                                                  Interfaces
//****************************************************************************


//------------------------------------------------------------- IExceptionSafe
/*
 *   For a class to support this interface, the following features will
 *   need to be coded:
 *
 *   // Define the class has the 'IExceptionSafe' interface:
 *
 *        public class CMyClass : IExceptionSafe
 *
 *   // Define a private, volatile 'CLibExceptions' object called "moEx".
 *
 *        private  volatile  CLibExceptions  moEx ;
 *
 *   // Define a property to return the 'CLibExceptions' object:
 *
 *        public  CLibExceptions  ExceptionInfo  { get => moEx ; }
 *
 *   // In the class contructor, create the exceptions handler object
 *        moEx = new CLibExceptions () ;
 */
public interface IExceptionSafe
{

// Properties

     CLibExceptions      ExceptionInfo  { get ; }
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
