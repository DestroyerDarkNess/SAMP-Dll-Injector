Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.IO
Imports SAMPInjector.DestroyerSDK
Imports SAMPInjector.LogFuncs

Namespace Udrakoloader

    Public Class Plugin

        Public Shared CurrentDLLPath As String = Assembly.GetExecutingAssembly().Location
        Public Shared CurrentLocation As String = Path.GetDirectoryName(CurrentDLLPath)


        Public Shared GamePath As String = Assembly.GetCallingAssembly.Location
        Public Shared GameProcName As String = System.IO.Path.GetFileNameWithoutExtension(GamePath)

        Public Shared DLL_Folder As String = "SAMPInjector"
        Public Shared DLLPluginDir As String = CurrentLocation & "\" & DLL_Folder

        Public Shared Function EntryPoint(ByVal pwzArgument As String) As Integer
            Dim tskThread As New Task(ScriptAsyc, TaskCreationOptions.LongRunning)
            tskThread.Start()
            Return 0
        End Function

        Private Shared ScriptAsyc As New Action(
      Sub()

          If Not My.Computer.FileSystem.DirectoryExists(DLLPluginDir) = True Then
              My.Computer.FileSystem.CreateDirectory(DLLPluginDir)
          End If

          If My.Computer.FileSystem.FileExists(LogFile) = True Then
              My.Computer.FileSystem.DeleteFile(LogFile)
          End If

          WriteLog("Session started .", InfoType.Information)
          WriteLog(" ", InfoType.None)
          WriteLog("SAMP Injector v.01 beta loaded.", InfoType.Information)
          WriteLog("Developers: Destroyer", InfoType.Information)
          WriteLog("Copyright (c) 2020", InfoType.Information)
          WriteLog(" ", InfoType.None)
          WriteLog("Working directory: " & DLLPluginDir, InfoType.Information)


          WriteLog("Loop started .", InfoType.Information)
          Dim m_IsGameFullyLoaded As Boolean = Nothing
          Dim TimeElap As Integer = 0

          For i As Integer = 0 To 2
              TimeElap += 1

              m_IsGameFullyLoaded = CBool(ReadInteger("gta_sa", &HA444A0))

              If m_IsGameFullyLoaded = True Then
                  WriteLog("Loop End | SAMP Loaded. " & TimeElap, InfoType.Information)
                  Exit For
              End If
              i -= 1
          Next

          WriteLog("Start Scan . Find DLL's", InfoType.Information)
          Dim Files As List(Of String) = FileDirSearcher.GetFilePaths(DLLPluginDir, SearchOption.TopDirectoryOnly).ToList

          For Each Plug As String In Files
              If LCase(Path.GetExtension(Plug)) = ".dll" Then
                  Dim PlugName As String = Path.GetFileNameWithoutExtension(Plug)
                  Dim NativePlugInject As Boolean = Injector.InjectDLL("gta_sa", Plug)

                  If NativePlugInject = True Then
                      WriteLog("New Dll: " & PlugName, InfoType.None)
                  Else
                      WriteLog(PlugName & ": Native DLL died due to an error. Exeption : " & "[" & Injector.MsgInject & "]", InfoType.Critical)
                  End If
              End If
          Next

      End Sub)

    End Class

End Namespace

