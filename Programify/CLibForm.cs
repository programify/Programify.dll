//****************************************************************************
//
//   (c) Programify Ltd
//   Class Library : Form Functions                                CLibForm.cs
//
//****************************************************************************

//****************************************************************************
//                                                                Developments
//****************************************************************************
/*
 *   21-12-19  Added this module to the project.
 */

//----------------------------------------------------------------------------
//                                                         Compiler References
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;                        // Reference: System.Drawing
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;                  // Reference: System.Windows.Forms


//****************************************************************************
//                                                                   Namespace
//****************************************************************************
namespace Programify
{


//------------------------------------------------------------ InteropServices

internal static class NativeMethods
{        
     [DllImport("gdi32.dll")]
     public static extern int GetDeviceCaps (IntPtr hdc, int nIndex) ;
}


//****************************************************************************
//                                                                       Class
//****************************************************************************
public class CLibForm
{


//****************************************************************************
//                                                                     Methods
//****************************************************************************


//============================================================================
//                                                               FixFormHeight
//----------------------------------------------------------------------------
/*
 *   FixFormHeight() corrects the visible form height of a form after the
 *   Windows O/S may have incorrecty resized the form due to a difference in
 *   the DPI of the screen on which the software was developed and the target
 *   screen on which the application runs.
 *
 *   oForm          The form object to be corrected.
 *
 *   fHeight        Specifies the intended design height of the form in inches.
 *                  This should be supplied as a floating point number up to 
 *                  eight decimal places if needed.
 *
 *   Returns false if the form object is null, otherwise true is returned.
 */
public static Boolean FixFormHeight (Form oForm, float fHeight)
{
     float     fDpiY ;
     float     fMeasure ;

     Graphics  graphics ;
     Size      newsize ;

// Ignore if form object not present
     if (oForm == null)
          return false ;
// Get DPI of the current monitor
     graphics = oForm.CreateGraphics () ;
     fDpiY = graphics.DpiY ;
     graphics.Dispose () ;
// Calculate pixel height given physical height and monitor's DPI
     fMeasure = fHeight * fDpiY ;
// Modify forms height to match
     newsize  = oForm.ClientSize ;
     newsize.Height = (Int32) fMeasure ;
     oForm.ClientSize = newsize ;
     return true ;
}


//============================================================================
//                                                            GetScalingFactor
//----------------------------------------------------------------------------
/*
 *   GetScalingFactor() returns a float value which represents the scaling
 *   factor, where 1.0 = 100%, 1.2 = 120%, etc.
 *
 *        hWindow        Handle to the window 
 *
 *                       Set this to IntPtr.Zero to retrieve the scaling
 *                       factor for the current monitor.
 */
public static float GetScalingFactor (IntPtr hWindow)
{
     float     fScaling ;
     int       iLogScrHi ;
     int       iPhyScrHi ;

     Graphics  graphics ;
     IntPtr    hDesktop ;

     graphics  = Graphics.FromHwnd (hWindow) ;
     hDesktop  = graphics.GetHdc () ;
     iLogScrHi = NativeMethods.GetDeviceCaps (hDesktop, (int) Series.DeviceCap.VERTRES) ;
     iPhyScrHi = NativeMethods.GetDeviceCaps (hDesktop, (int) Series.DeviceCap.DESKTOPVERTRES) ;
     graphics.Dispose () ;

     fScaling = (float) iPhyScrHi / (float) iLogScrHi ;
     return fScaling ;
}


//============================================================================
//                                                            ConvertDpiMetric
//----------------------------------------------------------------------------
public static int ConvertDpiMetric (Form oForm, int iMetric, int iOrigDpi)
{
     float     fDpiY ;
     float     fFactor ;
     float     fMetric ;
     float     fNewMetric ;
     float     fOrigDpi ;

     Graphics  graphics ;

// Get DPI of the current monitor
     graphics = oForm.CreateGraphics () ;
     fDpiY = graphics.DpiY ;
     graphics.Dispose () ;
// Calculate new metric based on DPI
     fMetric    = iMetric ;
     fOrigDpi   = iOrigDpi ;
     fFactor    = fMetric / fOrigDpi ;
     fNewMetric = fFactor * fDpiY ;
// Return new metric
     return (int) fNewMetric ;
}


//****************************************************************************
//                                                                End of Class
//****************************************************************************
}


//****************************************************************************
//                                                            End of Namespace
//****************************************************************************
}
