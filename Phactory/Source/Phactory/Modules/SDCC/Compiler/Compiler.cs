using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace SDCC.Controller
{
    public partial class Compiler : PhactoryHost.CompilerPlugin
    {
        public PhactoryHost.Host Host;

        public Compiler()
        {
            Plugin.ControllerCompiler = this;
        }

        public bool IsDefaultPluginForUnknownTypes()
        {
            return false;
        }

        public bool ShowSettings(Panel parentPanel)
        {
            return false;
        }

        public bool SaveSettings()
        {
            return false;
        }

        public void Register(PhactoryHost.Host parent)
        {
            this.Host = parent;
        }

        public string GetName()
        {
            return "SDCC Compiler";
        }

        public string GetDescription()
        {
            return "Z80 C/ASM Compiler";
        }

        public string GetVersion()
        {
            return "1.0";
        }

        public List<PhactoryHost.PluginExtension> GetSupportedExtensions()
        {
            List<PhactoryHost.PluginExtension> extensions = new List<PhactoryHost.PluginExtension>();
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".asm", "ASM Source File (*.asm)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".c", "C Source File (*.c)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".cxx", "C Include-Source File (*.cxx)", true));
            extensions.Add(new PhactoryHost.BaseClass.PluginExtension(".h", "Header File (*.h)", true));

            return extensions;
        }

        public bool IsResourceSupported(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            foreach (PhactoryHost.PluginExtension extension in GetSupportedExtensions())
            {
                if (String.Compare(extension.GetName(), fileInfo.Extension, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CompileASM(PhactoryHost.Database.Resource resource)
        {
            if ( resource.IdOutputs.Count == 1 )
            {
                PhactoryHost.Database.Resource outputResource = Host.GetResource(resource.IdOutputs[0]);

                if (Host.IsResourceReferencedByOtherResources(outputResource).Count == 0)
                {
                    // simply skip compilation
                    return true;
                }
            }

            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            string exeFullPath = Host.GetPluginsPath() + "Pasmo.exe";
            string arguments = "--bin \"" + fileInfo.FullName + "\" \"" + fileInfo.FullName.Replace(".asm", ".bin") + "\"";

            bool isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
            if (isOK)
            {
                if (Host.GetLastErrorOutput().ToUpper().IndexOf("ERROR") != -1)
                {
                    isOK = false;
                    Host.Log("\nCompilation failed with '" + resource.DisplayName + "'..\nLog output:\n" + Host.GetLastErrorOutput());
                }
            }

            return isOK;
        }

        public bool IsIncludeResource(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            if (fileInfo.Extension.ToLower() == ".h")
            {
                return true;
            }
            if (fileInfo.Extension.ToLower() == ".cxx")
            {
                return true;
            }
            
            return false;
        }

        public bool Compile(PhactoryHost.Database.Resource resource)
        {
            FileInfo fileInfo = Host.GetFileInfo(resource);            
            if (fileInfo == null)
            {
                return false;
            }

            if (!IsResourceSupported(resource))
            {
                return false;
            }

            if (fileInfo.Extension.ToLower() == ".asm")
            {
                return CompileASM(resource);
            }

            if (fileInfo.Extension.ToLower() != ".c")
            {
                // only compiles .C files (ignore .H)
                return true;
            }

            // check that .c file is not an include from an other .c file
            // (in that case, skip compile)
            var dependencyResources = Host.GetDependencyResources(resource);
            bool hasAsmDependentResource = false;
            foreach (PhactoryHost.Database.Resource resourceDependency in dependencyResources)
            {
                FileInfo parentFileInfo = Host.GetFileInfo(resourceDependency);
                if (parentFileInfo.Extension.ToLower() == ".asm")
                {
                    hasAsmDependentResource = true;
                }
            }
            if (!hasAsmDependentResource)
            {
                foreach (PhactoryHost.Database.Resource resourceDependency in dependencyResources)
                {
                    FileInfo parentFileInfo = Host.GetFileInfo(resourceDependency);
                    if (parentFileInfo.Extension.ToLower() == ".c")
                    {
                        return true;
                    }
                }
            }

            string asmFilename = fileInfo.FullName.ToLower().Replace(".c", ".asm");

            string exeFullPath = Host.GetPluginsPath() + "SDCC\\bin\\SDCC.EXE";
            string arguments = "-mz80 -S --no-std-crt0 \"" + fileInfo.FullName + "\" --disable-warning 59 --disable-warning 154 --disable-warning 110 --disable-warning 84 --disable-warning 112";

            bool isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
            if (isOK)
            {
                string standardOutput = Host.GetLastErrorOutput();
                standardOutput = standardOutput.Replace("\\/", "\\");

                string[] lineOutputs = standardOutput.Split('\n');
                foreach( string iLineOutput in lineOutputs )
                {
                    string lineOutput = iLineOutput;

                    if (!lineOutput.Contains("In file included from") &&
                        !lineOutput.Contains("missing terminating") &&
                        !lineOutput.Contains("z80instructionSize()") &&
                        (lineOutput.Length > 4))
                    {
                        int filenameLength = lineOutput.IndexOf(":", 3);
                        if (filenameLength != -1)
                        {
                            string filename = lineOutput.Substring(0, filenameLength);

                            FileInfo lineFileInfo = new FileInfo(filename);
                            PhactoryHost.Database.Resource lineResource = Host.GuessResource(lineFileInfo);
                            if (lineResource != null)
                            {
                                lineOutput = lineOutput.Replace(filename, lineResource.DisplayName);
                            }

                            if (lineOutput.IndexOf("error") != -1)
                            {
                                isOK = false;
                                Host.Log("\n" + lineOutput);
                                Host.Log("\nCompilation failed with '" + resource.DisplayName + "' !");
                                break;
                            }
                            else /*if (lineOutput.IndexOf(": warning") != -1)
                            {
                                if (Host.IsVerboseOutput())
                                {
                                    Host.Log("\n" + lineOutput);
                                }
                            }
                            else*/
                            {
                                Host.Log("\n" + lineOutput);
                            }
                        }
                        else
                        {
                            Host.Log("\n" + lineOutput);
                        }
                    }
                }
            }

            if (isOK)
            {
                exeFullPath = Host.GetPluginsPath() + "SDCC2Pasmo.EXE";
                arguments = "\"" + asmFilename + "\"";
                arguments += " \"" + asmFilename + ".maxam\"";

                isOK = Host.StartProcess(exeFullPath, arguments, fileInfo.DirectoryName, true);
                if (isOK)
                {
                    string standardOutput = Host.GetLastStandardOutput();

                    bool errorFound = false;
                    if (standardOutput.IndexOf("SDCC2Pasmo") != -1)
                    {
                        errorFound = true;
                    }

                    if (errorFound)
                    {
                        isOK = false;
                        Host.Log("\nCompilation failed with '" + resource.DisplayName + "'..\nLog output:\n" + Host.GetLastStandardOutput());
                    }
                }

                File.Delete(asmFilename);

                var maxamFilename = asmFilename + ".maxam";
                if (!File.Exists(maxamFilename))
                {
                    return false;
                }

                if (isOK)
                {
                    File.Move(maxamFilename, asmFilename);
                }
            }

            return isOK;
        }
    }
}
