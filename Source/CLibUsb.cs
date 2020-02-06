//****************************************************************************
//
//   (c) 2020, Programify Ltd.
//   All rights reserved.
//   Class Library : USB Functions                                  CLibUsb.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   28-12-19  Added this module to the project.
 *
 *   v1.3
 *   06-02-20  GetUsbDeviceInfo() now releases resources correctly.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Additional references which must be added to the project
using Microsoft.Win32;
using System.Management;
using System.Security;


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Programify
{


//****************************************************************************
//                                                                       Class
//****************************************************************************
public class CLibUsb
{
//--------------------------------------------------------------------- Public

//-------------------------------------------------------------------- Private

//--------------------------------------------------------- Property Volatiles

private   volatile string     mstrErrMessage ;

//--------------------------------------------------------------- Enumerations

private   volatile Series.Cause         meErrCause ;
private   volatile Series.Exception     meErrException ;

//-------------------------------------------------------------------- Objects

//---------------------------------------------------------------------- Locks

//-------------------------------------------------------------------- Structs

//----------------------------------------------------------------- Properties

public    string    ErrMessage     { get => mstrErrMessage ; set => mstrErrMessage = value ; }

private   Series.Cause        ErrCause     { get => meErrCause ;     set => meErrCause     = value ; }
private   Series.Exception    ErrException { get => meErrException ; set => meErrException = value ; }

//------------------------------------------------------------ InteropServices


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                            GetUsbDeviceInfo
//----------------------------------------------------------------------------
/*
 *   GetUsbDeviceInfo() attempts to locate a USB device by its name. The name
 *   provided must be located at the start of the 'Name' property and as such
 *   should be unique enough to identify the desired device or its class of
 *   devices.
 */
public static ManagementObject GetUsbDeviceInfo (string strNeedle)
{
     string    strValue ;

     ManagementObject          manobj ;
     ManagementObjectSearcher  searcher ;
     ObjectQuery               objquery ;

// Init
     manobj = null ;
// Construct a query for Plug and Play devices in the "Ports" class
     objquery = new ObjectQuery ("select * from WIN32_PnPEntity where PNPClass=\"Ports\"") ;
// Open an object searcher using the query
     searcher = new ManagementObjectSearcher (objquery) ;

// Enumerate objects that have been selected by the query
     foreach (ManagementObject obj in searcher.Get ())
     {
     // Extract the device's full name
          try
          {
               strValue = obj ["Name"].ToString () ;
          }
          catch (ManagementException)  { continue ; }
     // Check if found a device whose Name property contains the string haystack
          if (strValue.StartsWith (strNeedle, StringComparison.InvariantCultureIgnoreCase))
          {
               manobj = obj ;
               break ;
          }
     }
// Release resources
     searcher.Dispose () ;
     return manobj ;
}


//============================================================================
//                                                            GetUsbDeviceName
//----------------------------------------------------------------------------
/*
 *   GetUsbDeviceName() returns the full name of a USB device presenting 
 *   itself as a COM port. Supply the COM port name to return its full name
 *   which can be used by GetUsbDeviceInfo() to return a management object for
 *   the device.
 *
 *   This function uses LINQ to access system information usually viewed 
 *   through the Device Manager application.
 *
 *   See https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-pnpentity
 */
public static string GetUsbDeviceName (string strComPort)
{
     string    strName ;
     string    strCom ;

     ManagementObjectSearcher  searcher ;
     ObjectQuery               objquery ;

// Construct target string, e.g. "(COM23")
     strCom  = $"({strComPort})" ;
     strName = null ;

// Construct a query for Plug and Play devices in the "Ports" class
     objquery = new ObjectQuery ("select * from WIN32_PnPEntity where PNPClass=\"Ports\"") ;
// Open an object searcher using the query
     searcher = new ManagementObjectSearcher (objquery) ;

// Enumerate objects that have been selected by the query
     foreach (ManagementObject manobj in searcher.Get ())
     {
     // Extract the device's full name
          try
          {
               strName = manobj ["Name"].ToString () ;
          }
          catch (ManagementException)  { continue ; }
     // Check if full name contains the COM port identifier
          if (strName.Contains (strCom))
          {
               //Console.WriteLine (manobj ["Name"]);               // USB-SERIAL CH340 (COM4)
               //Console.WriteLine (manobj ["Description"]);        // USB-SERIAL CH340
               //Console.WriteLine (manobj ["DeviceID"]);           // USB\VID_1A86&PID_7523\5&2B52A5BA&0&4
               break ;
          }
     }
// Release resources
     searcher.Dispose () ;
// Return full device name or null
     return strName ;
}


//============================================================================
//                                                           GetPropertyString
//----------------------------------------------------------------------------
public static string GetPropertyString (ManagementObject oProperty, string strKey)
{
     object    mbobj ;

// Throw exception if management object reference is null
     if (oProperty == null)
          throw new ArgumentNullException ($"oProperty", Resources.Strings.ExNullParam) ;

// Attempt to get the property of the management object using the supplied key
     try
     {
          mbobj = oProperty.GetPropertyValue (strKey) ;
     }
     catch (ManagementException)
     {
          mbobj = null ;
     }
// Intercept values that are null and will fail to translate to a string
     if (mbobj == null)
          return null ;
// Return the value of the property as a string
     return mbobj.ToString () ;
}


//============================================================================
//                                                          GetDeviceAttribute
//----------------------------------------------------------------------------
// Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\USB\VID_2341&PID_804D&MI_00\6&32a6ff44&0&0000
// manobj.Path.RelativePath = "Win32_PnPEntity.DeviceID="USB\\VID_2341&PID_804D&MI_00\\6&32A6FF44&0&0000""
public static string GetDeviceAttribute (ManagementObject oManObj, string strKey)
{
     Int32     iStart ;
     string    strDeviceId ;
     string    strPath ;
     string    strValue ;

     RegistryKey    rkBase ;
     RegistryKey    rkEntry ;

     char [] acQuote = { '"' } ;

// Throw exception if management object reference is null
     if (oManObj == null)
          throw new ArgumentNullException ($"oManObj", Resources.Strings.ExNullParam) ;

// Init
     strValue = null ;
     rkBase   = null ;
     rkEntry  = null ;
// Access the registry path "Win32_PnPEntity.DeviceID="USB\\VID_xxxx&PID_xxxx&MI_xx\\x&xxxxxxxxxx&x&xxxx""
     strPath = $"{oManObj.Path.RelativePath}" ;
     if (! strPath.StartsWith ("Win32_PnPEntity", StringComparison.InvariantCultureIgnoreCase))
          goto exit_function ;

// Extract the embedded registry path to the physical device being plugged in
     iStart = strPath.IndexOf ("DeviceID=", StringComparison.InvariantCultureIgnoreCase) ;
     if (iStart < 15)
          goto exit_function ;

     strDeviceId = strPath.Substring (iStart + 10) ;
     strDeviceId = strDeviceId.TrimEnd (acQuote) ;
// Locate entry in Registry
     rkBase  = RegistryKey.OpenBaseKey (RegistryHive.LocalMachine, RegistryView.Default) ;
     rkEntry = rkBase.OpenSubKey ($"SYSTEM\\CurrentControlSet\\Enum\\{strDeviceId}") ;
// Get existing value
     strValue = rkEntry.GetValue (strKey).ToString () ;

exit_function:

// Release resources
     if (rkBase != null)
          rkBase.Dispose () ;
     if (rkEntry != null)
          rkEntry.Dispose () ;
// Return value of USB device's attribute
     return strValue ;
}


//============================================================================
//                                                          SetDeviceAttribute
//----------------------------------------------------------------------------
// Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\USB\VID_2341&PID_804D&MI_00\6&32a6ff44&0&0000
// manobj.Path.RelativePath = "Win32_PnPEntity.DeviceID="USB\\VID_2341&PID_804D&MI_00\\6&32A6FF44&0&0000""
public Boolean SetDeviceAttribute (ManagementObject oManObj, string strKey, string strValue)
{
     Boolean   bfSuccess ;
     Int32     iStart ;
     string    strDeviceId ;
     string    strPath ;

     RegistryKey    rkBase ;
     RegistryKey    rkEntry ;

     char [] acQuote = { '"' } ;

// Throw exception if management object reference is null
     if (oManObj == null)
          throw new ArgumentNullException ($"oManObj", Resources.Strings.ExNullParam) ;

// Init
     bfSuccess  = false ;
     rkBase     = null ;
     rkEntry    = null ;
     ErrMessage = null ;
// Returns "Win32_PnPEntity.DeviceID="USB\\VID_2341&PID_804D&MI_00\\6&32A6FF44&0&0000""
     strPath = $"{oManObj.Path.RelativePath}" ;
     if (! strPath.StartsWith ("Win32_PnPEntity", StringComparison.InvariantCultureIgnoreCase))
          goto exit_function ;

     iStart = strPath.IndexOf ("DeviceID=", StringComparison.InvariantCultureIgnoreCase) ;
     if (iStart < 15)
          goto exit_function ;

     strDeviceId = strPath.Substring (iStart + 10) ;
     strDeviceId = strDeviceId.TrimEnd (acQuote) ;
// Locate entry in Registry
     try
     {
          ErrCause = Series.Cause.OpenBaseKey ;
          rkBase  = RegistryKey.OpenBaseKey (RegistryHive.LocalMachine, RegistryView.Default) ;
     }
     catch (ArgumentException ex)           { ErrMessage = ex.Message ; ErrException = Series.Exception.Argument ; }
     catch (SecurityException ex)           { ErrMessage = ex.Message ; ErrException = Series.Exception.Security ; }
     catch (UnauthorizedAccessException ex) { ErrMessage = ex.Message ; ErrException = Series.Exception.Access ; }

// Check if an exception was raised
     if (ErrException != Series.Exception.None)
          goto exit_function ;

// Create subkey in Windows Registry
     try
     {
          ErrCause = Series.Cause.CreateSubKey ;
          rkEntry = rkBase.CreateSubKey ($"SYSTEM\\CurrentControlSet\\Enum\\{strDeviceId}") ;
     }
     catch (ArgumentNullException ex)       { ErrMessage = ex.Message ; ErrException = Series.Exception.ArgumentNull ; }
     catch (ObjectDisposedException ex)     { ErrMessage = ex.Message ; ErrException = Series.Exception.ObjectDisposed ; }
     catch (SecurityException ex)           { ErrMessage = ex.Message ; ErrException = Series.Exception.Security ; }
     catch (UnauthorizedAccessException ex) { ErrMessage = ex.Message ; ErrException = Series.Exception.Access ; }

// Check if an exception was raised
     if (ErrException != Series.Exception.None)
          goto exit_function ;

// Get existing value
     try
     {
          ErrCause = Series.Cause.RegistrySetValue ;
          rkEntry.SetValue (strKey, strValue) ;
     }
     catch (ArgumentNullException ex)       { ErrMessage = ex.Message ; ErrException = Series.Exception.ArgumentNull ; }
     catch (NullReferenceException ex)      { ErrMessage = ex.Message ; ErrException = Series.Exception.NullReference ; }
     catch (SecurityException ex)           { ErrMessage = ex.Message ; ErrException = Series.Exception.Security ; }
     catch (UnauthorizedAccessException ex) { ErrMessage = ex.Message ; ErrException = Series.Exception.Access ; }

// Check if no exceptions raised
     if (ErrException == Series.Exception.None)
     {
          ErrCause  = Series.Cause.None ;
          bfSuccess = true ;
     }

exit_function:

// Release resources
     if (rkBase != null)
          rkBase.Dispose () ;
     if (rkEntry != null)
          rkEntry.Dispose () ;
// Indicate success of this function
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
