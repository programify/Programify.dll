//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Enumerated Series                            CLibEnums.cs
//
//****************************************************************************

//****************************************************************************
//                                                                 Development
//****************************************************************************
/*
 *   21-12-19  Added to project.
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
public static class Series
{

// Which function or method gave rise to the error
public enum Cause
{
     None                     =  0,
     CreateDirectory          =  1,
     CreateSubKey             = 12,
     CreateWebRequest         = 14,
     DirEnumFiles             = 21,
     ExecuteProcess           = 10,        // An external process invoked by the application.
     FileClose                = 31,
     FileCopy                 =  2,
     FileDelete               = 17,
     FileFlush                = 30,
     FileInfoNew              = 18,
     FileInfoOpen             = 19,
     FileOpen                 = 16,
     FileRead                 =  3,
     FileReadAllText          = 22,
     FileStreamWrite          = 20,
     FileWrite                = 29,
     FtpWebResponse           =  5,
     GetPropertyValue         =  6,
     GetResponseFtpWebReq     =  4,
     IndexOf                  = 37, //
     IsWow64Process           =  7,
     OpenBaseKey              = 13,
     ReadBytes                = 15,
     RegistrySetValue         = 11,
     SerialPort               = 32,
     SerialPortClose          = 36,
     SerialPortOpen           = 35,
     SerialPortWrite          = 33,
     SessionCreateDir         = 24,
     SessionFileExists        = 34,
     SessionGetFiles          = 28,
     SessionNew               = 27,
     SessionOpen              = 26,
     SessionPutFiles          = 25,
     SmtpClientSend           = 23,
     Wow64Disable             =  8,
     PnPUtil                  =  9
}

// Standard Exceptions
public enum Exception
{
     None           =  0,
     Access         =  1,
     Argument       =  2,
     ArgumentNull   =  3,
     ArgumentRange  = 17,
     PathTooLong    =  4,
     DirNotFound    =  5,
     InvalidOp      =  6,
     IO             =  7,
     FileNotFound   =  8,
     Format         = 18,
     Management     = 16,
     NotSupported   =  9,
     NullReference  = 14,
     ObjectDisposed = 15,
     Security       = 10,
     SessionLocal   = 20,
     SessionRemote  = 21,
     Smtp           = 19,
     Timeout        = 22, //
     UriFormat      = 11,
     Web            = 12,
     Win32          = 13
}

public enum SwitchId
{
     Culture,
     IgnoreArgs,
     Verbose,
     Unknown
}

public enum DeviceCap
{
    VERTRES        = 10,
    DESKTOPVERTRES = 117

// See: http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
