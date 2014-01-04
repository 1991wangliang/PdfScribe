﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

using PdfScribeCore;

namespace PdfScribeInstallCustomAction
{
    /// <summary>
    /// Lotsa notes from here:
    /// http://stackoverflow.com/questions/835624/how-do-i-pass-msiexec-properties-to-a-wix-c-sharp-custom-action
    /// </summary>
    public class CustomActions
    {

        static readonly String traceSourceName = "PdfScribeInstaller";

        [CustomAction]
        public static ActionResult CheckIfPrinterNotInstalled(Session session)
        {
            ActionResult resultCode;
            TextWriterTraceListener installTraceListener = new TextWriterTraceListener("C:\\testout.txt");
            PdfScribeInstaller installer = new PdfScribeInstaller(traceSourceName);
            installer.AddTraceListener(installTraceListener);
            if (installer.IsPdfScribePrinterInstalled())
                resultCode = ActionResult.Success;
            else
                resultCode = ActionResult.Failure;

            return resultCode;
        }


        [CustomAction]
        public static ActionResult InstallPdfScribePrinter(Session session)
        {
            ActionResult printerInstalled;

            String driverSourceDirectory = session.CustomActionData["DriverSourceDirectory"];
            String outputCommand = session.CustomActionData["OutputCommand"];
            String outputCommandArguments = session.CustomActionData["OutputCommandArguments"];

            TextWriterTraceListener installTraceListener = new TextWriterTraceListener("C:\\testout.txt");
            installTraceListener.TraceOutputOptions = TraceOptions.Timestamp;

            PdfScribeInstaller installer = new PdfScribeInstaller(traceSourceName);

            installer.AddTraceListener(installTraceListener);


            if (installer.InstallPdfScribePrinter(driverSourceDirectory,
                                              outputCommand,
                                              outputCommandArguments))
                printerInstalled = ActionResult.Success;
            else
                printerInstalled = ActionResult.Failure;

            installTraceListener.Flush();
            installTraceListener.Close();

            return printerInstalled;
        }


        [CustomAction]
        public static ActionResult UninstallPdfScribePrinter()
        {
            ActionResult printerUninstalled;

            PdfScribeInstaller installer = new PdfScribeInstaller(traceSourceName);
            if (installer.UninstallPdfScribePrinter())
                printerUninstalled = ActionResult.Success;
            else
                printerUninstalled = ActionResult.Failure;

            return printerUninstalled;
        }
    }
}
