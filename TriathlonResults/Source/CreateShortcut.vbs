'CreateShortcut.vbs
'Generates a .LNK shortcut to the specified program
'Usage: CreateShortcut.vbs <path to exe> <exeName> <shortcutName>

Dim args, arg
Set args = WScript.Arguments

Dim workingDirectory, exeName, shortcutName
workingDirectory=args(0)
exeName=args(1)
shortcutName=args(2)

Dim WshShell
Set WshShell = WScript.CreateObject("WScript.Shell")

Dim objLink
Set objLink = WshShell.CreateShortcut("Shortcuts\" & shortcutName & ".lnk")
objLink.Description = shortcutName
objLink.TargetPath = workingDirectory & "\" & exeName
objLink.WindowStyle = 1
objLink.WorkingDirectory = workingDirectory
objLink.Save

